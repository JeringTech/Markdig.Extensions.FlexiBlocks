using FlexiBlocks.Alerts;
using FlexiBlocks.FlexiCode;
using FlexiBlocks.JsonOptions;
using FlexiBlocks.ResponsiveTables;
using FlexiBlocks.Sections;
using JeremyTCD.WebUtils.SyntaxHighlighters.HighlightJS;
using JeremyTCD.WebUtils.SyntaxHighlighters.Prism;
using Markdig;
using Microsoft.Extensions.DependencyInjection;

namespace FlexiBlocks
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

        public static MarkdownPipelineBuilder UseSections(this MarkdownPipelineBuilder pipelineBuilder, FlexiSectionsExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiSectionsExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiSectionsExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseFlexiAlerts(this MarkdownPipelineBuilder pipelineBuilder, FlexiAlertsExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiAlertsExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiAlertsExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseJsonOptions(this MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiOptionsExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiOptionsExtension());
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseResponsiveTables(this MarkdownPipelineBuilder pipelineBuilder, FlexiTablesExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiTablesExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiTablesExtension(options));
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
