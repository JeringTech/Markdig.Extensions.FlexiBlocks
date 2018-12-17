using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// <para>Represents a <see cref="FlexiSectionBlock"/>'s heading.</para>
    /// <para>A <see cref="FlexiSectionBlock"/> is demarcated by an ATX heading. The contents of the ATX heading must be processed as per normal - 
    /// its inlines must be processed. For this to happen, the contents of the ATX heading must be assigned to a <see cref="LeafBlock"/>. Therefore,
    /// <see cref="FlexiSectionBlockParser.TryOpenFlexiBlock(BlockProcessor)"/> creates a new <see cref="FlexiSectionHeadingBlock"/>. This new block is
    /// added as its parent <see cref="FlexiSectionBlock"/>'s first child and assigned the content of the ATX heading.</para>
    /// </summary>
    public class FlexiSectionHeadingBlock : LeafBlock
    {
        /// <summary>
        /// Creates a <see cref="FlexiSectionHeadingBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this block.</param>
        public FlexiSectionHeadingBlock(BlockParser parser) : base(parser)
        {
            ProcessInlines = true;
        }
    }
}
