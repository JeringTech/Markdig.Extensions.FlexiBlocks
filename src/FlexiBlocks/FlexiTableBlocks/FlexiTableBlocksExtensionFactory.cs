using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// <see cref="FlexiTableBlocksExtension"/> factory.
    /// </summary>
    public class FlexiTableBlocksExtensionFactory : IFlexiBlocksExtensionFactory<FlexiTableBlocksExtension, FlexiTableBlocksExtensionOptions>
    {
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Creates a <see cref="FlexiTableBlocksExtensionFactory"/> instance.
        /// </summary>
        /// <param name="flexiOptionsBlockService">The service that will handle options generation.</param>
        public FlexiTableBlocksExtensionFactory(IFlexiOptionsBlockService flexiOptionsBlockService)
        {
            _flexiOptionsBlockService = flexiOptionsBlockService;
        }

        /// <summary>
        /// Creates a <see cref="FlexiTableBlocksExtension"/> instance.
        /// </summary>
        /// <param name="extensionOptions">The options for the <see cref="FlexiTableBlocksExtension"/>.</param>
        public FlexiTableBlocksExtension Build(FlexiTableBlocksExtensionOptions extensionOptions = null)
        {
            extensionOptions = extensionOptions ?? new FlexiTableBlocksExtensionOptions();

            var flexiTableBlockRenderer = new FlexiTableBlockRenderer(extensionOptions);

            return new FlexiTableBlocksExtension(flexiTableBlockRenderer, _flexiOptionsBlockService, extensionOptions);
        }
    }
}
