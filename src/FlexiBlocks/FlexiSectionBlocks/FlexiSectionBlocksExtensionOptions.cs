namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// Represents options for the <see cref="FlexiSectionBlocksExtension"/>.
    /// </summary>
    public class FlexiSectionBlocksExtensionOptions : IFlexiBlocksExtensionOptions<FlexiSectionBlockOptions>
    {
        /// <summary>
        /// Gets or sets the default <see cref="FlexiSectionBlockOptions"/>.
        /// </summary>
        public FlexiSectionBlockOptions DefaultBlockOptions { get; set; } = new FlexiSectionBlockOptions();
    }
}