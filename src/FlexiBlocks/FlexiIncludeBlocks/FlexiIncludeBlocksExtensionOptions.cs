namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents options for the <see cref="FlexiIncludeBlocksExtension"/>.
    /// </summary>
    public class FlexiIncludeBlocksExtensionOptions : IFlexiBlocksExtensionOptions<FlexiIncludeBlockOptions>
    {
        /// <summary>
        /// Gets or sets the default <see cref="FlexiIncludeBlockOptions"/>.
        /// </summary>
        public FlexiIncludeBlockOptions DefaultBlockOptions { get; set; } = new FlexiIncludeBlockOptions();
    }
}
