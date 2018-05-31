using JeremyTCD.Markdig.Extensions.Alerts;
using JeremyTCD.Markdig.Extensions.FlexiCode;
using JeremyTCD.Markdig.Extensions.JsonOptions;
using JeremyTCD.Markdig.Extensions.ResponsiveTables;
using JeremyTCD.Markdig.Extensions.Sections;
using Markdig;

namespace JeremyTCD.Markdig.Extensions
{
    public static class UseExtensions
    {
        public static MarkdownPipelineBuilder UseSections(this MarkdownPipelineBuilder pipelineBuilder, SectionsExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<SectionsExtension>())
            {
                pipelineBuilder.Extensions.Add(new SectionsExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseAlerts(this MarkdownPipelineBuilder pipelineBuilder, AlertsExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<AlertsExtension>())
            {
                pipelineBuilder.Extensions.Add(new AlertsExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseJsonOptions(this MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.Extensions.Contains<JsonOptionsExtension>())
            {
                pipelineBuilder.Extensions.Add(new JsonOptionsExtension());
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseResponsiveTables(this MarkdownPipelineBuilder pipelineBuilder, ResponsiveTablesExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<ResponsiveTablesExtension>())
            {
                pipelineBuilder.Extensions.Add(new ResponsiveTablesExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseFlexiCode(this MarkdownPipelineBuilder pipelineBuilder, FlexiCodeExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiCodeExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiCodeExtension(options));
            }

            return pipelineBuilder;
        }
    }
}
