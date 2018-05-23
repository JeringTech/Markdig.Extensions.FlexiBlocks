namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionsExtensionOptions
    {
        /// <summary>
        /// Used if section's level is 1.
        /// </summary>
        public SectioningContentElement Level1WrapperElement { get; set; } = SectioningContentElement.None;

        /// <summary>
        /// Used if section's level is greater than or equal to 2.
        /// </summary>
        public SectioningContentElement Level2PlusWrapperElement { get; set; } = SectioningContentElement.Section;

        /// <summary>
        /// Default <see cref="SectionBlockOptions"/>.
        /// </summary>
        public SectionBlockOptions DefaultSectionBlockOptions { get; set; } = new SectionBlockOptions();
    }
}