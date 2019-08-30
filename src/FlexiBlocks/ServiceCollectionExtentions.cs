using Jering.IocServices.Newtonsoft.Json;
using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using System.Linq;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions for FlexiBlocks.
    /// </summary>
    public static class ServiceCollectionExtentions
    {
        /// <summary>
        /// Adds services for FlexiBlocks.
        /// </summary>
        public static IServiceCollection AddFlexiBlocks(this IServiceCollection services)
        {
            return services.
                AddContextObjects().
                AddIncludeBlocks().
                AddOptionsBlocks().
                AddFlexiAlertBlocks().
                AddFlexiBannerBlocks().
                AddFlexiCodeBlocks().
                AddFlexiFigureBlocks().
                AddFlexiQuoteBlocks().
                AddFlexiSectionBlocks().
                AddFlexiTableBlocks();
        }

        /// <summary>
        /// Adds services for the <see cref="ContextObjectsExtension"/>.
        /// </summary>
        public static IServiceCollection AddContextObjects(this IServiceCollection services)
        {
            services.TryAddSingleton<IContextObjectsService, ContextObjectsService>();
            services.TryAddTransient<ContextObjectsExtension>(); // Transient since context objects are unique to each MarkdownPipeline
            services.TryAddTransient<ContextObjectsStore>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="IncludeBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddIncludeBlocks(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptionsService<IIncludeBlocksExtensionOptions, IncludeBlocksExtensionOptions, IIncludeBlockOptions>();
            services.TryAddSingleton<IFileService, FileService>();
            services.TryAddSingleton<IDirectoryService, DirectoryService>();
            services.TryAddSingleton<IHttpClientService, HttpClientService>();
            services.TryAddSingleton<IBlockExtension<IncludeBlock>, IncludeBlocksExtension>();
            services.TryAddSingleton<ProxyBlockParser<IncludeBlock, ProxyJsonBlock>, IncludeBlockParser>();
            services.TryAddSingleton<IJsonBlockFactory<IncludeBlock, ProxyJsonBlock>, IncludeBlockFactory>();
            services.TryAddSingleton<IDiskCacheService, DiskCacheService>();
            services.TryAddSingleton<IContentRetrieverService, ContentRetrieverService>();
            services.TryAddSingleton<ILeadingWhitespaceEditorService, LeadingWhitespaceEditorService>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="OptionsBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddOptionsBlocks(this IServiceCollection services)
        {
            services.TryAddSingleton<IBlockExtension<OptionsBlock>, OptionsBlocksExtension>();
            services.TryAddSingleton<ProxyBlockParser<OptionsBlock, ProxyJsonBlock>, OptionsBlockParser>();
            services.TryAddSingleton<IJsonBlockFactory<OptionsBlock, ProxyJsonBlock>, OptionsBlockFactory>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiAlertBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiAlertBlocks(this IServiceCollection services)
        {
            services.AddOptionsService<IFlexiAlertBlocksExtensionOptions, FlexiAlertBlocksExtensionOptions, IFlexiAlertBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiAlertBlock>, FlexiAlertBlocksExtension>();
            services.TryAddSingleton<BlockParser<FlexiAlertBlock>, FlexiAlertBlockParser>();
            services.TryAddSingleton<BlockRenderer<FlexiAlertBlock>, FlexiAlertBlockRenderer>();
            services.TryAddSingleton<IFlexiAlertBlockFactory, FlexiAlertBlockFactory>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiBannerBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiBannerBlocks(this IServiceCollection services)
        {
            services.AddOptionsService<IFlexiBannerBlocksExtensionOptions, FlexiBannerBlocksExtensionOptions, IFlexiBannerBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiBannerBlock>, FlexiBannerBlocksExtension>();
            services.TryAddSingleton<BlockParser<FlexiBannerBlock>, FlexiBannerBlockParser>();
            services.TryAddSingleton<BlockRenderer<FlexiBannerBlock>, FlexiBannerBlockRenderer>();
            services.TryAddSingleton<IMultipartBlockFactory<FlexiBannerBlock>, FlexiBannerBlockFactory>();
            services.TryAddSingleton<PartBlockParser>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiCodeBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiCodeBlocks(this IServiceCollection services)
        {
            services.AddHighlightJS();
            services.AddPrism();
            services.AddOptionsService<IFlexiCodeBlocksExtensionOptions, FlexiCodeBlocksExtensionOptions, IFlexiCodeBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiCodeBlock>, FlexiCodeBlocksExtension>();
            services.TryAddSingleton<BlockRenderer<FlexiCodeBlock>, FlexiCodeBlockRenderer>();
            services.TryAddSingleton<IFlexiCodeBlockFactory, FlexiCodeBlockFactory>();
            services.TryAddSingleton<ProxyBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock>, FencedFlexiCodeBlockParser>();
            services.TryAddSingleton<ProxyBlockParser<FlexiCodeBlock, ProxyLeafBlock>, IndentedFlexiCodeBlockParser>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiFigureBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiFigureBlocks(this IServiceCollection services)
        {
            services.AddOptionsService<IFlexiFigureBlocksExtensionOptions, FlexiFigureBlocksExtensionOptions, IFlexiFigureBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiFigureBlock>, FlexiFigureBlocksExtension>();
            services.TryAddSingleton<BlockParser<FlexiFigureBlock>, FlexiFigureBlockParser>();
            services.TryAddSingleton<BlockRenderer<FlexiFigureBlock>, FlexiFigureBlockRenderer>();
            services.TryAddSingleton<IMultipartBlockFactory<FlexiFigureBlock>, FlexiFigureBlockFactory>();
            services.TryAddSingleton<PartBlockParser>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiQuoteBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiQuoteBlocks(this IServiceCollection services)
        {
            services.AddOptionsService<IFlexiQuoteBlocksExtensionOptions, FlexiQuoteBlocksExtensionOptions, IFlexiQuoteBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiQuoteBlock>, FlexiQuoteBlocksExtension>();
            services.TryAddSingleton<BlockParser<FlexiQuoteBlock>, FlexiQuoteBlockParser>();
            services.TryAddSingleton<BlockRenderer<FlexiQuoteBlock>, FlexiQuoteBlockRenderer>();
            services.TryAddSingleton<IMultipartBlockFactory<FlexiQuoteBlock>, FlexiQuoteBlockFactory>();
            services.TryAddSingleton<PartBlockParser>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiSectionBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiSectionBlocks(this IServiceCollection services)
        {
            services.AddOptionsService<IFlexiSectionBlocksExtensionOptions, FlexiSectionBlocksExtensionOptions, IFlexiSectionBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiSectionBlock>, FlexiSectionBlocksExtension>();
            services.TryAddSingleton<BlockParser<FlexiSectionBlock>, FlexiSectionBlockParser>();
            services.TryAddSingleton<IFlexiSectionBlockFactory, FlexiSectionBlockFactory>();
            services.TryAddSingleton<IFlexiSectionHeadingBlockFactory, FlexiSectionHeadingBlockFactory>();
            services.TryAddSingleton<BlockRenderer<FlexiSectionBlock>, FlexiSectionBlockRenderer>();

            return services;
        }

        /// <summary>
        /// Adds services for the <see cref="FlexiTableBlocksExtension"/>.
        /// </summary>
        public static IServiceCollection AddFlexiTableBlocks(this IServiceCollection services)
        {
            services.AddOptionsService<IFlexiTableBlocksExtensionOptions, FlexiTableBlocksExtensionOptions, IFlexiTableBlockOptions>();
            services.TryAddSingleton<IBlockExtension<FlexiTableBlock>, FlexiTableBlocksExtension>();
            services.TryAddSingleton<IFlexiTableBlockFactory, FlexiTableBlockFactory>();
            services.TryAddSingleton<BlockRenderer<FlexiTableBlock>, FlexiTableBlockRenderer>();

            // TryAddSingleton will only add 1 implementation per interface. Since we're injecting these in an IEnumerable, we must 
            // add them using AddSingleton. To avoid adding implementations of the same type multiple times, we ensure they have not been
            // registered.
            Type proxyBlockParserType = typeof(ProxyBlockParser<FlexiTableBlock, ProxyTableBlock>);
            Type advancedFlexiTableBlockParserType = typeof(AdvancedFlexiTableBlockParser);
            if (!services.Any(serviceDescriptor => serviceDescriptor.ServiceType == proxyBlockParserType && serviceDescriptor.ImplementationType == advancedFlexiTableBlockParserType))
            {
                services.AddSingleton<ProxyBlockParser<FlexiTableBlock, ProxyTableBlock>, AdvancedFlexiTableBlockParser>();
            }

            Type basicFlexiTableBlockParser = typeof(BasicFlexiTableBlockParser);
            if (!services.Any(serviceDescriptor => serviceDescriptor.ServiceType == proxyBlockParserType && serviceDescriptor.ImplementationType == basicFlexiTableBlockParser))
            {
                services.AddSingleton<ProxyBlockParser<FlexiTableBlock, ProxyTableBlock>, BasicFlexiTableBlockParser>();
            }

            return services;
        }

        private static IServiceCollection AddOptionsService<TExtensionOptions, TDefaultExtensionOptions, TBlockOptions>(this IServiceCollection services)
            where TExtensionOptions : IExtensionOptions<TBlockOptions>
            where TDefaultExtensionOptions : class, TExtensionOptions, new()
            where TBlockOptions : IBlockOptions<TBlockOptions>
        {
            services.TryAddSingleton<IJsonSerializerService, JsonSerializerService>();
            services.TryAddSingleton<IContextObjectsService, ContextObjectsService>();
            services.TryAddSingleton(typeof(IBlockOptionsFactory<>), typeof(BlockOptionsFactory<>));
            services.TryAddSingleton(typeof(IOptionsService<,>), typeof(OptionsService<,>));
            services.TryAddSingleton<IExtensionOptionsFactory<TExtensionOptions, TBlockOptions>, ExtensionOptionsFactory<TExtensionOptions, TDefaultExtensionOptions, TBlockOptions>>();

            return services;
        }
    }
}
