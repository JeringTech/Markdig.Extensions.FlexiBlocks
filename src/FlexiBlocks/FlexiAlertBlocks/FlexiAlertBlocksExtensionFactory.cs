using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// <see cref="FlexiAlertBlocksExtension"/> factory.
    /// </summary>
    public class FlexiAlertBlocksExtensionFactory : IFlexiBlocksExtensionFactory<FlexiAlertBlocksExtension, FlexiAlertBlocksExtensionOptions>
    {
        private readonly FlexiAlertBlockRenderer _flexiAlertBlockRenderer;
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Creates a <see cref="FlexiAlertBlocksExtensionFactory"/> instance.
        /// </summary>
        /// <param name="flexiOptionsBlockService">The service that will handle options generation for parsers.</param>
        /// <param name="flexiAlertBlockRenderer">The renderer for <see cref="FlexiAlertBlocksExtension"/>s.</param>
        public FlexiAlertBlocksExtensionFactory(IFlexiOptionsBlockService flexiOptionsBlockService,
            FlexiAlertBlockRenderer flexiAlertBlockRenderer)
        {
            _flexiOptionsBlockService = flexiOptionsBlockService;
            _flexiAlertBlockRenderer = flexiAlertBlockRenderer;
        }

        /// <summary>
        /// Creates a <see cref="FlexiAlertBlocksExtension"/> instance.
        /// </summary>
        /// <param name="extensionOptions">The options for the <see cref="FlexiAlertBlocksExtension"/>.</param>
        public FlexiAlertBlocksExtension Build(FlexiAlertBlocksExtensionOptions extensionOptions = null)
        {
            extensionOptions = extensionOptions ?? new FlexiAlertBlocksExtensionOptions();

            var flexiAlertBlockParser = new FlexiAlertBlockParser(_flexiOptionsBlockService, extensionOptions);

            return new FlexiAlertBlocksExtension(flexiAlertBlockParser, _flexiAlertBlockRenderer);
        }
    }
}
