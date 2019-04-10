using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// An abstraction for <see cref="FlexiAlertsExtension"/> options.
    /// </summary>
    public interface IFlexiAlertsExtensionOptions : IFlexiBlocksExtensionOptions<IFlexiAlertOptions>
    {
        /// <summary>
        /// Gets a map of <see cref="FlexiAlert" /> types to icon HTML fragments.
        /// </summary>
        ReadOnlyDictionary<string, string> IconHtmlFragments { get; }
    }
}