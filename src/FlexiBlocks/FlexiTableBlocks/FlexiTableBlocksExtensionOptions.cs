namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// Represents options for the <see cref="FlexiTableBlocksExtension"/>.
    /// </summary>
    public class FlexiTableBlocksExtensionOptions : IFlexiBlocksExtensionOptions<FlexiTableBlockOptions>
    {
        /// <summary>
        /// Gets or sets the default <see cref="FlexiTableBlockOptions"/>.
        /// </summary>
        public FlexiTableBlockOptions DefaultBlockOptions { get; set; } = new FlexiTableBlockOptions();
    }
}
