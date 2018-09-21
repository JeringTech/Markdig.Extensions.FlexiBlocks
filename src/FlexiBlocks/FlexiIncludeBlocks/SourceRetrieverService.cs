using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// <para>The default implementation of <see cref="ISourceRetrieverService"/>.</para>
    /// <para>This service caches all retrieved content in memory. Additionally, it caches content retrieved from remote sources on disk.</para>
    /// </summary>
    public class SourceRetrieverService : ISourceRetrieverService
    {
        /// <summary>
        /// Thread safe, in-memory content cache - https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<ReadOnlyCollection<string>>> _cache = new ConcurrentDictionary<string, Lazy<ReadOnlyCollection<string>>>();
        private readonly IHttpClientService _httpClientService;
        private readonly IDiskCacheService _diskCacheService;
        private readonly IFileService _fileService;
        private readonly ILogger<SourceRetrieverService> _logger;

        /// <summary>
        /// Creates a <see cref="SourceRetrieverService"/> instance.
        /// </summary>
        /// <param name="httpClientService">The service that will handle HTTP requests.</param>
        /// <param name="fileService">The service that will handle file operations.</param>
        /// <param name="diskCacheService">The service that will handle the disk cache.</param>
        /// <param name="loggerFactory">The factory for <see cref="ILogger"/>s.</param>
        public SourceRetrieverService(IHttpClientService httpClientService,
            IFileService fileService,
            IDiskCacheService diskCacheService,
            ILoggerFactory loggerFactory)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _diskCacheService = diskCacheService ?? throw new ArgumentNullException(nameof(diskCacheService));
            _logger = loggerFactory?.CreateLogger<SourceRetrieverService>();
        }

        /// <inheritdoc />
        public virtual ReadOnlyCollection<string> GetSource(Uri sourceUri, string cacheDirectory = null, CancellationToken cancellationToken = default)
        {
            if (sourceUri == null)
            {
                throw new ArgumentNullException(nameof(sourceUri));
            }

            return _cache.GetOrAdd(sourceUri.AbsoluteUri, _ => new Lazy<ReadOnlyCollection<string>>(() => GetSourceCore(sourceUri, cacheDirectory, cancellationToken))).Value;
        }

        internal virtual ReadOnlyCollection<string> GetSourceCore(Uri sourceUri, string cacheDirectory, CancellationToken cancellationToken)
        {
            if (sourceUri.Scheme == "file")
            {
                // Local source
                try
                {
                    using (FileStream fileStream = _fileService.Open(sourceUri.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        return ReadAndNormalizeAllLines(fileStream);
                    }
                }
                catch (Exception exception)
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_InvalidLocalUri, sourceUri.AbsolutePath), exception);
                }
            }

            // Remote source
            return GetRemoteSource(sourceUri, cacheDirectory, cancellationToken);
        }

        internal virtual ReadOnlyCollection<string> GetRemoteSource(Uri sourceUri, string cacheDirectory, CancellationToken cancellationToken)
        {
            bool cacheOnDisk = !string.IsNullOrWhiteSpace(cacheDirectory);
            if (cacheOnDisk) // Don't try to retrieve from file cache if no cache directoy is specified
            {
                FileStream readOnlyFileStream = _diskCacheService.TryGetCacheFile(sourceUri.AbsoluteUri, cacheDirectory);
                if (readOnlyFileStream != null)
                {
                    using (readOnlyFileStream)
                    {
                        return ReadAndNormalizeAllLines(readOnlyFileStream);
                    }
                }
            }

            int remainingTries = 3;
            do
            {
                remainingTries--;

                HttpResponseMessage response = null;
                try
                {
                    response = _httpClientService.GetAsync(sourceUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        Stream contentStream = null;
                        if (cacheOnDisk) // If file caching is requested
                        {
                            contentStream = _diskCacheService.CreateOrGetCacheFile(sourceUri.AbsoluteUri, cacheDirectory);

                            // If contentStream.Length > 0, file was created between TryGetCacheFile and GetOrCreateCacheFile calls, no need to write to it
                            if (contentStream.Length == 0)
                            {
                                // TODO try copying to file stream and generating lines in 1 pass
                                response.Content.CopyToAsync(contentStream).GetAwaiter().GetResult();

                                // Rewind file stream (use file stream as content stream since NetworkStream can't be rewound).
                                contentStream.Position = 0;
                            }
                        }
                        else
                        {
                            contentStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                        }

                        return ReadAndNormalizeAllLines(contentStream);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // No point retrying if server is responsive but content does not exist
                        throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_RemoteUriDoesNotExist, sourceUri.AbsoluteUri));
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // No point retrying if server is responsive but access to the content is forbidden
                        throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_RemoteUriAccessForbidden, sourceUri.AbsoluteUri));
                    }
                    else
                    {
                        // Might be a random internal server error or some intermittent network issue
                        _logger?.LogDebug($"Http request to \"{sourceUri.AbsoluteUri} failed with status code \"{response.StatusCode}\".");
                    }
                }
                catch (OperationCanceledException) // HttpClient.GetAsync throws OperationCanceledException on timeout - https://github.com/dotnet/corefx/blob/25d0f5c20edddbf872d17fa699b4279c0827c320/src/System.Net.Http/src/System/Net/Http/HttpClient.cs#L536
                {
                    _logger?.LogDebug($"Attempt to retrieve content from \"{sourceUri.AbsoluteUri}\" timed out, {remainingTries} tries remaining.");
                }
                catch (HttpRequestException exception)
                {
                    // HttpRequestException is the general exception used for several situations, some of which may be intermittent.
                    _logger?.LogDebug($"A {nameof(HttpRequestException)} with message \"{exception.Message}\" occurred when attempting to retrieve content from \"{sourceUri.AbsoluteUri}\", {remainingTries} tries remaining.");
                }
                finally
                {
                    response?.Dispose();
                }

                Thread.Sleep(1000);
            }
            while (remainingTries > 0);

            // remainingTries == 0
            throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_FlexiIncludeBlocks_FailedAfterMultipleAttempts, sourceUri.AbsoluteUri));
        }

        internal virtual ReadOnlyCollection<string> ReadAndNormalizeAllLines(Stream stream)
        {
            // This is exactly what File.ReadAllLines does - https://github.com/dotnet/corefx/blob/e267ad25d58459b90be7cea74ea11b9689daf191/src/System.IO.FileSystem/src/System/IO/File.cs#L449
            string line;
            var lines = new List<string>();

            using (var streamReader = new StreamReader(stream))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    // Replace null characters to prevent null byte injection. A description of such attacks can be found here: http://projects.webappsec.org/w/page/13246949/Null%20Byte%20Injection.
                    // Replacing null characters is recommended by the commonmark spec: https://spec.commonmark.org/0.28/#insecure-characters. The risk of null byte injection attacks is exacerbated by
                    // the fact that this class can read remote content, it is like that some users may read from untrusted sources. Additionally, users may add extensions that are vulnerable to null 
                    // byte injection attacks. It is worth the performance hit to remove all null characters.
                    lines.Add(line.Replace('\0', '\uFFFD'));
                }
            }

            return lines.AsReadOnly();
        }
    }
}
