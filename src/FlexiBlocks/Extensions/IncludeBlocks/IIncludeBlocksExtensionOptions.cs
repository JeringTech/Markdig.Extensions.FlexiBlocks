namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// An abstractions for <see cref="IncludeBlocksExtension"/> options.
    /// </summary>
    public interface IIncludeBlocksExtensionOptions : IExtensionOptions<IIncludeBlockOptions>
    {
        /// <summary>
        /// Gets the base URI for <see cref="IncludeBlock"/>s in root content.
        /// </summary>
        string BaseUri { get; }
    }
}
