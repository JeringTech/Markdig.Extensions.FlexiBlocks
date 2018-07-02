namespace FlexiBlocks.ResponsiveTables
{
    /// <summary>
    /// Options for <see cref="FlexiTablesExtension"/>. Note that JSON options cannot be used with responsive tables because <see cref="PipeTableParser"/> is
    /// an <see cref="InlineParser"/>.
    /// </summary>
    public class FlexiTablesExtensionOptions
    {
        /// <summary>
        /// Default <see cref="FlexiTableOptions"/>.
        /// </summary>
        public FlexiTableOptions DefaultResponsiveTableOptions { get; set; } = new FlexiTableOptions();
    }
}
