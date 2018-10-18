using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// <see cref="FlexiSectionBlocksExtension"/> factory.
    /// </summary>
    public class FlexiSectionBlocksExtensionFactory : IFlexiBlocksExtensionFactory<FlexiSectionBlocksExtension, FlexiSectionBlocksExtensionOptions>
    {
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;
        private readonly FlexiSectionBlockRenderer _flexiSectionBlockRenderer;

        /// <summary>
        /// Creates a <see cref="FlexiSectionBlocksExtensionFactory"/> instance.
        /// </summary>
        /// <param name="flexiOptionsBlockService">The service that will handle options generation for parsers.</param>
        /// <param name="flexiSectionBlockRenderer">The renderer for <see cref="FlexiSectionBlocksExtension"/>s.</param>
        public FlexiSectionBlocksExtensionFactory(IFlexiOptionsBlockService flexiOptionsBlockService,
            FlexiSectionBlockRenderer flexiSectionBlockRenderer)
        {
            _flexiOptionsBlockService = flexiOptionsBlockService;
            _flexiSectionBlockRenderer = flexiSectionBlockRenderer;
        }

        /// <summary>
        /// Creates a <see cref="FlexiSectionBlocksExtension"/> instance.
        /// </summary>
        /// <param name="extensionOptions">The options for the <see cref="FlexiSectionBlocksExtension"/>.</param>
        public FlexiSectionBlocksExtension Build(FlexiSectionBlocksExtensionOptions extensionOptions = null)
        {
            extensionOptions = extensionOptions ?? new FlexiSectionBlocksExtensionOptions();

            var flexiSectionBlockParser = new FlexiSectionBlockParser(_flexiOptionsBlockService, extensionOptions);

            return new FlexiSectionBlocksExtension(flexiSectionBlockParser, _flexiSectionBlockRenderer);
        }
    }
}
