using Jering.IocServices.Newtonsoft.Json;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// <see cref="FlexiIncludeBlocksExtension"/> factory.
    /// </summary>
    public class FlexiIncludeBlocksExtensionFactory : IFlexiBlocksExtensionFactory<FlexiIncludeBlocksExtension, FlexiIncludeBlocksExtensionOptions>
    {
        private readonly IJsonSerializerService _jsonSerializerService;
        private readonly ISourceRetrieverService _sourceRetrieverService;
        private readonly ILeadingWhitespaceEditorService _leadingWhitespaceEditorService;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlocksExtensionFactory"/> instance.
        /// </summary>
        /// <param name="jsonSerializerService">The service that will handle JSON deserialization for parsers.</param>
        /// <param name="sourceRetrieverService">The service that will handle content retrieval for parsers.</param>
        /// <param name="leadingWhitespaceEditorService">The service that will handle editing of leading whitespace for parsers.</param>
        public FlexiIncludeBlocksExtensionFactory(IJsonSerializerService jsonSerializerService,
            ISourceRetrieverService sourceRetrieverService,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService)
        {
            _jsonSerializerService = jsonSerializerService;
            _sourceRetrieverService = sourceRetrieverService;
            _leadingWhitespaceEditorService = leadingWhitespaceEditorService;
        }

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlocksExtension"/> instance.
        /// </summary>
        /// <param name="extensionOptions">The options for the <see cref="FlexiIncludeBlocksExtension"/>.</param>
        public FlexiIncludeBlocksExtension Build(FlexiIncludeBlocksExtensionOptions extensionOptions = null)
        {
            extensionOptions = extensionOptions ?? new FlexiIncludeBlocksExtensionOptions();

            var flexiIncludeBlockParser = new FlexiIncludeBlockParser(_sourceRetrieverService, _jsonSerializerService, _leadingWhitespaceEditorService, extensionOptions);

            return new FlexiIncludeBlocksExtension(flexiIncludeBlockParser);
        }
    }
}
