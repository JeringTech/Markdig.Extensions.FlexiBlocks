using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Renderers;

namespace FlexiBlocks.FlexiTableBlocks
{
    public class FlexiTableBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiTableBlocksExtensionOptions _options;

        public FlexiTableBlocksExtension(FlexiTableBlocksExtensionOptions options)
        {
            _options = options ?? new FlexiTableBlocksExtensionOptions();
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

                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiTableBlockRenderer(_options.DefaultResponsiveTableOptions));
                }
            }
        }
    }
}
