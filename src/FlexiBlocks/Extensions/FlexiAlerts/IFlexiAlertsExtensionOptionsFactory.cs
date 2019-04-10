using Markdig.Parsers;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// An abstraction for creating <see cref="IFlexiAlertsExtensionOptions"/>.
    /// </summary>
    public interface IFlexiAlertsExtensionOptionsFactory
    {
        /// <summary>
        /// Creates a <see cref="IFlexiAlertsExtensionOptions"/>.
        /// </summary>
        /// <param name="blockProcessor">A processor that might hold a reference to an <see cref="IFlexiAlertsExtensionOptions"/>.</param>
        IFlexiAlertsExtensionOptions Create(BlockProcessor blockProcessor);
    }
}