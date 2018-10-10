using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    // How extension options work:
    // There are two goals for extension options. The first is that if Use* is called for different MarkdownPipelineBuilder instances, and different options
    // are specified, then different options should apply to each eventual MarkdownPipeline. The second is that the architecture should not compromise on
    // DI. DI makes it easy to ensure that certain services, such as HttpClientService, are used as singletons. This in turn yields measurable performance benefits.
    //
    // To achieve these goals
    // - A custom OptionsManager is assigned specified options before extensions are resolved. 
    // - Extensions, renderers, and parsers, are all registered as transient services. 
    // - _serviceProvider usage is always thread locked.
    // The result of these measures it that extensions, renderers, and parsers, for every MarkdownPipeline can reliably have their own extension options.
    //
    // Child service providers are a possible alternative to this pattern.
    /// <summary>
    /// <see cref="MarkdownPipelineBuilder"/> extensions for adding FlexiBlocks extensions.
    /// </summary>
    public static class FlexiBlocksMarkdownPipelineBuilderExtensions
    {
        private static readonly IServiceProvider _serviceProvider;
        private static readonly object _serviceProviderLock;

        static FlexiBlocksMarkdownPipelineBuilderExtensions()
        {
            _serviceProviderLock = new object();
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            _serviceProvider = services.BuildServiceProvider();
        }

        public static IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
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
                pipelineBuilder.Extensions.Add(_serviceProvider.GetRequiredService<FlexiOptionsBlocksExtension>());
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
                lock (_serviceProviderLock)
                {
                    if (options != null)
                    {
                        SetOptions(options, _serviceProvider);
                    }
                    pipelineBuilder.Extensions.Add(_serviceProvider.GetRequiredService<FlexiAlertBlocksExtension>());
                    if (options != null)
                    {
                        SetOptions<FlexiAlertBlocksExtensionOptions>(null, _serviceProvider);
                    }
                }
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
                lock (_serviceProviderLock)
                {
                    if (options != null)
                    {
                        SetOptions(options, _serviceProvider);
                    }
                    pipelineBuilder.Extensions.Add(_serviceProvider.GetRequiredService<FlexiCodeBlocksExtension>());
                    if (options != null)
                    {
                        SetOptions<FlexiCodeBlocksExtensionOptions>(null, _serviceProvider);
                    }
                }
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
                lock (_serviceProviderLock)
                {
                    if (options != null)
                    {
                        SetOptions(options, _serviceProvider);
                    }
                    pipelineBuilder.Extensions.Add(_serviceProvider.GetRequiredService<FlexiIncludeBlocksExtension>());
                    if (options != null)
                    {
                        SetOptions<FlexiIncludeBlocksExtensionOptions>(null, _serviceProvider);
                    }
                }
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
                lock (_serviceProviderLock)
                {
                    if (options != null)
                    {
                        SetOptions(options, _serviceProvider);
                    }
                    pipelineBuilder.Extensions.Add(_serviceProvider.GetRequiredService<FlexiSectionBlocksExtension>());
                    if (options != null)
                    {
                        SetOptions<FlexiSectionBlocksExtensionOptions>(null, _serviceProvider);
                    }
                }
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
                lock (_serviceProviderLock)
                {
                    if (options != null)
                    {
                        SetOptions(options, _serviceProvider);
                    }
                    pipelineBuilder.Extensions.Add(_serviceProvider.GetRequiredService<FlexiTableBlocksExtension>());
                    if (options != null)
                    {
                        SetOptions<FlexiTableBlocksExtensionOptions>(null, _serviceProvider);
                    }
                }
            }

            return pipelineBuilder;
        }

        internal static void SetOptions<T>(this T extensionOptions, IServiceProvider serviceProvider) where T : class, new()
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
