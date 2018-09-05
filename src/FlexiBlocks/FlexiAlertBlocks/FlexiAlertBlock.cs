using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// Represents a block containing tangential content such as warnings and elaborations.
    /// </summary>
    public class FlexiAlertBlock : ContainerBlock
    {
        /// <summary>
        /// Creates a <see cref="FlexiAlertBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this block.</param>
        public FlexiAlertBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// The options for this block.
        /// </summary>
        public FlexiAlertBlockOptions FlexiAlertBlockOptions { get; set; }
    }
}
