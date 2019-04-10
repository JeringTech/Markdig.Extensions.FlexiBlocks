using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// <para>The default implementation of <see cref="IFlexiAlertsExtensionOptions"/>.</para>
    /// 
    /// <para>This class is immutable.</para>
    /// </summary>
    public class FlexiAlertsExtensionOptions : FlexiBlocksExtensionOptions<IFlexiAlertOptions>, IFlexiAlertsExtensionOptions
    {
        private static readonly ReadOnlyDictionary<string, string> _defaultHtmlFragments = new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "info", Icons.MATERIAL_DESIGN_INFO },
                { "warning", Icons.MATERIAL_DESIGN_WARNING},
                { "critical-warning", Icons.MATERIAL_DESIGN_ERROR}
            }
        );

        /// <summary>
        /// Creates a <see cref="FlexiAlertsExtensionOptions" />.
        /// </summary>
        /// <param name="defaultBlockOptions">
        /// <para>Default <see cref="FlexiAlertOptions"/> for all <see cref="FlexiAlert"/>s.</para>
        /// <para>If this value is null, a <see cref="FlexiAlertOptions"/> with default values is used.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="iconHtmlFragments">
        /// <para>A map of <see cref="FlexiAlert"/> types to icon HTML fragments.</para>
        /// <para>If this value is null, a map of icon HTML fragments containing types "info", "warning" and "critical-warning" is used.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        public FlexiAlertsExtensionOptions(IFlexiAlertOptions defaultBlockOptions = null, IDictionary<string, string> iconHtmlFragments = null) :
            base(defaultBlockOptions ?? new FlexiAlertOptions())
        {
            IconHtmlFragments = iconHtmlFragments is ReadOnlyDictionary<string, string> ? iconHtmlFragments as ReadOnlyDictionary<string, string> :
                iconHtmlFragments != null ? new ReadOnlyDictionary<string, string>(iconHtmlFragments) :
                _defaultHtmlFragments;
        }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual ReadOnlyDictionary<string, string> IconHtmlFragments { get; private set; }
    }
}
