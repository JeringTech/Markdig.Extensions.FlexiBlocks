using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsExtensionOptions
    {
        /// <summary>
        /// Map of alert type names to icon markup.
        /// </summary>
        public Dictionary<string, string> IconMarkups = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Default <see cref="AlertBlockOptions"/>.
        /// </summary>
        public AlertBlockOptions DefaultAlertBlockOptions { get; set; } = new AlertBlockOptions();
    }
}
