using FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;

namespace FlexiBlocks.FlexiTableBlocks
{
    public class FlexiTableBlocksExtension : IMarkdownExtension
    {
        public const string FLEXI_TABLE_BLOCK_OPTIONS_KEY = "flexiTableBlockOptions";
        private readonly FlexiTableBlocksExtensionOptions _options;
        private readonly FlexiOptionsBlockService _flexiOptionsBlockService;

        public FlexiTableBlocksExtension(FlexiTableBlocksExtensionOptions options)
        {
            _options = options ?? new FlexiTableBlocksExtensionOptions();
            _flexiOptionsBlockService = new FlexiOptionsBlockService();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Do nothing
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiTableBlockRenderer>())
                {
                    HtmlTableRenderer htmlTableRenderer = htmlRenderer.ObjectRenderers.Find<HtmlTableRenderer>();
                    if (htmlTableRenderer != null)
                    {
                        htmlRenderer.ObjectRenderers.Remove(htmlTableRenderer);
                    }

                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiTableBlockRenderer());
                }
            }
        }

        /// <summary>
        /// Called when a <see cref="Table"/> is closed. Creates the <see cref="FlexiTableBlockOptions"/> for <paramref name="block"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        internal virtual void CodeBlockOnClosed(BlockProcessor processor, Block block)
        {
            FlexiTableBlockOptions flexiTableBlockOptions = _options.DefaultResponsiveTableOptions.Clone();

            // Apply FlexiOptionsBlock options if they exist
            _flexiOptionsBlockService.TryPopulateOptions(processor, flexiTableBlockOptions, block.Line);

            block.SetData(FLEXI_TABLE_BLOCK_OPTIONS_KEY, flexiTableBlockOptions);
        }
    }
}
