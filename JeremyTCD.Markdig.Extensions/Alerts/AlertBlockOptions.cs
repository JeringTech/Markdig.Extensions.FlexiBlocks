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
        /// Gets or sets the value used as the format for the alert's outer div's class. The inserted sub-string is the alert's type name (the string provided in the first line 
        /// of the markdown for an alert). 
        /// 
        /// If the value is null, whitespace or an empty string, no class is assigned to the alert's outer div.
        /// </summary>
        public string ClassNameFormat { get; set; } = "alert-{0}";

        /// <summary>
        /// Gets or sets the value used as the class of the alert's content wrapper (rendered only if <see cref="IconMarkup"/> is a valid string). If the value is null, whitespace or an empty string, 
        /// no class is assigned. 
        /// </summary>
        public string ContentClassName { get; set; } = "alert-content";

        /// <summary>
        /// HTML attributes for the outermost element of the alert block.
        /// </summary>
        public HtmlAttributeDictionary Attributes { get; set; } = new HtmlAttributeDictionary();

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public AlertBlockOptions Clone()
        {
            var result = (AlertBlockOptions)MemberwiseClone();
            result.Attributes = new HtmlAttributeDictionary(Attributes);

            return result;
        }
    }
}
