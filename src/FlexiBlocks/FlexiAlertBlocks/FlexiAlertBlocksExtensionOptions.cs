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
        /// Gets or sets a map of <see cref="FlexiAlertBlock" /> types to icon markups.
        /// </summary>
        public Dictionary<string, string> IconMarkups { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "info", Icons.MATERIAL_DESIGN_INFO },
            { "warning", Icons.MATERIAL_DESIGN_WARNING},
            { "critical-warning", Icons.MATERIAL_DESIGN_ERROR }
        };

        /// <summary>
        /// Gets or sets the default <see cref="FlexiAlertBlockOptions"/>.
        /// </summary>
        public FlexiAlertBlockOptions DefaultBlockOptions { get; set; } = new FlexiAlertBlockOptions();
    }
}
