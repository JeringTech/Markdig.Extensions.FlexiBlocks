using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class FlexiIncludeBlocksExtensionOptions : IExtensionOptions<FlexiIncludeBlocksExtensionOptions>
    {
        public string SourceBaseUri { get; set; } = Directory.GetCurrentDirectory() + "/";
        public string FileCacheDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "FileCache");

        public void CopyTo(FlexiIncludeBlocksExtensionOptions target)
        {
            target.SourceBaseUri = SourceBaseUri;
            target.FileCacheDirectory = FileCacheDirectory;
        }
    }
}
