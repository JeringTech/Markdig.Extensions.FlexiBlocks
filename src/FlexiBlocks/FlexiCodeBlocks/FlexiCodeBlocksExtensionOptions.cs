namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// Represents options for the <see cref="FlexiCodeBlocksExtension"/>.
    /// </summary>
    public class FlexiCodeBlocksExtensionOptions : IFlexiBlocksExtensionOptions<FlexiCodeBlockOptions>
    {
        /// <summary>
        /// Gets or sets the default <see cref="FlexiCodeBlockOptions"/>.
        /// </summary>
        public FlexiCodeBlockOptions DefaultBlockOptions { get; set; } = new FlexiCodeBlockOptions();
    }
}
