namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents options for a FlexiBlocksExtension.
    /// </summary>
    /// <typeparam name="T">The options type for the extension's FlexiBlocks.</typeparam>
    public interface IFlexiBlocksExtensionOptions<T>
    {
        /// <summary>
        /// Gets or sets the default options for the extension's FlexiBlocks.
        /// </summary>
        T DefaultBlockOptions { get; set; }
    }
}
