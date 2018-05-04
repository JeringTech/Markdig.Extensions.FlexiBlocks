using Markdig.Parsers;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions
{
    public class SectionBlock : ContainerBlock
    {
        public SectionBlock(BlockParser parser) : base(parser)
        {
        }

        public int Level { get; set; }
    }
}
