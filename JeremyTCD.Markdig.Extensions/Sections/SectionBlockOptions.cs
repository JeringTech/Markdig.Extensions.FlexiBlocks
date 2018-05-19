using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockOptions : IMarkdownObjectOptions<SectionBlockOptions>
    {
        /// <summary>
        /// <see cref="IdentifierService"/>
        /// </summary>
        public bool GenerateIdentifier { get; set; } = true;

        /// <summary>
        /// <see cref="AutoLinkService"/>
        /// </summary>
        public bool AutoLinkable { get; set; } = true;

        /// <summary>
        /// Used if section's level is 1.
        /// </summary>
        public SectioningContentElement Level1WrapperElement { get; set; }

        /// <summary>
        /// Used if section's level is greater than or equal to 2.
        /// </summary>
        public SectioningContentElement Level2PlusWrapperElement { get; set; } = SectioningContentElement.Section;

        /// <summary>
        /// HTML attributes
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
