namespace FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// Options for <see cref="FlexiTableBlocksExtension"/>. Note that JSON options cannot be used with responsive tables because <see cref="PipeTableParser"/> is
    /// an <see cref="InlineParser"/>.
    /// </summary>
    public class FlexiTableBlocksExtensionOptions
    {
        /// <summary>
        /// Default <see cref="FlexiTableBlockOptions"/>.
        /// </summary>
        public FlexiTableBlockOptions DefaultResponsiveTableOptions { get; set; } = new FlexiTableBlockOptions();
    }
}
