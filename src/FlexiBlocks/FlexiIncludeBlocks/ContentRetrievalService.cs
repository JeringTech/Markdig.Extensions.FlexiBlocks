using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class ContentRetrievalService
    {
        /// <summary>
        /// Thread safe, in-memory content cache - https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<string[]>> _cache = new ConcurrentDictionary<string, Lazy<string[]>>();

        private readonly IHttpClientService _httpClientService;
        private readonly IFileService _fileService;
        private readonly ILogger<ContentRetrievalService> _logger;

        public ContentRetrievalService(IHttpClientService httpClientService, IFileService fileService, ILoggerFactory loggerFactory)
        {
            _httpClientService = httpClientService;
            _fileService = fileService;
            _logger = loggerFactory?.CreateLogger<ContentRetrievalService>();
        }

        public string[] GetContent(string absoluteUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _cache.GetOrAdd(absoluteUri, new Lazy<string[]>(() => GetContentCore(absoluteUri, cancellationToken))).Value;
        }

        // TODO would be great if this could be async, but methods down the stack are not async
        private string[] GetContentCore(string absoluteUri, CancellationToken cancellationToken)
        {
            // absoluteUri must be an absolute url or the absolute path of a file on disk. Full list of schemes: https://docs.microsoft.com/en-sg/dotnet/api/system.uri.scheme?view=netstandard-2.0#System_Uri_Scheme
            if (!Uri.TryCreate(absoluteUri, UriKind.Absolute, out Uri uri))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_UriMustBeAbsolute, absoluteUri));
            }

            if(uri.Scheme != "http" && uri.Scheme != "https" && uri.Scheme != "file")
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_UriSchemeUnsupported, absoluteUri, uri.Scheme));
            }

            // Local source
            if (uri.Scheme == "file")
            {
                // TODO what is the diff between LocalPath and AbsolutePath
                return _fileService.ReadAllLines(uri.LocalPath);
            }

            // Remote source

            // TODO try to retrieve from on disk cache
            // Try FileCache

            int remainingTries = 3;

            do
            {
                remainingTries--;

                try
                {
                    // TODO does this throw an aggregate exception? can it be avoided?
                    HttpResponseMessage response = _httpClientService.GetAsync(uri, cancellationToken).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Stream contentStream = response.Content.ReadAsStreamAsync().Result;

                        // This is exactly what File.ReadAllLines does - https://github.com/dotnet/corefx/blob/e267ad25d58459b90be7cea74ea11b9689daf191/src/System.IO.FileSystem/src/System/IO/File.cs#L449
                        string line;
                        var lines = new List<string>();

                        using (var streamReader = new StreamReader(contentStream))
                        {
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                lines.Add(line);
                            }
                        }

                        return lines.ToArray();
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // TODO no point retrying if server is up but content does not exist or user is forbidden
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // TODO no point retrying if server is up but content does not exist or user is forbidden
                    }
                    else
                    {
                        // TODO print content
                        _logger?.LogDebug(string.Format(Strings.HttpRequestException_UnsuccessfulRequest, absoluteUri, response.StatusCode));
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger?.LogDebug($"Attempt to retrieve content from \"{absoluteUri}\" timed out, {remainingTries} tries remaining.");
                }
                catch (HttpRequestException exception)
                {
                    _logger?.LogDebug($"A {nameof(HttpRequestException)} with message \"{exception.Message}\" occurred when attempting to retrieve content from \"{absoluteUri}\", {remainingTries} tries remaining.");
                }
                catch (Exception exception)
                {
                    // Unexpected exception
                    throw new ContentRetrievalException("Content retrieval failed with an unexpected exception. Refer to the inner exception for details.", exception);
                }

                // TODO sleep
            }
            while (remainingTries > 0);

            // remainingTries == 0
            throw new ContentRetrievalException($"Multiple attempts retrieve content from \"{absoluteUri}\" have failed. Please ensure that the Url is valid and that your network connection is stable.");
        }
    }
}
