using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// A markdig extension for <see cref="FlexiAlert"/>s.
    /// </summary>
    public class FlexiAlertsExtension : BlockExtension<FlexiAlert>
    {
        /// <summary>
        /// Creates a <see cref="FlexiAlertsExtension"/>.
        /// </summary>
        /// <param name="flexiAlertParser">The parser for creating <see cref="FlexiAlert"/>s from markdown.</param>
        /// <param name="flexiAlertRenderer">The renderer for rendering <see cref="FlexiAlert"/>s as HTML.</param>
        public FlexiAlertsExtension(BlockParser<FlexiAlert> flexiAlertParser, BlockRenderer<FlexiAlert> flexiAlertRenderer):
            base(flexiAlertParser, flexiAlertRenderer)
        {
        }
    }
}
