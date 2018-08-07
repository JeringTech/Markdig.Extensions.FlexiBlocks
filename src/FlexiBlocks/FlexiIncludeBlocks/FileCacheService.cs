using Jering.IocServices.System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// <para>The default implementation of <see cref="IFileCacheService"/>.</para>
    /// <para>This is an extremely simple caching service, without policies, size limits etc. Additional features 
    /// may be added if the need for them arises.</para>
    /// </summary>
    public class FileCacheService : IFileCacheService
    {
        private readonly FileCacheServiceOptions _fileCacheOptions;
        private readonly ILogger<FileCacheService> _logger;
        private readonly IFileService _fileService;

        /// <summary>
        /// Creates an <see cref="IFileCacheService"/> instance.
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="fileService"></param>
        /// <param name="loggerFactory"></param>
        public FileCacheService(IOptions<FileCacheServiceOptions> optionsAccessor, IFileService fileService, ILoggerFactory loggerFactory)
        {
            _fileCacheOptions = optionsAccessor?.Value ?? new FileCacheServiceOptions();
            _logger = loggerFactory?.CreateLogger<FileCacheService>();
            _fileService = fileService;
        }

        /// <summary>
        /// Attempts to get a read only file stream for the cache file with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the file to get a read only file stream for.</param>
        /// <param name="readOnlyFileStream">The read only file stream for the file.</param>
        /// <returns>true if a cache file with the specified identifier exists, false otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains invalid path characters.</exception>
        /// <exception cref="PathTooLongException">Thrown if the file path for the specified identifier is too long.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="IOException">Thrown if the cache file is in use and remains in use on the third try to open it.</exception>
        public bool TryGetReadOnlyFileStream(string identifier, out FileStream readOnlyFileStream)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_CannotBeNullWhiteSpaceOrAnEmptyString, nameof(identifier)));
            }

            string filePath = CreatePath(identifier);

            if (!_fileService.Exists(filePath))
            {
                readOnlyFileStream = null;
                return false;
            }

            int remainingTries = 3;

            while(true)
            {
                remainingTries--;

                try
                {
                    readOnlyFileStream = _fileService.Open(filePath,
                        FileMode.Open, // Throw if file was removed between _fileService.Exists and this call
                        FileAccess.Read, // Read only access
                        FileShare.Read); // Allow other threads to read the file but prevent writing until we are done reading

                    return true;
                }
                catch (Exception exception) when (exception is DirectoryNotFoundException || exception is FileNotFoundException)
                {
                    // File does not exist (something happened to it between the _fileService.Exists call and the _fileService.Open call)
                    readOnlyFileStream = null;
                    return false;
                }
                catch (IOException)
                {
                    _logger?.LogDebug($"The file \"{filePath}\" is in use. {remainingTries} tries remaining.");

                    if(remainingTries == 0)
                    {
                        throw;
                    }

                    Thread.Sleep(50);
                }

                // Other exceptions (PathTooLongException, UnauthorizedAccessException and ArgumentException) can't be recovered from, so we don't bother 
                // catching them.
            }
        }

        /// <summary>
        /// Gets a write only file stream for the cache file with the specified identifier. If the file doesn't already exist, creates the file.
        /// </summary>
        /// <param name="identifier">The identifier of the file to get a write only file stream for.</param>
        /// <returns>A write only file stream for the cache file with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains invalid path characters.</exception>
        /// <exception cref="PathTooLongException">Thrown if the file path for the specified identifier is too long.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="IOException">Thrown if the cache file is in use and remains in use on the third try to open it.</exception>
        public FileStream GetWriteOnlyFileStream(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_CannotBeNullWhiteSpaceOrAnEmptyString, nameof(identifier)));
            }

            string filePath = CreatePath(identifier);

            int remainingTries = 3;

            while (true)
            {
                remainingTries--;

                try
                {
                    return _fileService.Open(filePath,
                        FileMode.OpenOrCreate, // Create file if it doesn't already exist
                        FileAccess.Write, // Write only access
                        FileShare.None); // Don't allow other threads to access the file while we write to it
                }
                catch (IOException)
                {
                    _logger?.LogDebug($"The file \"{filePath}\" is in use. {remainingTries} tries remaining.");

                    if (remainingTries == 0)
                    {
                        throw;
                    }

                    Thread.Sleep(50);
                }

                // Other exceptions (PathTooLongException, UnauthorizedAccessException and ArgumentException) can't be recovered from, so we don't bother 
                // catching them.
            }
        }

        internal virtual string CreatePath(string identifier)
        {
            return Path.Combine(_fileCacheOptions.RootDirectory, identifier, ".txt");
        }
    }
}
