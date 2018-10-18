namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para>An abstraction for <see cref="FlexiBlocksExtension"/> factories.</para>
    /// <para>We want to be able to build multiple MarkdownPipelines using the same low-level services (e.g services like INodeJSService, that are most efficient
    /// as singletons). At the same time, some higher-level services such as FlexiBlocksExtensions implementations require MarkdownPipeline-specific state.
    /// This abstraction facilitates a consistent structure for extensions. All MarkdownPipeline specific state must be passed through IFlexiBlocksExtensionOptions
    /// instances.</para>
    /// </summary>
    /// <typeparam name="TExtension">The extension's type.</typeparam>
    /// <typeparam name="TExtensionOptions">The extension's options type.</typeparam>
    public interface IFlexiBlocksExtensionFactory<TExtension, TExtensionOptions> where TExtension : FlexiBlocksExtension
    {
        /// <summary>
        /// Creates an instance of the extension.
        /// </summary>
        /// <param name="extensionOptions">The options for the extension.</param>
        TExtension Build(TExtensionOptions extensionOptions);
    }
}
