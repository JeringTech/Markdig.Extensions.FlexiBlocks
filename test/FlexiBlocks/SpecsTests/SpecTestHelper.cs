using Markdig;
using Newtonsoft.Json;
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
        private static readonly Dictionary<string, MethodInfo> _useExtensionMethods = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, Type> _extensionOptionsTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer() { DefaultValueHandling = DefaultValueHandling.Populate };
        private static readonly Assembly _flexiBlockAssembly = typeof(MarkdownPipelineBuilderExtensions).Assembly;
        private static readonly string[] _extensionNames;

        static SpecTestHelper()
        {
            // Populate UseExtensionMethods and ExtensionOptionsTypes for constructing pipelines
            Type useExtensions = typeof(MarkdownPipelineBuilderExtensions);
            foreach (MethodInfo methodInfo in useExtensions.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (!methodInfo.Name.StartsWith("Use") || methodInfo.Name == "UseFlexiBlocks") // Don't include general use method
                {
                    continue;
                }

                string extensionName = methodInfo.Name.Replace("Use", "");
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length > 1) // Assume methods that take extension options all have similar signatures, (MarkdownPipelineBuilder, ExtensionOptions)
                {
                    _extensionOptionsTypes.Add(extensionName, GetFirstImplementingType(parameters[1].ParameterType)); // Assume extension options each have only 1 implementing type in the FlexiBlocks assembly
                }
                _useExtensionMethods.Add(extensionName, methodInfo);
            }

            _extensionNames = _useExtensionMethods.Keys.ToArray();
        }

        public static void AssertCompliance(string markdown,
            string expectedHtml,
            string pipelineOptions,
            bool classicRendering,
            string extensionOptionsJson = null)
        {
            MarkdownPipeline pipeline = CreatePipeline(pipelineOptions, extensionOptionsJson, classicRendering);
            string result = Markdown.ToHtml(markdown, pipeline);
            result = Compact(result);
            string expectedResult = Compact(expectedHtml);

            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        private static MarkdownPipeline CreatePipeline(string pipelineOptions, string allExtensionOptionsJson, bool classicRendering)
        {
            var builder = new MarkdownPipelineBuilder();
            // Vanilla extensions only
            if (pipelineOptions.Equals("CommonMark", StringComparison.OrdinalIgnoreCase))
            {
                return builder.Build();
            }

            JObject allExtensionOptionsJObject = allExtensionOptionsJson == null ? null : JObject.Parse(allExtensionOptionsJson);

            foreach (string extension in pipelineOptions.Equals("all", StringComparison.OrdinalIgnoreCase) ? _extensionNames : pipelineOptions.Split('_'))
            {
                if (!_useExtensionMethods.TryGetValue(extension, out MethodInfo useExtensionMethod))
                {
                    throw new InvalidOperationException($"The extension {extension} does not exist.");
                }

                object[] args = null;
                JToken extensionOptionsJToken;
                if (!_extensionOptionsTypes.TryGetValue(extension, out Type extensionOptionsType)) // Extension extension method has no options parameter
                {
                    args = new object[] { builder };
                }
                else if ((extensionOptionsJToken = allExtensionOptionsJObject?.GetValue(extension, StringComparison.OrdinalIgnoreCase)) == null && // Extension extension method has options parameter but no value was provided
                    !classicRendering)
                {
                    args = new object[] { builder, null };
                }
                else
                {
                    // If we're running CommonMark specs, we want classic rendering output
                    if (classicRendering)
                    {
                        if (extensionOptionsJToken == null)
                        {
                            extensionOptionsJToken = JToken.Parse("{defaultBlockOptions: { \"renderingMode\": \"classic\"}}");
                        }
                        else
                        {
                            extensionOptionsJToken["defaultBlockOptions"]["renderingMode"] = "classic";
                        }
                    }

                    object extensionOptions = Activator.CreateInstance(extensionOptionsType);
                    _jsonSerializer.Populate(extensionOptionsJToken.CreateReader(), extensionOptions);
                    args = new object[] { builder, extensionOptions };
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
            return html.Normalize(NormalizationForm.FormKD);
        }

        public static Type GetFirstImplementingType(Type type)
        {
            foreach (TypeInfo typeInfo in _flexiBlockAssembly.DefinedTypes)
            {
                if (typeInfo.ImplementedInterfaces.Contains(type))
                {
                    return typeInfo.AsType();
                }
            }

            return null;
        }
    }
}
