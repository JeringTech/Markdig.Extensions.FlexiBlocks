using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// <para>The default implementation of <see cref="IContentRetrieverService"/>.</para>
    /// <para>This service caches all retrieved content in memory. Additionally, it caches content retrieved from remote sources on disk.</para>
    /// </summary>
    public class ContentRetrieverService : IContentRetrieverService
    {
        // We only support a subset of schemes. For the full list of schemes, see https://docs.microsoft.com/en-sg/dotnet/api/system.uri.scheme?view=netstandard-2.0#System_Uri_Scheme
        private static readonly string[] _supportedSchemes = new string[] { "file", "http", "https" };
        private static readonly Dictionary<int, char> _decimalToHex = new Dictionary<int, char>
        {
            {0, '0' }, {1, '1' }, {2, '2' }, {3, '3' }, {4, '4' }, {5, '5' }, {6, '6' }, {7, '7' },
            {8, '8' }, {9, '9' }, { 10, 'A' }, {11, 'B'}, {12, 'C'}, {13, 'D'}, {14, 'E'}, {15, 'F'}
        };

        /// <summary>
        /// Thread safe, in-memory content cache - https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<ReadOnlyCollection<string>>> _cache = new ConcurrentDictionary<string, Lazy<ReadOnlyCollection<string>>>();
        private readonly MD5 _mD5 = MD5.Create();
        private readonly IHttpClientService _httpClientService;
        private readonly IFileCacheService _fileCacheService;
        private readonly IFileService _fileService;
        private readonly ILogger<ContentRetrieverService> _logger;

        /// <summary>
        /// Creates a <see cref="ContentRetrieverService"/> instance.
        /// </summary>
        /// <param name="httpClientService"></param>
        /// <param name="fileService"></param>
        /// <param name="fileCacheService"></param>
        /// <param name="loggerFactory"></param>
        public ContentRetrieverService(IHttpClientService httpClientService,
            IFileService fileService,
            IFileCacheService fileCacheService,
            ILoggerFactory loggerFactory)
        {
            _fileService = fileService;
            _httpClientService = httpClientService;
            _fileCacheService = fileCacheService;
            _logger = loggerFactory?.CreateLogger<ContentRetrieverService>();
        }

        /// <inheritdoc />
        public virtual ReadOnlyCollection<string> GetContent(string source, string cacheDirectory = null, string sourceBaseUri = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, nameof(source)));
            }

            if (!Uri.TryCreate(source, UriKind.Absolute, out Uri uri)) // source is not an absolute URI
            {
                if (string.IsNullOrWhiteSpace(sourceBaseUri))
                {
                    throw new ArgumentException(string.Format(Strings.ArgumentException_BaseUriMustBeDefinedIfSourceIsNotAnAbsoluteUri, nameof(sourceBaseUri)));
                }

                // Create base URI. A base URI must be absolute, see http://www.ietf.org/rfc/rfc3986.txt, section 5.1
                if (!Uri.TryCreate(sourceBaseUri, UriKind.Absolute, out Uri baseUri))
                {
                    throw new ArgumentException(string.Format(Strings.ArgumentException_BaseUriMustBeAbsolute, sourceBaseUri));
                }

                if (!Uri.TryCreate(baseUri, source, out uri)) // source is not a relative uri
                {
                    // Source is neither a valid relative URI nor a valid absolute URI
                    throw new ArgumentException(string.Format(Strings.ArgumentException_NotAValidUri, source));
                }
            }

            if (!_supportedSchemes.Contains(uri.Scheme))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_UriSchemeUnsupported, uri.AbsoluteUri, uri.Scheme));
            }

            return _cache.GetOrAdd(uri.AbsoluteUri, _ => new Lazy<ReadOnlyCollection<string>>(() => GetContentCore(uri, cacheDirectory, cancellationToken))).Value;
        }

        internal virtual ReadOnlyCollection<string> GetContentCore(Uri uri, string cacheDirectory, CancellationToken cancellationToken)
        {
            if (uri.Scheme == "file")
            {
                // Local source
                using (FileStream fileStream = _fileService.Open(uri.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return ReadAndNormalizeAllLines(fileStream);
                }
            }

            // Remote source
            return GetRemoteContent(uri, cacheDirectory, cancellationToken);
        }

        internal virtual ReadOnlyCollection<string> GetRemoteContent(Uri uri, string cacheDirectory, CancellationToken cancellationToken)
        {
            string cacheIdentifier = null;
            if (!string.IsNullOrWhiteSpace(cacheDirectory)) // Don't try to retrieve from file cache if no cache directoy is specified
            {
                cacheIdentifier = GetCacheIdentifier(uri.AbsoluteUri);

                (bool success, FileStream readOnlyFileStream) = _fileCacheService.TryGetCacheFile(cacheIdentifier, cacheDirectory);
                if (success)
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
                    response = _httpClientService.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        Stream contentStream = null;
                        if (cacheIdentifier != null) // If file caching is requested
                        {
                            contentStream = _fileCacheService.CreateOrGetCacheFile(cacheIdentifier, cacheDirectory);

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
                        throw new ContentRetrieverException(string.Format(Strings.ContentRetrieverException_RemoteUriDoesNotExist, uri.AbsoluteUri));
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // No point retrying if server is responsive but access to the content is forbidden
                        throw new ContentRetrieverException(string.Format(Strings.ContentRetrieverException_RemoteUriAccessForbidden, uri.AbsoluteUri));
                    }
                    else
                    {
                        // Might be a random internal server error or some intermittent network issue
                        _logger?.LogDebug($"Http request to \"{uri.AbsoluteUri} failed with status code \"{response.StatusCode}\".");
                    }
                }
                catch (OperationCanceledException) // HttpClient.GetAsync throws OperationCanceledException on timeout - https://github.com/dotnet/corefx/blob/25d0f5c20edddbf872d17fa699b4279c0827c320/src/System.Net.Http/src/System/Net/Http/HttpClient.cs#L536
                {
                    _logger?.LogDebug($"Attempt to retrieve content from \"{uri.AbsoluteUri}\" timed out, {remainingTries} tries remaining.");
                }
                catch (HttpRequestException exception)
                {
                    // HttpRequestException is the general exception used for several situations, some of which may be intermittent.
                    _logger?.LogDebug($"A {nameof(HttpRequestException)} with message \"{exception.Message}\" occurred when attempting to retrieve content from \"{uri.AbsoluteUri}\", {remainingTries} tries remaining.");
                }
                finally
                {
                    response?.Dispose();
                }

                Thread.Sleep(1000);
            }
            while (remainingTries > 0);

            // remainingTries == 0
            throw new ContentRetrieverException(string.Format(Strings.ContentRetrieverException_FailedAfterMultipleAttempts, uri.AbsoluteUri));
        }

        /// <summary>
        /// Hashes a URI to create a unique cache identifier that does not contain illegal characters and has a fixed length.
        /// </summary>
        /// <param name="absoluteUri"></param>
        internal virtual string GetCacheIdentifier(string absoluteUri)
        {
            int byteCount = Encoding.UTF8.GetByteCount(absoluteUri);
            byte[] bytes = null;
            byte[] hashBytes = null;
            try
            {
                bytes = ArrayPool<byte>.Shared.Rent(byteCount);
                Encoding.UTF8.GetBytes(absoluteUri, 0, absoluteUri.Length, bytes, 0);
#if NETSTANDARD1_3
                hashBytes = _mD5.ComputeHash(bytes, 0, byteCount);
#elif NETSTANDARD2_0
                hashBytes = ArrayPool<byte>.Shared.Rent(16);
                _mD5.TransformBlock(bytes, 0, bytes.Length, hashBytes, 0);
#endif

                var hex = new StringBuilder();
                foreach (byte hashByte in hashBytes)
                {
                    hex.Append(_decimalToHex[(hashByte / 16) % 16]);
                    hex.Append(_decimalToHex[hashByte % 16]);
                }

                return hex.ToString();
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
