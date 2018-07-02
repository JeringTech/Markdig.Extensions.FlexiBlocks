using FlexiBlocks.JsonOptions;
using Markdig;
using Markdig.Renderers;

namespace FlexiBlocks.Alerts
{
    public class FlexiAlertsExtension : IMarkdownExtension
    {
        private readonly FlexiAlertsExtensionOptions _options;

        public FlexiAlertsExtension(FlexiAlertsExtensionOptions options)
        {
            _options = options ?? new FlexiAlertsExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<FlexiAlertBlockParser>())
            {
                var flexiOptionsService = new FlexiOptionsService();
                var flexiAlertBlockParser = new FlexiAlertBlockParser(_options, flexiOptionsService);
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
