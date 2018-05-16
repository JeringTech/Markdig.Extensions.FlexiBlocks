using Markdig.Parsers;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    /// <summary>
    /// A <see cref="ContainerBlock"/> that holds a logical section of a page.
    /// </summary>
    public class SectionBlock : ContainerBlock
    {
        public SectionBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// Nesting level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The options for this block
        /// </summary>
        public SectionBlockOptions SectionBlockOptions { get; set; }
    }
}
