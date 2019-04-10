using Markdig.Parsers;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// Represents an alert containing content you'd like to draw readers attention to, such as warnings and important information.
    /// </summary>
    public class FlexiAlert : FlexiBlock<IFlexiAlertOptions>
    {
        /// <summary>
        /// <para>Creates a <see cref="FlexiAlert"/>.</para>
        /// </summary>
        /// <param name="iconHtmlFragment">This <see cref="FlexiAlert" />'s icon as a HTML fragment.</param>
        /// <param name="flexiAlertOptions">The options for this <see cref="FlexiAlert"/>.</param>
        /// <param name="parser">The parser for this <see cref="FlexiAlert"/>.</param>
        public FlexiAlert(string iconHtmlFragment, IFlexiAlertOptions flexiAlertOptions, BlockParser parser) : base(flexiAlertOptions, parser)
        {
            IconHtmlFragment = iconHtmlFragment;
        }

        /// <summary>
        /// Gets this <see cref="FlexiAlert" />'s icon as a HTML fragment.
        /// </summary>
        public virtual string IconHtmlFragment { get; }
    }
}
