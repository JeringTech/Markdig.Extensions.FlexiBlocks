using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockOptions : IMarkdownObjectOptions<SectionBlockOptions>
    {
        /// <summary>
        /// Markup for an icon. If specified, rendered as part of the section's header.
        /// </summary>
        public string IconMarkup { get; set; }

        /// <summary>
        /// <see cref="IdentifierService"/>.
        /// </summary>
        public bool GenerateIdentifier { get; set; } = true;

        /// <summary>
        /// <see cref="AutoLinkService"/>.
        /// </summary>
        public bool AutoLinkable { get; set; } = true;

        /// <summary>
        /// Sectioning content element used to wrap section.
        /// </summary>
        public SectioningContentElement WrapperElement { get; set; }

        /// <summary>
        /// HTML attributes.
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public SectionBlockOptions Clone()
        {
            var result = MemberwiseClone() as SectionBlockOptions;
            result.Attributes = new Dictionary<string, string>(Attributes, StringComparer.OrdinalIgnoreCase);

            return result;
        }
    }
}
