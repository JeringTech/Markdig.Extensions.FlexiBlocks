using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class ContentRetrievalService
    {
        /// <summary>
        /// Thread safe, in-memory content cache - https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<ReadOnlyCollection<string>>> _cache = new ConcurrentDictionary<string, Lazy<ReadOnlyCollection<string>>>();
        private readonly MD5 _mD5 = MD5.Create();
        private readonly IHttpClientService _httpClientService;
        private readonly IFileCacheService _fileCacheService;
        private readonly ILogger<ContentRetrievalService> _logger;
        private readonly IFileService _fileService;
        private readonly ContentRetrievalServiceOptions _options;
        private readonly Uri _baseUri;

        public ContentRetrievalService(IHttpClientService httpClientService,
            IFileService fileService,
            IFileCacheService fileCacheService,
            ILoggerFactory loggerFactory,
            IOptions<ContentRetrievalServiceOptions> optionsAccessor)
        {
            _fileService = fileService;
            _httpClientService = httpClientService;
            _fileCacheService = fileCacheService;
            _logger = loggerFactory?.CreateLogger<ContentRetrievalService>();
            _options = optionsAccessor?.Value ?? new ContentRetrievalServiceOptions();

            if (!Uri.TryCreate(_options.RootPath, UriKind.Absolute, out _baseUri))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_UriMustBeAbsolute, _options.RootPath));
            }
        }

        /// <summary>
        /// Return type is read only for thread safety
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ReadOnlyCollection<string> GetContent(string source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _cache.GetOrAdd(source, _ => new Lazy<ReadOnlyCollection<string>>(() => GetContentCore(source, cancellationToken))).Value;
        }

        // Full list of schemes: https://docs.microsoft.com/en-sg/dotnet/api/system.uri.scheme?view=netstandard-2.0#System_Uri_Scheme
        private ReadOnlyCollection<string> GetContentCore(string source, CancellationToken cancellationToken)
        {
            if (!Uri.TryCreate(_baseUri, source, out Uri uri) // source is not a relative uri
                && !Uri.TryCreate(source, UriKind.Absolute, out uri)) // source is not an absolute uri
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_NotAUri, source));
            }

            if (uri.Scheme == "file")
            {
                // Local source
                return new ReadOnlyCollection<string>(_fileService.ReadAllLines(uri.AbsolutePath));
            }

            if(uri.Scheme != "http" && uri.Scheme != "https")
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_UriSchemeUnsupported, uri.AbsolutePath, uri.Scheme));
            }

            // Remote source
            return GetRemoteContent(uri, cancellationToken);
        }

        internal virtual ReadOnlyCollection<string> GetRemoteContent(Uri uri, CancellationToken cancellationToken)
        {
            string cacheIdentifier = GetCacheIdentifier(uri.AbsolutePath);
            FileStream readOnlyFileStream = null;
            try
            {
                // Try to retrieve file from cache
                if (_fileCacheService.TryGetReadOnlyFileStream(cacheIdentifier, out readOnlyFileStream))
                {
                    return ReadAllLines(readOnlyFileStream);
                }
            }
            catch (Exception exception)
            {
                readOnlyFileStream?.Dispose();

                throw new ContentRetrievalException($"Failed to retrieve content from \"{uri.AbsolutePath}\". Refer to the inner exception for details.", exception);
            }
            finally
            {
                readOnlyFileStream?.Dispose();
            }

            int remainingTries = 3;
            do
            {
                remainingTries--;

                try
                {
                    HttpResponseMessage response = _httpClientService.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        // TODO how does HttpCompletionOption.ResponseHeadersRead affect ReadAsStream? should there be a using around response?
                        Stream contentStream = response.Content.ReadAsStreamAsync().Result;
                        FileStream writeOnlyFileStream = _fileCacheService.GetWriteOnlyFileStream(cacheIdentifier);
                        // TODO copy to writeOnlyFileStream and get lines in 1 pass
                        contentStream.CopyTo(writeOnlyFileStream);

                        // TODO Move to start of stream again? might not work since a httpcontentstream does not keep all the data in the buffer
                        contentStream.Position = 0;

                        return new ReadOnlyCollection<string>(ReadAllLines(contentStream));
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
                        // TODO might be a service unavailable or random 500 error, retry if retries remain
                        // TODO print content
                        _logger?.LogDebug(string.Format(Strings.HttpRequestException_UnsuccessfulRequest, uri.AbsolutePath, response.StatusCode));
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger?.LogDebug($"Attempt to retrieve content from \"{uri.AbsolutePath}\" timed out, {remainingTries} tries remaining.");
                }
                catch (HttpRequestException exception)
                {
                    // TODO When does this get thrown? when status code is fail? Read through source to figure this out
                    _logger?.LogDebug($"A {nameof(HttpRequestException)} with message \"{exception.Message}\" occurred when attempting to retrieve content from \"{uri.AbsolutePath}\", {remainingTries} tries remaining.");
                }
                catch (Exception exception)
                {
                    // TODO exception list, No use retrying
                    throw new ContentRetrievalException($"Failed to retrieve content from \"{uri.AbsolutePath}\". Refer to the inner exception for details.", exception);
                }

                Thread.Sleep(1000);
            }
            while (remainingTries > 0);

            // remainingTries == 0
            throw new ContentRetrievalException($"Multiple attempts retrieve content from \"{uri.AbsolutePath}\" have failed. Please ensure that the Url is valid and that your network connection is stable.");
        }

        /// <summary>
        /// Hashes a path to create a unique cache identifier that does not contain illegal characters and has a fixed length.
        /// </summary>
        /// <param name="absolutePath"></param>
        internal virtual string GetCacheIdentifier(string absolutePath)
        {
            int byteCount = Encoding.UTF8.GetByteCount(absolutePath);
            byte[] bytes = null;
            byte[] hashBytes = null;
            try
            {
                bytes = ArrayPool<byte>.Shared.Rent(byteCount);
                Encoding.UTF8.GetBytes(absolutePath, 0, absolutePath.Length, bytes, 0);
#if NETSTANDARD1_3
                hashBytes = _mD5.ComputeHash(bytes);
#elif NETSTANDARD2_0
                hashBytes = ArrayPool<byte>.Shared.Rent(16);
                _mD5.TransformBlock(bytes, 0, bytes.Length, hashBytes, 0);
#endif

                return BitConverter.ToString(hashBytes);
            }
            finally
            {
                if (bytes != null)
                {
                    ArrayPool<byte>.Shared.Return(bytes);
                }

#if NETSTANDARD2_0
                if (hashBytes != null)
                {
                    ArrayPool<byte>.Shared.Return(hashBytes);
                }
#endif
            }
        }

        private ReadOnlyCollection<string> ReadAllLines(Stream stream)
        {
            // This is exactly what File.ReadAllLines does - https://github.com/dotnet/corefx/blob/e267ad25d58459b90be7cea74ea11b9689daf191/src/System.IO.FileSystem/src/System/IO/File.cs#L449
            string line;
            var lines = new List<string>();

            using (var streamReader = new StreamReader(stream))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.AsReadOnly();
        }
    }
}
