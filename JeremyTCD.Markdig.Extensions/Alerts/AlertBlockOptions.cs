using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertBlockOptions : IMarkdownObjectOptions<AlertBlockOptions>
    {
        /// <summary>
        /// Markup for an icon. If specified, rendered before the content of the alert wrapped in a div with class alert-content.
        /// </summary>
        public string IconMarkup { get; set; }

        /// <summary>
        /// HTML attributes.
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public AlertBlockOptions Clone()
        {
            var result = (AlertBlockOptions)MemberwiseClone();
            result.Attributes = new Dictionary<string, string>(Attributes);

            return result;
        }
    }
}
