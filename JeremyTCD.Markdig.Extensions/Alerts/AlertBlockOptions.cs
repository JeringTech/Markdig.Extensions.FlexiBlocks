using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertBlockOptions : IMarkdownObjectOptions<AlertBlockOptions>
    {
        public string IconElementMarkup { get; set; }

        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public AlertBlockOptions Clone()
        {
            var result = (AlertBlockOptions)MemberwiseClone();
            result.Attributes = new Dictionary<string, string>(Attributes);

            return result;
        }
    }
}
