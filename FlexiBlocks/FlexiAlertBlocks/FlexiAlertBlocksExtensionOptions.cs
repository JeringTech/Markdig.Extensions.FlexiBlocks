using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    public class FlexiAlertBlocksExtensionOptions
    {
        /// <summary>
        /// Map of FlexiAlertBlock types to icon markups.
        /// 
        /// The default SVGs are part of the excellent material design icon set - https://material.io/tools/icons/?style=baseline
        /// They are licensed under an Apache License Version 2 license - https://www.apache.org/licenses/LICENSE-2.0.html
        /// </summary>
        public Dictionary<string, string> IconMarkups = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "info", "<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z\"></path></svg>" },
            { "warning", "<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>" },
            { "critical-warning", "<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M0 0h24v24H0z\" fill=\"none\"></path><path d=\"M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z\"></path></svg>" }
        };

        /// <summary>
        /// Default <see cref="FlexiAlertBlockOptions"/>.
        /// </summary>
        public FlexiAlertBlockOptions DefaultFlexiAlertBlockOptions { get; set; } = new FlexiAlertBlockOptions();
    }
}
