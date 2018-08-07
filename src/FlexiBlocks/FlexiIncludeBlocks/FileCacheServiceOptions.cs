using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class FileCacheServiceOptions
    {
        public string RootDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "FileCache");
    }
}
