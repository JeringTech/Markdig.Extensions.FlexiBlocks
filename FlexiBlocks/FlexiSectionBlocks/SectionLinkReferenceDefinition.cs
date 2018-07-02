using Markdig.Syntax;

namespace FlexiBlocks.FlexiSectionBlocks
{
    public class SectionLinkReferenceDefinition : LinkReferenceDefinition
    {
        public FlexiSectionBlock SectionBlock { get; set; }
    }
}
