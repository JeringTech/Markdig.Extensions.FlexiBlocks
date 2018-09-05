using Jering.IocServices.Newtonsoft.Json;
using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
//using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    public static class FlexiBlocksServiceCollectionExtentions
    {
        public static void AddFlexiBlocks(this IServiceCollection services)
        {
            // Third party services
            services.AddPrism();
            services.AddHighlightJS();
            services.AddLogging();
            services.AddOptions();
            services.TryAddSingleton<IFileService, FileService>();
            services.TryAddSingleton<IDirectoryService, DirectoryService>();
            services.TryAddSingleton<IHttpClientService, HttpClientService>();
            services.TryAddSingleton<IJsonSerializerService, JsonSerializerService>();

            // Shared
#if NETSTANDARD1_3
            services.AddSingleton(typeof(IOptions<>), typeof(ExposedOptionsManager<>));
#endif

            // FlexiOptionsBlocks
            services.AddSingleton<IFlexiOptionsBlockService, FlexiOptionsBlockService>();
            services.AddSingleton<FlexiOptionsBlockParser>();
            services.AddSingleton<FlexiOptionsBlocksExtension>();

            // FlexAlertBlocks
            services.AddSingleton<FlexiAlertBlockParser>();
            services.AddSingleton<FlexiAlertBlockRenderer>();
            services.AddSingleton<FlexiAlertBlocksExtension>();

            // FlexiIncludeBlocks
            //services.AddSingleton<IFileCacheService, FileCacheService>();
            //services.AddSingleton<IContentRetrieverService, ContentRetrieverService>();
            //services.AddSingleton<FlexiIncludeBlockParser>();
            //services.AddSingleton<FlexiIncludeBlocksExtension>();
        }
    }
}
