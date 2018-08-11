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
        /// Attempts to get a read-only file stream for the cache file with the specified identifier. This method serves as a way to check if 
        /// a cache file exists and to retrieve it if it does, in one go. This method returns a read-only file stream because cache files
        /// aren't meant to be edited once they've been created.
        /// </summary>
        /// <param name="identifier">The identifier of the file to get a read only file stream for.</param>
        /// <param name="readOnlyFileStream">The read only file stream for the file.</param>
        /// <returns>true if a cache file with the specified identifier exists, false otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains invalid path characters.</exception>
        /// <exception cref="PathTooLongException">Thrown if the file path for the specified identifier is too long.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="IOException">Thrown if the cache file is in use and remains in use on the third try to open it.</exception>
        bool TryGetCacheFile(string identifier, out FileStream readOnlyFileStream);

        /// <summary>
        /// Creates a cache file with the specified identifier and gets a read-write file stream for it. If the file already exists, returns
        /// a read-write file stream for it. This method retuns a readable stream since a network stream can only be read once - thereafter,
        /// the file stream must be read for in-memory caching.
        /// </summary>
        /// <param name="identifier">The identifier of the file to get a write only file stream for.</param>
        /// <returns>A write only file stream for the cache file with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is null, white space, an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains invalid path characters.</exception>
        /// <exception cref="PathTooLongException">Thrown if the file path for the specified identifier is too long.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="IOException">Thrown if the cache file is in use and remains in use on the third try to open it.</exception>
        FileStream CreateOrGetCacheFile(string identifier);
    }
}
