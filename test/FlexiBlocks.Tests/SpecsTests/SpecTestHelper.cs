using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public static class SpecTestHelper
    {
        private static readonly Dictionary<string, MethodInfo> UseExtensionMethods = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, Type> ExtensionOptionsTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private static readonly string[] ExtensionNames;

        static SpecTestHelper()
        {
            // Populate UseExtensionMethods and ExtensionOptionsTypes for constructing pipelines
            Type flexiBlocksUseExtensions = typeof(FlexiBlocksMarkdownPipelineBuilderExtensions);
            foreach (MethodInfo methodInfo in flexiBlocksUseExtensions.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (!methodInfo.Name.StartsWith("Use") || methodInfo.Name == "UseFlexiBlocks") // Don't include general use method since it doesn't allow for extension options
                {
                    continue;
                }

                string extensionName = methodInfo.Name.Replace("Use", "");
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length > 1 && 
                    parameters[1].ParameterType.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFlexiBlocksExtensionOptions<>)))
                {
                    // Assume that use methods only take extension options as arguments
                    ExtensionOptionsTypes.Add(extensionName, parameters[1].ParameterType);
                }
                UseExtensionMethods.Add(extensionName, methodInfo);
            }

            ExtensionNames = UseExtensionMethods.Keys.ToArray();
        }

        public static void AssertCompliance(string markdown,
            string expectedHtml,
            string pipelineOptions,
            string extensionOptionsJson = null)
        {
            // Create service provider (use a fresh ServiceProvider to prevent specs from interfering with one another)
            var services = new ServiceCollection();
            services.AddFlexiBlocks();
            IServiceProvider serviceProvider = null;

            try
            {
                serviceProvider = services.BuildServiceProvider();
                MarkdownPipeline pipeline = CreatePipeline(pipelineOptions, extensionOptionsJson, serviceProvider);
                string result = Markdown.ToHtml(markdown, pipeline);
                result = Compact(result);
                string expectedResult = Compact(expectedHtml);

                Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
            }
            finally
            {
                (serviceProvider as IDisposable)?.Dispose(); // Can't use using block for IServiceProvider since in earlier netcoreapp versions, it does not implement IDisposable directly
            }
        }

        private static MarkdownPipeline CreatePipeline(string pipelineOptions, string extensionOptionsJson, IServiceProvider serviceProvider)
        {
            JObject extensionOptions = null;

            if (extensionOptionsJson != null)
            {
                extensionOptions = JObject.Parse(extensionOptionsJson);
            }

            var builder = new MarkdownPipelineBuilder();

            // Vanilla extensions only
            if (pipelineOptions.Equals("CommonMark", StringComparison.OrdinalIgnoreCase))
            {
                return builder.Build();
            }

            // TODO could be sped up
            string[] extensions = pipelineOptions.Equals("all", StringComparison.OrdinalIgnoreCase) ? ExtensionNames : pipelineOptions.Split('_');

            foreach (string extension in extensions)
            {
                if (!UseExtensionMethods.TryGetValue(extension, out MethodInfo useExtensionMethod))
                {
                    throw new InvalidOperationException($"The extension {extension} does not exist.");
                }

                object[] args;
                if (ExtensionOptionsTypes.TryGetValue(extension, out Type extensionOptionsType))
                {
                    args = new object[] { builder,
                        extensionOptions?.GetValue(extension, StringComparison.OrdinalIgnoreCase)?.ToObject(extensionOptionsType),
                        serviceProvider };
                }
                else
                {
                    args = new object[] { builder, serviceProvider };
                }

                useExtensionMethod.Invoke(null, args);
            }

            return builder.Build();
        }

        private static string Compact(string html)
        {
            // Normalize the output to make it compatible with CommonMark specs
            html = html.Replace("\r\n", "\n").Replace(@"\r", @"\n").Trim();
            html = Regex.Replace(html, @"\s+</li>", "</li>");
            html = Regex.Replace(html, @"<li>\s+", "<li>");
            html = html.Normalize(NormalizationForm.FormKD);
            return html;
        }
    }
}
