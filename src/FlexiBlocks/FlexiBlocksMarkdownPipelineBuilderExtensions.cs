using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using System;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para><see cref="MarkdownPipelineBuilder"/> extensions for adding FlexiBlocks extensions.</para>
    /// </summary>
    public static class FlexiBlocksMarkdownPipelineBuilderExtensions
    {
        private static volatile IServiceCollection _services;
        private static volatile ServiceProvider _serviceProvider;
        private static readonly object _createLock = new object();

        private static ServiceProvider GetOrCreateServiceProvider()
        {
            if (_serviceProvider == null || _services != null)
            {
                lock (_createLock)
                {
                    if (_serviceProvider == null || _services != null)
                    {
                        // Dispose of service provider
                        _serviceProvider?.Dispose();

                        // Create new service provider
                        (_services ?? (_services = new ServiceCollection())).AddFlexiBlocks();
                        _serviceProvider = _services.BuildServiceProvider();
                        _services = null;
                    }
                }
            }

            return _serviceProvider;
        }

        /// <summary>
        /// <para>Disposes the underlying <see cref="IServiceProvider"/> used to resolve FlexiBlocks services.</para>
        /// <para>This method is not thread safe.</para>
        /// </summary>
        public static void DisposeServiceProvider()
        {
            _serviceProvider?.Dispose();
            _serviceProvider = null;
        }

        /// <summary>
        /// <para>Configures options.</para>
        /// <para>This method is not thread safe.</para>
        /// </summary>
        /// <typeparam name="T">The type of options to configure.</typeparam>
        /// <param name="configureOptions">The action that configures the options.</param>
        public static void Configure<T>(Action<T> configureOptions) where T : class
        {
            (_services ?? (_services = new ServiceCollection())).Configure(configureOptions);
        }

        /// <summary>
        /// Adds all FlexiBlocks extensions to the specified <see cref="MarkdownPipelineBuilder"/>.
        /// </summary>
        /// <param name="pipelineBuilder">The <see cref="MarkdownPipelineBuilder"/> to add the extensions to.</param>
        /// <param name="alertBlocksExtensionOptions">Options for the <see cref="FlexiAlertBlocksExtension"/>.</param>
        /// <param name="codeBlocksExtensionOptions">Options for the <see cref="FlexiCodeBlocksExtension"/>.</param>
        /// <param name="includeBlocksExtensionOptions">Options for the <see cref="FlexiIncludeBlocksExtension"/>.</param>
        /// <param name="sectionBlocksExtensionOptions">Options for the <see cref="FlexiSectionBlocksExtension"/>.</param>
        /// <param name="tableBlocksExtensionOptions">Options for the <see cref="FlexiTableBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiAlertBlocksExtensionOptions alertBlocksExtensionOptions = null,
            FlexiCodeBlocksExtensionOptions codeBlocksExtensionOptions = null,
            FlexiIncludeBlocksExtensionOptions includeBlocksExtensionOptions = null,
            FlexiSectionBlocksExtensionOptions sectionBlocksExtensionOptions = null,
            FlexiTableBlocksExtensionOptions tableBlocksExtensionOptions = null)
        {
            return pipelineBuilder.
                UseFlexiOptionsBlocks().
                UseFlexiAlertBlocks(alertBlocksExtensionOptions).
                UseFlexiCodeBlocks(codeBlocksExtensionOptions).
                UseFlexiIncludeBlocks(includeBlocksExtensionOptions).
                UseFlexiSectionBlocks(sectionBlocksExtensionOptions).
                UseFlexiTableBlocks(tableBlocksExtensionOptions);
        }

        /// <summary>
        /// Adds the <see cref="FlexiOptionsBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder"></param> 
        public static MarkdownPipelineBuilder UseFlexiOptionsBlocks(this MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiOptionsBlocksExtension>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<FlexiOptionsBlocksExtension>());
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiAlertBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiAlertBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiAlertBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiAlertBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiAlertBlocksExtension>())
            {
                IFlexiBlocksExtensionFactory<FlexiAlertBlocksExtension, FlexiAlertBlocksExtensionOptions> extensionFactory =
                    GetOrCreateServiceProvider().GetRequiredService<IFlexiBlocksExtensionFactory<FlexiAlertBlocksExtension, FlexiAlertBlocksExtensionOptions>>();
                pipelineBuilder.Extensions.Add(extensionFactory.Build(options));
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiCodeBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiCodeBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiCodeBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiCodeBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiCodeBlocksExtension>())
            {
                IFlexiBlocksExtensionFactory<FlexiCodeBlocksExtension, FlexiCodeBlocksExtensionOptions> extensionFactory =
                    GetOrCreateServiceProvider().GetRequiredService<IFlexiBlocksExtensionFactory<FlexiCodeBlocksExtension, FlexiCodeBlocksExtensionOptions>>();
                pipelineBuilder.Extensions.Add(extensionFactory.Build(options));
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiIncludeBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiIncludeBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiIncludeBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiIncludeBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiIncludeBlocksExtension>())
            {
                IFlexiBlocksExtensionFactory<FlexiIncludeBlocksExtension, FlexiIncludeBlocksExtensionOptions> extensionFactory =
                    GetOrCreateServiceProvider().GetRequiredService<IFlexiBlocksExtensionFactory<FlexiIncludeBlocksExtension, FlexiIncludeBlocksExtensionOptions>>();
                pipelineBuilder.Extensions.Add(extensionFactory.Build(options));
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiSectionBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiSectionBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiSectionBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiSectionBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiSectionBlocksExtension>())
            {
                IFlexiBlocksExtensionFactory<FlexiSectionBlocksExtension, FlexiSectionBlocksExtensionOptions> extensionFactory =
                    GetOrCreateServiceProvider().GetRequiredService<IFlexiBlocksExtensionFactory<FlexiSectionBlocksExtension, FlexiSectionBlocksExtensionOptions>>();
                pipelineBuilder.Extensions.Add(extensionFactory.Build(options));
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiTableBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiTableBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiTableBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            FlexiTableBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<FlexiTableBlocksExtension>())
            {
                IFlexiBlocksExtensionFactory<FlexiTableBlocksExtension, FlexiTableBlocksExtensionOptions> extensionFactory =
                    GetOrCreateServiceProvider().GetRequiredService<IFlexiBlocksExtensionFactory<FlexiTableBlocksExtension, FlexiTableBlocksExtensionOptions>>();
                pipelineBuilder.Extensions.Add(extensionFactory.Build(options));
            }

            return pipelineBuilder;
        }
    }
}
