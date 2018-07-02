using System;
using System.Collections.Generic;

namespace FlexiBlocks.FlexiSectionBlocks
{
    public class FlexiSectionBlockOptions : IMarkdownObjectOptions<FlexiSectionBlockOptions>
    {
        /// <summary>
        /// Gets or sets the value used as the markup for the section header's icon. 
        /// If the value is null, whitespace or an empty string, no icon is rendered for the section header.
        ///
        /// The default SVG is part of the excellent material design icon set - https://material.io/tools/icons/?style=baseline
        /// It is licensed under an Apache License Version 2 license - https://www.apache.org/licenses/LICENSE-2.0.html
        /// </summary>
        public string HeaderIconMarkup { get; set; } = "<svg viewBox=\"0 0 24 24\" width=\"24\" height=\"24\"><path d=\"M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7c-2.76 0-5 2.24-5 5s2.24 5 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1zM8 13h8v-2H8v2zm9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1s-1.39 3.1-3.1 3.1h-4V17h4c2.76 0 5-2.24 5-5s-2.24-5-5-5z\"></path></svg>";

        /// <summary>
        /// Gets or sets the value used as the format for section header's class. The inserted sub-string is the section's level.
        /// If the value is null, whitespace or an empty string, no class is assigned to the section header.
        /// </summary>
        public string HeaderClassNameFormat { get; set; } = "header-level-{0}";

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
        /// HTML attributes for the outermost element of the section block.
        /// </summary>
        public HtmlAttributeDictionary Attributes { get; set; } = new HtmlAttributeDictionary();

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public FlexiSectionBlockOptions Clone()
        {
            var result = MemberwiseClone() as FlexiSectionBlockOptions;
            result.Attributes = new HtmlAttributeDictionary(Attributes);

            return result;
        }
    }
}
