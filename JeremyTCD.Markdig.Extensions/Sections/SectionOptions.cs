namespace JeremyTCD.Markdig.Extensions
{
    public class SectionOptions
    {
        public bool AutoLink { get; set; } = true;
        public SectioningContentElement H1WrapperElement { get; set; } = SectioningContentElement.Article;
    }
}