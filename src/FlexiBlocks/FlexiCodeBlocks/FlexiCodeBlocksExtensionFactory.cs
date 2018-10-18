using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// <see cref="FlexiCodeBlocksExtension"/> factory.
    /// </summary>
    public class FlexiCodeBlocksExtensionFactory : IFlexiBlocksExtensionFactory<FlexiCodeBlocksExtension, FlexiCodeBlocksExtensionOptions>
    {
        private readonly FlexiCodeBlockRenderer _flexiCodeBlockRenderer;
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Creates a <see cref="FlexiCodeBlocksExtensionFactory"/> instance.
        /// </summary>
        /// <param name="flexiOptionsBlockService">The service that will handle options generation for parsers.</param>
        /// <param name="flexiCodeBlockRenderer">The renderer for <see cref="FlexiCodeBlocksExtension"/>s.</param>
        public FlexiCodeBlocksExtensionFactory(IFlexiOptionsBlockService flexiOptionsBlockService,
            FlexiCodeBlockRenderer flexiCodeBlockRenderer)
        {
            _flexiOptionsBlockService = flexiOptionsBlockService;
            _flexiCodeBlockRenderer = flexiCodeBlockRenderer;
        }

        /// <summary>
        /// Creates a <see cref="FlexiCodeBlocksExtension"/> instance.
        /// </summary>
        /// <param name="extensionOptions">The options for the <see cref="FlexiCodeBlocksExtension"/>.</param>
        public FlexiCodeBlocksExtension Build(FlexiCodeBlocksExtensionOptions extensionOptions = null)
        {
            extensionOptions = extensionOptions ?? new FlexiCodeBlocksExtensionOptions();

            return new FlexiCodeBlocksExtension(_flexiCodeBlockRenderer, _flexiOptionsBlockService, extensionOptions);
        }
    }
}
