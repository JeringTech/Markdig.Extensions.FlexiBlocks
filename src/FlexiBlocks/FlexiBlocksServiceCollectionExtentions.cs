using Jering.IocServices.Newtonsoft.Json;
using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions for FlexiBlocks.
    /// </summary>
    public static class FlexiBlocksServiceCollectionExtentions
    {
        /// <summary>
        /// Registers services for FlexiBlocks.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services to.</param>
        public static void AddFlexiBlocks(this IServiceCollection services)
        {
            // Shared
            services.AddLogging();
            services.AddOptions();
            services.TryAddSingleton<IFileService, FileService>();
            services.TryAddSingleton<IDirectoryService, DirectoryService>();
            services.TryAddSingleton<IHttpClientService, HttpClientService>();
            services.TryAddSingleton<IJsonSerializerService, JsonSerializerService>();

            // FlexAlertBlocks
            services.TryAddSingleton<IFlexiBlocksExtensionFactory<FlexiAlertBlocksExtension, FlexiAlertBlocksExtensionOptions>, FlexiAlertBlocksExtensionFactory>();
            services.TryAddSingleton<FlexiAlertBlockRenderer>();

            // FlexiCodeBlocks
            services.TryAddSingleton<IFlexiBlocksExtensionFactory<FlexiCodeBlocksExtension, FlexiCodeBlocksExtensionOptions>, FlexiCodeBlocksExtensionFactory>();
            services.TryAddSingleton<FlexiCodeBlockRenderer>();
            services.TryAddSingleton<ILineEmbellisherService, LineEmbellisherService>();
            services.AddHighlightJS();
            services.AddPrism();

            // FlexiIncludeBlocks
            services.TryAddSingleton<IFlexiBlocksExtensionFactory<FlexiIncludeBlocksExtension, FlexiIncludeBlocksExtensionOptions>, FlexiIncludeBlocksExtensionFactory>();
            services.TryAddSingleton<IDiskCacheService, DiskCacheService>();
            services.TryAddSingleton<ISourceRetrieverService, SourceRetrieverService>();
            services.TryAddSingleton<ILeadingWhitespaceEditorService, LeadingWhitespaceEditorService>();

            // FlexiOptionsBlocks
            services.TryAddSingleton<IFlexiOptionsBlockService, FlexiOptionsBlockService>();
            services.TryAddSingleton<FlexiOptionsBlockParser>();
            services.TryAddSingleton<FlexiOptionsBlocksExtension>();

            // FlexiSectionBlocks
            services.TryAddSingleton<IFlexiBlocksExtensionFactory<FlexiSectionBlocksExtension, FlexiSectionBlocksExtensionOptions>, FlexiSectionBlocksExtensionFactory>();
            services.TryAddSingleton<FlexiSectionBlockRenderer>();

            // FlexiTableBlocks
            services.TryAddSingleton<IFlexiBlocksExtensionFactory<FlexiTableBlocksExtension, FlexiTableBlocksExtensionOptions>, FlexiTableBlocksExtensionFactory>();
        }
    }
}
