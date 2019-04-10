using Jering.Markdig.Extensions.FlexiBlocks.Options;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// An abstraction for <see cref="FlexiAlert"/> options.
    /// </summary>
    public interface IFlexiAlertOptions : IFlexiBlockOptions<IFlexiAlertOptions>
    {
        /// <summary>
        /// Gets the <see cref="FlexiAlert" />'s icon as a HTML fragment.
        /// </summary>
        string IconHtmlFragment { get; }

        /// <summary>
        /// Gets the <see cref="FlexiAlert"/>'s type.
        /// </summary>
        string Type { get; }
    }
}