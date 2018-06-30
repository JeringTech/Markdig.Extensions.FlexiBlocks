using JeremyTCD.Markdig.Extensions.Alerts;
using JeremyTCD.Markdig.Extensions.FlexiCode;
using JeremyTCD.Markdig.Extensions.JsonOptions;
using JeremyTCD.Markdig.Extensions.ResponsiveTables;
using JeremyTCD.Markdig.Extensions.Sections;
using JeremyTCD.WebUtils.SyntaxHighlighters.HighlightJS;
using JeremyTCD.WebUtils.SyntaxHighlighters.Prism;
using Markdig;
using Microsoft.Extensions.DependencyInjection;

namespace JeremyTCD.Markdig.Extensions
{
    public static class UseExtensions
    {
        private static readonly ServiceProvider _serviceProvider;

        static UseExtensions()
        {
            // The underlying service for running JS, INodeService, was built with DI in mind. Using DI does ensure that only one instance of INodeService
            // is ever created (since it is a singleton service). Every INodeService instance instantiated creates a new Node.js process, so using DI here
            // is fine.
            // TODO consider registering services for extensions, some renderers and parsers use services.
            var services = new ServiceCollection();
            services.
                AddPrism().
                AddHighlightJS();
            _serviceProvider = services.BuildServiceProvider();
        }

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
                pipelineBuilder.Extensions.Add(new FlexiCodeExtension(options,
                    _serviceProvider.GetRequiredService<IPrismService>(),
                    _serviceProvider.GetRequiredService<IHighlightJSService>()));
            }

            return pipelineBuilder;
        }
    }
}
