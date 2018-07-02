using Markdig.Parsers;
using Markdig.Syntax;

namespace FlexiBlocks.Alerts
{
    /// <summary>
    /// A block containing content that is tangential to its containing article.
    /// </summary>
    public class FlexiAlertBlock : ContainerBlock
    {
        public FlexiAlertBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// The options for this block.
        /// </summary>
        public FlexiAlertBlockOptions FlexiAlertBlockOptions { get; set; }
    }
}
