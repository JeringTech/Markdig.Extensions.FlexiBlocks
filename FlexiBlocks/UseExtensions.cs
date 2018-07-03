﻿using FlexiBlocks.FlexiAlertBlocks;
using FlexiBlocks.FlexiCodeBlocks;
using FlexiBlocks.FlexiOptionsBlocks;
using FlexiBlocks.FlexiTableBlocks;
using FlexiBlocks.FlexiSectionBlocks;
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

        public static MarkdownPipelineBuilder UseFlexiSectionBlocks(this MarkdownPipelineBuilder pipelineBuilder, FlexiSectionBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiSectionBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiSectionBlocksExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseFlexiAlertBlocks(this MarkdownPipelineBuilder pipelineBuilder, FlexiAlertBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiAlertBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiAlertBlocksExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseFlexiOptionsBlocks(this MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiOptionsBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiOptionsBlocksExtension());
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseFlexiTableBlocks(this MarkdownPipelineBuilder pipelineBuilder, FlexiTableBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiTableBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiTableBlocksExtension(options));
            }

            return pipelineBuilder;
        }

        public static MarkdownPipelineBuilder UseFlexiCodeBlocks(this MarkdownPipelineBuilder pipelineBuilder, FlexiCodeBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiCodeBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(new FlexiCodeBlocksExtension(options,
                    _serviceProvider.GetRequiredService<IPrismService>(),
                    _serviceProvider.GetRequiredService<IHighlightJSService>()));
            }

            return pipelineBuilder;
        }
    }
}