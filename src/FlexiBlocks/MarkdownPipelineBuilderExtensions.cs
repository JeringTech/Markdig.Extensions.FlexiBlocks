using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using System;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Markdig.Extensions.Citations;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para><see cref="MarkdownPipelineBuilder"/> extensions for adding FlexiBlocks extensions.</para>
    /// </summary>
    public static class MarkdownPipelineBuilderExtensions
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
        /// <param name="includeBlocksExtensionOptions">Options for the <see cref="IncludeBlocksExtensionOptions"/>.</param>
        /// <param name="flexiAlertBlocksExtensionOptions">Options for the <see cref="FlexiAlertBlocksExtension"/>.</param>
        /// <param name="flexiCodeBlocksExtensionOptions">Options for the <see cref="FlexiCodeBlocksExtension"/>.</param>
        /// <param name="flexiQuoteBlocksExtensionOptions">Options for the <see cref="FlexiQuoteBlocksExtension"/>.</param>
        /// <param name="flexiSectionBlocksExtensionOptions">Options for the <see cref="FlexiSectionBlocksExtension"/>.</param>
        /// <param name="flexiTableBlocksExtensionOptions">Options for the <see cref="FlexiTableBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiBlocks(this MarkdownPipelineBuilder pipelineBuilder,
            IIncludeBlocksExtensionOptions includeBlocksExtensionOptions = null,
            IFlexiAlertBlocksExtensionOptions flexiAlertBlocksExtensionOptions = null,
            IFlexiCodeBlocksExtensionOptions flexiCodeBlocksExtensionOptions = null,
            IFlexiQuoteBlocksExtensionOptions flexiQuoteBlocksExtensionOptions = null,
            IFlexiSectionBlocksExtensionOptions flexiSectionBlocksExtensionOptions = null,
            IFlexiTableBlocksExtensionOptions flexiTableBlocksExtensionOptions = null)
        {
            return pipelineBuilder.
                UseContextObjects().
                UseIncludeBlocks(includeBlocksExtensionOptions).
                UseOptionsBlocks().
                UseFlexiAlertBlocks(flexiAlertBlocksExtensionOptions).
                UseFlexiCodeBlocks(flexiCodeBlocksExtensionOptions).
                UseFlexiQuoteBlocks(flexiQuoteBlocksExtensionOptions).
                UseFlexiSectionBlocks(flexiSectionBlocksExtensionOptions).
                UseFlexiTableBlocks(flexiTableBlocksExtensionOptions);
        }

        /// <summary>
        /// Adds the <see cref="ContextObjectsExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        public static MarkdownPipelineBuilder UseContextObjects(this MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.Extensions.Contains<ContextObjectsExtension>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<ContextObjectsExtension>());
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="IncludeBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="IncludeBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseIncludeBlocks(this MarkdownPipelineBuilder pipelineBuilder, IIncludeBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<IncludeBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<IncludeBlock>>());
            }

            if (options != null)
            {
                AddContextObjectWithTypeAsKey(pipelineBuilder, options);
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="OptionsBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param> 
        public static MarkdownPipelineBuilder UseOptionsBlocks(this MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<OptionsBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<OptionsBlock>>());
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiAlertBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiAlertBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiAlertBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiAlertBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<FlexiAlertBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<FlexiAlertBlock>>());
            }

            if (options != null)
            {
                AddContextObjectWithTypeAsKey(pipelineBuilder, options);
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiCodeBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiCodeBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiCodeBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiCodeBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<FlexiCodeBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<FlexiCodeBlock>>());
            }

            if (options != null)
            {
                AddContextObjectWithTypeAsKey(pipelineBuilder, options);
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiQuoteBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiQuoteBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiQuoteBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiQuoteBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<FlexiQuoteBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<FlexiQuoteBlock>>());
            }

            if (options != null)
            {
                AddContextObjectWithTypeAsKey(pipelineBuilder, options);
            }

            pipelineBuilder.Extensions.AddIfNotAlready<CitationExtension>();

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiSectionBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiSectionBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiSectionBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiSectionBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<FlexiSectionBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<FlexiSectionBlock>>());
            }

            if (options != null)
            {
                AddContextObjectWithTypeAsKey(pipelineBuilder, options);
            }

            return pipelineBuilder;
        }

        /// <summary>
        /// Adds the <see cref="FlexiTableBlocksExtension"/> to the pipeline.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder for the pipeline.</param>
        /// <param name="options">Options for the <see cref="FlexiTableBlocksExtension"/>.</param>
        public static MarkdownPipelineBuilder UseFlexiTableBlocks(this MarkdownPipelineBuilder pipelineBuilder, IFlexiTableBlocksExtensionOptions options = null)
        {
            if (!pipelineBuilder.Extensions.Contains<IBlockExtension<FlexiTableBlock>>())
            {
                pipelineBuilder.Extensions.Add(GetOrCreateServiceProvider().GetRequiredService<IBlockExtension<FlexiTableBlock>>());
            }

            if (options != null)
            {
                AddContextObjectWithTypeAsKey(pipelineBuilder, options);
            }

            return pipelineBuilder;
        }

        private static void AddContextObjectWithTypeAsKey<T>(MarkdownPipelineBuilder pipelineBuilder, T contextObject)
        {
            ContextObjectsExtension contextObjectsExtension = pipelineBuilder.Extensions.Find<ContextObjectsExtension>();

            if (contextObjectsExtension == null)
            {
                contextObjectsExtension = GetOrCreateServiceProvider().GetRequiredService<ContextObjectsExtension>();
                pipelineBuilder.Extensions.Add(contextObjectsExtension);
            }

            contextObjectsExtension.ContextObjectsStore.Add(typeof(T), contextObject);
        }
    }
}
