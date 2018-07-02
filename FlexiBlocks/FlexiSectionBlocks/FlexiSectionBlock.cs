using Markdig.Parsers;
using Markdig.Syntax;

namespace FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// A <see cref="ContainerBlock"/> that holds a logical section of a page.
    /// </summary>
    public class FlexiSectionBlock : ContainerBlock
    {
        public FlexiSectionBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// Nesting level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The options for this block.
        /// </summary>
        public FlexiSectionBlockOptions SectionBlockOptions { get; set; }
    }
}
