using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions.ResponsiveTables
{
    public class ResponsiveTablesExtension : IMarkdownExtension
    {
        private readonly ResponsiveTablesExtensionOptions _options;

        public ResponsiveTablesExtension(ResponsiveTablesExtensionOptions options)
        {
            _options = options ?? new ResponsiveTablesExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Do nothing
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<ResponsiveTableRenderer>())
                {
                    HtmlTableRenderer htmlTableRenderer = htmlRenderer.ObjectRenderers.Find<HtmlTableRenderer>();
                    if (htmlTableRenderer != null)
                    {
                        htmlRenderer.ObjectRenderers.Remove(htmlTableRenderer);
                    }

                    htmlRenderer.ObjectRenderers.Insert(0, new ResponsiveTableRenderer(_options.DefaultResponsiveTableOptions));
                }
            }
        }
    }
}
