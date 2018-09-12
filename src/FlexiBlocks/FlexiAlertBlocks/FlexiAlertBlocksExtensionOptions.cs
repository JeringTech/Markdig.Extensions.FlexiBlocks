using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// Represents options for the <see cref="FlexiAlertBlocksExtension"/>.
    /// </summary>
    public class FlexiAlertBlocksExtensionOptions : IFlexiBlocksExtensionOptions<FlexiAlertBlockOptions>
    {
        /// <summary>
        /// <para>Gets or sets a map of <see cref="FlexiAlertBlock" /> types to icon markups.</para>
        /// 
        /// <para>By default, contains type "info" with icon https://material.io/tools/icons/?icon=info&amp;style=sharp,
        /// type "warning" with icon "https://material.io/tools/icons/?icon=warning&amp;style=sharp", 
        /// and type "critical-warning" with icon "https://material.io/tools/icons/?icon=error&amp;style=sharp".</para>
        /// 
        /// <para>The default icons are licensed under an Apache License Version 2 license - https://www.apache.org/licenses/LICENSE-2.0.html.</para>
        /// </summary>
        public Dictionary<string, string> IconMarkups { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "info", "<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-6h2v6zm0-8h-2v-2h2v2z\"/></svg>" },
            { "warning", "<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m1 21h22l-11-19-11 19zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"/></svg>" },
            { "critical-warning", "<svg viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M0,0h24v24H0V0z\" fill=\"none\"/><path d=\"m12 2c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm1 15h-2v-2h2v2zm0-4h-2v-6h2v6z\"/>" }
        };

        /// <summary>
        /// Gets or sets the default <see cref="FlexiAlertBlockOptions"/>.
        /// </summary>
        public FlexiAlertBlockOptions DefaultBlockOptions { get; set; } = new FlexiAlertBlockOptions();
    }
}
