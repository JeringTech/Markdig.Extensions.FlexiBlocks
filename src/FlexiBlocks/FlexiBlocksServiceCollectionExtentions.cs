using Jering.IocServices.System.IO;
using Jering.IocServices.System.Net.Http;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

            // FlexiIncludeBlocks
            services.AddSingleton<IFileCacheService, FileCacheService>();
            services.AddSingleton<IContentRetrievalService, ContentRetrievalService>();
            services.AddSingleton<FlexiIncludeBlocksExtension>();
            services.AddSingleton<FlexiIncludeBlockParser>();
        }
    }
}
