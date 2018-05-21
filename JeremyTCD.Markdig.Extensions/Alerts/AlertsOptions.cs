using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsOptions
    {
        /// <summary>
        /// Map of alert type names to icon elements markup.
        /// </summary>
        public Dictionary<string, string> IconElementMarkups = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Default <see cref="AlertBlockOptions"/>.
        /// </summary>
        public AlertBlockOptions DefaultAlertBlockOptions { get; set; } = new AlertBlockOptions();
    }
}
