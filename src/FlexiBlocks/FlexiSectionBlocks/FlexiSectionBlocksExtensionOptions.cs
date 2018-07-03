namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    public class FlexiSectionBlocksExtensionOptions
    {
        /// <summary>
        /// Used if <see cref="FlexiSectionBlock"/>'s level is 1.
        /// </summary>
        public SectioningContentElement Level1WrapperElement { get; set; } = SectioningContentElement.None;

        /// <summary>
        /// Used if <see cref="FlexiSectionBlock"/>'s level is greater than or equal to 2.
        /// </summary>
        public SectioningContentElement Level2PlusWrapperElement { get; set; } = SectioningContentElement.Section;

        /// <summary>
        /// Default <see cref="FlexiSectionBlockOptions"/>.
        /// </summary>
        public FlexiSectionBlockOptions DefaultFlexiSectionBlockOptions { get; set; } = new FlexiSectionBlockOptions();
    }
}