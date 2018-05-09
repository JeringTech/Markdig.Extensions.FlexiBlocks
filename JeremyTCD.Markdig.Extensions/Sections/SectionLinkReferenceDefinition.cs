using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions
{
    public class SectionLinkReferenceDefinition : LinkReferenceDefinition
    {
        public SectionBlock SectionBlock { get; set; }
    }
}
