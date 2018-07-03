using FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Renderers;

namespace FlexiBlocks.FlexiAlertBlocks
{
    public class FlexiAlertBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiAlertBlocksExtensionOptions _options;

        public FlexiAlertBlocksExtension(FlexiAlertBlocksExtensionOptions options)
        {
            _options = options ?? new FlexiAlertBlocksExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<FlexiAlertBlockParser>())
            {
                var flexiOptionsBlockService = new FlexiOptionsBlockService();
                var flexiAlertBlockParser = new FlexiAlertBlockParser(_options, flexiOptionsBlockService);
                pipeline.BlockParsers.Insert(0, flexiAlertBlockParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<FlexiAlertBlockRenderer>())
            {
                htmlRenderer.ObjectRenderers.Insert(0, new FlexiAlertBlockRenderer());
            }
        }
    }
}
