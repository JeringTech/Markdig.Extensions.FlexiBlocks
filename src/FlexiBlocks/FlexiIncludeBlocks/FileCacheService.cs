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

        /// <inheritdoc />
        public bool TryGetCacheFile(string identifier, out FileStream readOnlyFileStream)
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

            try
            {
                readOnlyFileStream = GetStream(filePath,
                        FileMode.Open, // Throw if file was removed between _fileService.Exists and this call
                        FileAccess.Read, // Read only access
                        FileShare.Read); // Don't allow other threads to write to the file while we read from it
                return true;
            }
            catch (Exception exception) when (exception is DirectoryNotFoundException || exception is FileNotFoundException)
            {
                // File does not exist (something happened to it between the _fileService.Exists call and the _fileService.Open call in GetStream)
                readOnlyFileStream = null;
                return false;
            }
        }

        /// <inheritdoc />
        public FileStream CreateOrGetCacheFile(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_CannotBeNullWhiteSpaceOrAnEmptyString, nameof(identifier)));
            }

            string filePath = CreatePath(identifier);

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

        internal virtual string CreatePath(string identifier)
        {
            return Path.Combine(_fileCacheOptions.RootDirectory, identifier, ".txt");
        }
    }
}
