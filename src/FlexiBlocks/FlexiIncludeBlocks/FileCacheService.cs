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
    /// <para>This is an extremely simple caching service, it lacks policies (expiration etc), size limits and other bells and whistles. Additional features 
    /// will be added as the need for them arises.</para>
    /// </summary>
    public class FileCacheService : IFileCacheService
    {
        private readonly ILogger<FileCacheService> _logger;
        private readonly IFileService _fileService;
        private readonly IDirectoryService _directoryService;

        /// <summary>
        /// Creates an <see cref="IFileCacheService"/> instance.
        /// </summary>
        /// <param name="fileService"></param>
        /// <param name="directoryService"></param>
        /// <param name="loggerFactory"></param>
        public FileCacheService(IFileService fileService, IDirectoryService directoryService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<FileCacheService>();
            _fileService = fileService;
            _directoryService = directoryService;
        }

        /// <inheritdoc />
        public (bool, FileStream) TryGetCacheFile(string identifier, string cacheDirectory)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, nameof(identifier)));
            }

            if (string.IsNullOrWhiteSpace(cacheDirectory))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, nameof(cacheDirectory)));
            }

            string filePath = CreatePath(identifier, cacheDirectory);

            if (!_fileService.Exists(filePath))
            {
                return (false, null);
            }

            try
            {
                FileStream result = GetStream(filePath,
                        FileMode.Open, // Throw if file was removed between _fileService.Exists and this call
                        FileAccess.Read, // Read only access
                        FileShare.Read); // Don't allow other threads to write to the file while we read from it

                return (true, result);
            }
            catch (Exception exception) when (exception is DirectoryNotFoundException || exception is FileNotFoundException)
            {
                // File does not exist (something happened to it between the _fileService.Exists call and the _fileService.Open call in GetStream)
                return (false, null);
            }
        }

        /// <inheritdoc />
        public FileStream CreateOrGetCacheFile(string identifier, string cacheDirectory)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, nameof(identifier)));
            }

            if (string.IsNullOrWhiteSpace(cacheDirectory))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString, nameof(cacheDirectory)));
            }

            // Ensure that cache directory string is valid and that the directory exists
            try
            {
                _directoryService.CreateDirectory(cacheDirectory);
            }
            catch(Exception exception)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidCacheDirectory, cacheDirectory), exception);
            }

            string filePath = CreatePath(identifier, cacheDirectory);

            return GetStream(filePath,
                FileMode.OpenOrCreate, // Create file if it doesn't already exist
                FileAccess.ReadWrite, // Read and write access
                FileShare.None); // Don't allow other threads to access the file while we write to it
        }

        /// <summary>
        /// Open a file stream, retrying up to 3 times if the file is in use.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="fileShare"></param>
        internal virtual FileStream GetStream(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            int remainingTries = 3;

            while (true)
            {
                remainingTries--;

                try
                {
                    return _fileService.Open(path, fileMode, fileAccess, fileShare);
                }
                catch (IOException)
                {
                    _logger?.LogDebug($"The file \"{path}\" is in use. {remainingTries} tries remaining.");

                    if (remainingTries == 0)
                    {
                        throw;
                    }

                    Thread.Sleep(50);
                }

                // Other exceptions (FileNotFoundException, DirectoryNotFoundException, PathTooLongException, UnauthorizedAccessException and ArgumentException) 
                // will almost certainly continue to be thrown however many times we retry, so just let them propagate.
            }
        }

        internal virtual string CreatePath(string identifier, string cacheDirectory)
        {
            return Path.Combine(cacheDirectory, $"{identifier}.txt");
        }
    }
}
