using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Renderers;

namespace FlexiBlocks.ResponsiveTables
{
    public class FlexiTablesExtension : IMarkdownExtension
    {
        private readonly FlexiTablesExtensionOptions _options;

        public FlexiTablesExtension(FlexiTablesExtensionOptions options)
        {
            _options = options ?? new FlexiTablesExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Do nothing
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiTableRenderer>())
                {
                    HtmlTableRenderer htmlTableRenderer = htmlRenderer.ObjectRenderers.Find<HtmlTableRenderer>();
                    if (htmlTableRenderer != null)
                    {
                        htmlRenderer.ObjectRenderers.Remove(htmlTableRenderer);
                    }

                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiTableRenderer(_options.DefaultResponsiveTableOptions));
                }
            }
        }
    }
}
