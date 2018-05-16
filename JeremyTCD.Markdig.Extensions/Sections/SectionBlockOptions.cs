using System;
using System.Collections.Generic;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockOptions : IMarkdownObjectOptions<SectionBlockOptions>
    {
        public bool GenerateIdentifier { get; set; } = true;
        public bool AutoLinkable { get; set; } = true;
        public SectioningContentElement Level1WrapperElement { get; set; } = SectioningContentElement.None;
        public SectioningContentElement Level2PlusWrapperElement { get; set; } = SectioningContentElement.Section;

        // TODO test attribute population and overwriting
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public SectionBlockOptions Clone()
        {
            SectionBlockOptions result = MemberwiseClone() as SectionBlockOptions;
            result.Attributes = new Dictionary<string, string>(Attributes, StringComparer.OrdinalIgnoreCase);

            return result;
        }
    }
}
