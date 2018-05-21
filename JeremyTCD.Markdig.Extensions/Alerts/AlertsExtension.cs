using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsExtension : IMarkdownExtension
    {
        private readonly AlertsOptions _options;

        public AlertsExtension(AlertsOptions options)
        {
            _options = options ?? new AlertsOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<AlertsParser>())
            {
                var jsonOptionsService = new JsonOptionsService();
                var alertsParser = new AlertsParser(_options, jsonOptionsService);
                pipeline.BlockParsers.Insert(0, alertsParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<AlertsRenderer>())
            {
                htmlRenderer.ObjectRenderers.Insert(0, new AlertsRenderer());
            }
        }
    }
}
