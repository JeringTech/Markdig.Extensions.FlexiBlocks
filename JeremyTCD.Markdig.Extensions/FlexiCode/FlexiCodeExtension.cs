using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    public class FlexiCodeExtension : IMarkdownExtension
    {
        private readonly FlexiCodeExtensionOptions _options;
        private readonly JsonOptionsService _jsonOptionsService;
        public const string FLEXI_CODE_OPTIONS_KEY = "flexiCodeOptions";

        public FlexiCodeExtension(FlexiCodeExtensionOptions options)
        {
            _options = options ?? new FlexiCodeExtensionOptions();
            _jsonOptionsService = new JsonOptionsService();
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
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiCodeRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiCodeRenderer());
                }

                CodeBlockRenderer codeBlockRenderer = htmlRenderer.ObjectRenderers.Find<CodeBlockRenderer>();
                if (codeBlockRenderer != null)
                {
                    htmlRenderer.ObjectRenderers.Remove(codeBlockRenderer);
                }
            }
        }

        /// <summary>
        /// Called when a code block is closed. Creates the <see cref="FlexiCodeOptions"/> for <paramref name="block"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        internal virtual void CodeBlockOnClosed(BlockProcessor processor, Block block)
        {
            FlexiCodeOptions flexiCodeOptions = _options.DefaultFlexiCodeOptions.Clone();

            // TODO merge class attribute with attributes map 
            // Apply JSON options if they exist
            _jsonOptionsService.TryPopulateOptions(processor, flexiCodeOptions, block.Line);

            if (!string.IsNullOrWhiteSpace(flexiCodeOptions.FlexiCodeClassName))
            {
                flexiCodeOptions.Attributes.Add("class", flexiCodeOptions.FlexiCodeClassName);
            }

            block.SetData(FLEXI_CODE_OPTIONS_KEY, flexiCodeOptions);
        }
    }
}
