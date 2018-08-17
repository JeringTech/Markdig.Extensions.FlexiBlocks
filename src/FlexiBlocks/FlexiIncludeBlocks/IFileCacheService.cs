using System.IO;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// An abstraction for caching data in files on disk. 
    /// </summary>
    public interface IFileCacheService
    {
        /// <summary>
        /// Attempts to get a read-only file stream for the cache file with the specified identifier in the specified cache directory. This method 
        /// serves as a way to check if a cache file exists and to retrieve it if it does, in one operation. This method returns a read-only 
        /// file stream because cache files aren't meant to be edited once they've been created.
        /// </summary>
        /// <param name="identifier">The identifier of the file to get a read only file stream for.</param>
        /// <param name="cacheDirectory">The directory to look for a file with the specified identifier.</param>
        /// <returns>A (bool, FileStream) tuple. The bool is true if a cache file with the specified identifier exists, and false otherwise.
        /// The FileStream is defined if the a cache file with the specified identifier exists, and null otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cacheDirectory"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains invalid path characters.</exception>
        /// <exception cref="PathTooLongException">Thrown if the file path for the specified identifier is too long.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="IOException">Thrown if the cache file is in use and remains in use on the third try to open it.</exception>
        (bool, FileStream) TryGetCacheFile(string identifier, string cacheDirectory);

        /// <summary>
        /// Creates a cache file with the specified identifier in the specified cache directory. Gets a read-write file stream for the file. If the 
        /// file already exists, returns a read-write file stream for it. This method retuns a readable stream since a network stream can only 
        /// be read once - thereafter, the file stream must be read for in-memory caching.
        /// </summary>
        /// <param name="identifier">The identifier of the file to create or get.</param>
        /// <param name="cacheDirectory">The directory to look for a file with the specified identifier.</param>
        /// <returns>A write only file stream for the cache file with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cacheDirectory"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains invalid path characters.</exception>
        /// <exception cref="PathTooLongException">Thrown if the file path for the specified identifier is too long.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="IOException">Thrown if the cache file is in use and remains in use on the third try to open it.</exception>
        FileStream CreateOrGetCacheFile(string identifier, string cacheDirectory);
    }
}
