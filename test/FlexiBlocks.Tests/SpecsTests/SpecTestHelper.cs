using Jering.Markdig.Extensions.FlexiBlocks. FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public static class SpecTestHelper
    {
        private static readonly Dictionary<string, Action<MarkdownPipelineBuilder, JObject>> _extensionAdders =
            new Dictionary<string, Action<MarkdownPipelineBuilder, JObject>>
            {
                { "flexioptionsblocks", (MarkdownPipelineBuilder builder, JObject _) => builder.UseFlexiOptionsBlocks() },
                { "flexicodeblocks", (MarkdownPipelineBuilder builder, JObject options) => builder.UseFlexiCodeBlocks(options?["flexicodeblocks"]?.ToObject<FlexiCodeBlocksExtensionOptions>()) },
                { "flexisectionblocks", (MarkdownPipelineBuilder builder, JObject options) => builder.UseFlexiSectionBlocks(options?["flexisectionblocks"]?.ToObject<FlexiSectionBlocksExtensionOptions>()) },
                { "flexialertblocks", (MarkdownPipelineBuilder builder, JObject options) => builder.UseFlexiAlertBlocks(options?["flexialertblocks"]?.ToObject<FlexiAlertBlocksExtensionOptions>()) },
                { "flexitableblocks", (MarkdownPipelineBuilder builder, JObject options) => builder.UseFlexiTableBlocks(options?["flexitableblocks"]?.ToObject<FlexiTableBlocksExtensionOptions>()) },
                { "pipetables", (MarkdownPipelineBuilder builder, JObject _) => builder.UsePipeTables() },
                { "gridtables", (MarkdownPipelineBuilder builder, JObject _) => builder.UseGridTables() },
                { "all", (MarkdownPipelineBuilder builder, JObject options) => {
                    builder.
                        UseFlexiTableBlocks(options?["flexitableblocks"]?.ToObject<FlexiTableBlocksExtensionOptions>()).
                        UseFlexiSectionBlocks(options?["flexisectionblocks"]?.ToObject<FlexiSectionBlocksExtensionOptions>()).
                        UseFlexiAlertBlocks(options?["flexialertblocks"]?.ToObject<FlexiAlertBlocksExtensionOptions>()).
                        UseFlexiCodeBlocks(options?["flexicodeblocks"]?.ToObject<FlexiCodeBlocksExtensionOptions>()).
                        UseFlexiOptionsBlocks();
                } },
                { "commonmark", (MarkdownPipelineBuilder _, JObject __) => { } }
            };

        public static void AssertCompliance(string markdown,
            string expectedHtml,
            string pipelineOptions,
            string extensionOptionsJson = null)
        {
            MarkdownPipeline pipeline = CreatePipeline(pipelineOptions, extensionOptionsJson);
            string result = Markdown.ToHtml(markdown, pipeline);
            result = Compact(result);
            string expectedResult = Compact(expectedHtml);

            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        private static MarkdownPipeline CreatePipeline(string pipelineOptions, string extensionOptionsJson)
        {
            JObject extensionOptions = null;

            if (extensionOptionsJson != null)
            {
                extensionOptions = JObject.Parse(extensionOptionsJson);
            }

            string[] extensions = pipelineOptions.Split('_');

            var builder = new MarkdownPipelineBuilder();

            foreach (string extension in extensions)
            {
                _extensionAdders[extension.ToLower()](builder, extensionOptions);
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
