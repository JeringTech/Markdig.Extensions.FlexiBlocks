using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class ContentRetrievalServiceOptions
    {
        public string RootPath { get; set; } = Directory.GetCurrentDirectory();
    }
}