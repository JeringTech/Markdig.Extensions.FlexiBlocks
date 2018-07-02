using FlexiBlocks.FlexiOptionBlocks;
using JeremyTCD.WebUtils.SyntaxHighlighters.HighlightJS;
using JeremyTCD.WebUtils.SyntaxHighlighters.Prism;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace FlexiBlocks.FlexiCodeBlocks
{
    public class FlexiCodeBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiCodeBlocksExtensionOptions _options;
        private readonly FlexiOptionBlocksService _flexiOptionsService;
        public const string FLEXI_CODE_OPTIONS_KEY = "flexiCodeOptions";
        private readonly IPrismService _prismService;
        private readonly IHighlightJSService _highlightJSService;

        public FlexiCodeBlocksExtension(FlexiCodeBlocksExtensionOptions options,
            IPrismService prismService,
            IHighlightJSService highlightJSService)
        {
            _options = options ?? new FlexiCodeBlocksExtensionOptions();
            _flexiOptionsService = new FlexiOptionBlocksService();
            _prismService = prismService;
            _highlightJSService = highlightJSService;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // FencedCodeBlockParser and IndentedCodeBlockParser are default parsers registered in MarkdownPipelineBuilder's constructor
            FencedCodeBlockParser fencedCodeBlockParser = pipeline.BlockParsers.Find<FencedCodeBlockParser>();
            if (fencedCodeBlockParser != null)
            {
                fencedCodeBlockParser.Closed += CodeBlockOnClosed;
            }

            IndentedCodeBlockParser indentedCodeBlockParser = pipeline.BlockParsers.Find<IndentedCodeBlockParser>();
            if (indentedCodeBlockParser != null)
            {
                indentedCodeBlockParser.Closed += CodeBlockOnClosed;
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiCodeBlockRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiCodeBlockRenderer(_prismService, _highlightJSService));
                }

                CodeBlockRenderer codeBlockRenderer = htmlRenderer.ObjectRenderers.Find<CodeBlockRenderer>();
                if (codeBlockRenderer != null)
                {
                    htmlRenderer.ObjectRenderers.Remove(codeBlockRenderer);
                }
            }
        }

        /// <summary>
        /// Called when a code block is closed. Creates the <see cref="FlexiCodeBlockOptions"/> for <paramref name="block"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        internal virtual void CodeBlockOnClosed(BlockProcessor processor, Block block)
        {
            FlexiCodeBlockOptions flexiCodeOptions = _options.DefaultFlexiCodeOptions.Clone();

            // Apply FlexiOptions if they exist
            _flexiOptionsService.TryPopulateOptions(processor, flexiCodeOptions, block.Line);

            block.SetData(FLEXI_CODE_OPTIONS_KEY, flexiCodeOptions);
        }
    }
}
