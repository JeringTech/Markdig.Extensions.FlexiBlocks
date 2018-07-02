using Markdig.Parsers;
using Markdig.Syntax;

namespace FlexiBlocks.Alerts
{
    /// <summary>
    /// A block for alerts.
    /// </summary>
    public class FlexiAlertBlock : ContainerBlock
    {
        public FlexiAlertBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// The options for this block.
        /// </summary>
        public FlexiAlertBlockOptions AlertBlockOptions { get; set; }
    }
}
