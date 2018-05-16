using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionLinkReferenceDefinition : LinkReferenceDefinition
    {
        public SectionBlock SectionBlock { get; set; }
    }
}
