namespace JeremyTCD.Markdig.Extensions
{
    public class SectionExtensionOptions
    {
        public bool AutoLinking { get; set; } = true;
        public bool AutoIdentifiers { get; set; } = true;
        public SectioningContentElement Level1WrapperElement { get; set; } = SectioningContentElement.None;
        public SectioningContentElement Level2PlusWrapperElement { get; set; } = SectioningContentElement.Section;

        public SectionOptions DefaultSectionOptions { get; set; } = new SectionOptions();
    }
}