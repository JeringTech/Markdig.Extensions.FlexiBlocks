using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// Represents a section of an article.
    /// </summary>
    public class FlexiSectionBlock : ContainerBlock
    {
        /// <summary>
        /// Creates a <see cref="FlexiSectionBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this block.</param>
        public FlexiSectionBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// Gets or sets this <see cref="FlexiSectionBlock"/>'s level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the content for this <see cref="FlexiSectionBlock"/>'s header element.
        /// </summary>
        public string HeaderContent { get; set; }

        /// <summary>
        /// Gets or sets the ID for this <see cref="FlexiSectionBlock"/>'s outermost element.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the options for this block.
        /// </summary>
        public FlexiSectionBlockOptions FlexiSectionBlockOptions { get; set; }
    }
}
