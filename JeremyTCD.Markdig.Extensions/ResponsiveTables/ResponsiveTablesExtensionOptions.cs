namespace JeremyTCD.Markdig.Extensions.ResponsiveTables
{
    /// <summary>
    /// Options for <see cref="ResponsiveTablesExtension"/>. Note that JSON options cannot be used with responsive tables because <see cref="PipeTableParser"/> is
    /// an <see cref="InlineParser"/>.
    /// </summary>
    public class ResponsiveTablesExtensionOptions
    {
        /// <summary>
        /// Default <see cref="ResponsiveTableOptions"/>.
        /// </summary>
        public ResponsiveTableOptions DefaultResponsiveTableOptions { get; set; } = new ResponsiveTableOptions();
    }
}
