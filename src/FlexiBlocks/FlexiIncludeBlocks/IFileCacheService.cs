using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// An abstraction for caching data in files on disk. 
    /// </summary>
    public interface IFileCacheService
    {
        bool TryGetReadOnlyFileStream(string identifier, out FileStream readOnlyFileStream);
        FileStream GetWriteOnlyFileStream(string identifier);
    }
}
