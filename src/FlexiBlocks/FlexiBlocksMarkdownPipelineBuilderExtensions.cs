using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
//using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
//using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    public static class FlexiBlocksMarkdownPipelineBuilderExtensions
    {
        private static IServiceCollection _services;
        private static IServiceProvider _serviceProvider;

        static FlexiBlocksMarkdownPipelineBuilderExtensions()
        {
            // The underlying service for running JS, INodeService, was built with DI in mind. Using DI does ensure that only one instance of INodeService
            // is ever created (since it is a singleton service). Every INodeService instance instantiated creates a new Node.js process, so using DI here
            // is fine.
            // TODO consider registering services for extensions, some renderers and parsers use services.

            // Default services
            _services = new ServiceCollection();
            _services.AddFlexiBlocks();
            SetServiceCollection(_services);
        }

        public static void SetServiceCollection(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            _serviceProvider = services.BuildServiceProvider();
        }

        public static IServiceCollection GetServiceCollection()
        {
            return _services;
        }

        /// <summary>
        /// Adds all FlexiBlock extensions to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder"></param>
        public static MarkdownPipelineBuilder UseFlexiBlocks(this MarkdownPipelineBuilder pipelineBuilder, IServiceProvider serviceProvider = null)
        {
            return pipelineBuilder.
                UseFlexiOptionsBlocks(serviceProvider).
                UseFlexiAlertBlocks(serviceProvider: serviceProvider).
                UseFlexiCodeBlocks(serviceProvider: serviceProvider).
                UseFlexiIncludeBlocks(serviceProvider: serviceProvider);
        }

        //public static MarkdownPipelineBuilder UseFlexiSectionBlocks(this MarkdownPipelineBuilder pipelineBuilder, FlexiSectionBlocksExtensionOptions options = null)
        //{
        //    if (!pipelineBuilder.Extensions.Contains<FlexiSectionBlocksExtension>())
        //    {
        //        pipelineBuilder.Extensions.Add(new FlexiSectionBlocksExtension(options));
        //    }

        //    return pipelineBuilder;
        //}

        /// <summary>
        /// Adds <see cref="FlexiAlertBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiAlertBlocksExtension"/>.</param>
        /// <param name="serviceProvider">Alternative service provider for resolving the <see cref="FlexiAlertBlocksExtension"/> service.</param>
        public static MarkdownPipelineBuilder UseFlexiAlertBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiAlertBlocksExtensionOptions options = null,
            IServiceProvider serviceProvider = null)
        {
            serviceProvider = serviceProvider ?? _serviceProvider;

            if (options != null)
            {
                SetOptions(options, serviceProvider);
            }

            if (!pipelineBuilder.Extensions.Contains<FlexiAlertBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<FlexiAlertBlocksExtension>());
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds <see cref="FlexiCodeBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiCodeBlocksExtension"/>.</param>
        /// <param name="serviceProvider">Alternative service provider for resolving the <see cref="FlexiCodeBlocksExtension"/> service.</param>
        public static MarkdownPipelineBuilder UseFlexiCodeBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiCodeBlocksExtensionOptions options = null,
            IServiceProvider serviceProvider = null)
        {
            serviceProvider = serviceProvider ?? _serviceProvider;

            if (options != null)
            {
                SetOptions(options, serviceProvider);
            }

            if (!pipelineBuilder.Extensions.Contains<FlexiCodeBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<FlexiCodeBlocksExtension>());
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds <see cref="FlexiIncludeBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiIncludeBlocksExtension"/>.</param>
        /// <param name="serviceProvider">Alternative service provider for resolving the <see cref="FlexiIncludeBlocksExtension"/> service.</param>
        public static MarkdownPipelineBuilder UseFlexiIncludeBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiIncludeBlocksExtensionOptions options = null,
            IServiceProvider serviceProvider = null)
        {
            serviceProvider = serviceProvider ?? _serviceProvider;

            if (options != null)
            {
                SetOptions(options, serviceProvider);
            }

            if (!pipelineBuilder.Extensions.Contains<FlexiIncludeBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<FlexiIncludeBlocksExtension>());
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds <see cref="FlexiOptionsBlocksExtension"/> to the pipeline.
        /// </summary> 
        public static MarkdownPipelineBuilder UseFlexiOptionsBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            IServiceProvider serviceProvider = null)
        {
            serviceProvider = serviceProvider ?? _serviceProvider;

            if (!pipelineBuilder.Extensions.Contains<FlexiOptionsBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(serviceProvider.GetRequiredService<FlexiOptionsBlocksExtension>());
            }

            return pipelineBuilder;
        }

        //public static MarkdownPipelineBuilder UseFlexiTableBlocks(this MarkdownPipelineBuilder pipelineBuilder, FlexiTableBlocksExtensionOptions options = null)
        //{
        //    if (!pipelineBuilder.Extensions.Contains<FlexiTableBlocksExtension>())
        //    {
        //        pipelineBuilder.Extensions.Add(new FlexiTableBlocksExtension(options));
        //    }

        //    return pipelineBuilder;
        //}

        internal static void SetOptions<T>(T extensionOptions, IServiceProvider serviceProvider) where T : class, new()
        {
            if (serviceProvider.GetRequiredService<IOptions<T>>() is ExposedOptionsManager<T> optionsManager)
            {
                optionsManager.Value = extensionOptions;
            }
            else
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_UnableToSetOptions, typeof(T).Name));
            }
        }
    }
}
