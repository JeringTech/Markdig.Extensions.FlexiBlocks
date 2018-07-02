using Markdig.Syntax;

namespace FlexiBlocks.Sections
{
    public class SectionLinkReferenceDefinition : LinkReferenceDefinition
    {
        public FlexiSectionBlock SectionBlock { get; set; }
    }
}
