using Markdig.Parsers;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    /// <summary>
    /// A block for alerts.
    /// </summary>
    public class AlertBlock : ContainerBlock
    {
        public AlertBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// The options for this block.
        /// </summary>
        public AlertBlockOptions AlertBlockOptions { get; set; }
    }
}
