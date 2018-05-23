using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsExtension : IMarkdownExtension
    {
        private readonly AlertsExtensionOptions _options;

        public AlertsExtension(AlertsExtensionOptions options)
        {
            _options = options ?? new AlertsExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<AlertBlockParser>())
            {
                var jsonOptionsService = new JsonOptionsService();
                var alertBlockParser = new AlertBlockParser(_options, jsonOptionsService);
                pipeline.BlockParsers.Insert(0, alertBlockParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<AlertBlockRenderer>())
            {
                htmlRenderer.ObjectRenderers.Insert(0, new AlertBlockRenderer());
            }
        }
    }
}
