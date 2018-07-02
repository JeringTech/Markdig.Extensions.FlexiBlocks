using FlexiBlocks. Alerts;
using FlexiBlocks.FlexiCode;
using FlexiBlocks.ResponsiveTables;
using FlexiBlocks.Sections;
using Markdig;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace FlexiBlocks.Tests
{
    public static class SpecTestHelper
    {
        private static readonly Dictionary<string, Action<MarkdownPipelineBuilder, JObject>> _extensionAdders =
            new Dictionary<string, Action<MarkdownPipelineBuilder, JObject>>
            {
                { "flexioptions", (MarkdownPipelineBuilder builder, JObject _) => builder.UseJsonOptions() },
                { "flexicode", (MarkdownPipelineBuilder builder, JObject options) => builder.UseFlexiCode(options?["flexicode"]?.ToObject<FlexiCodeExtensionOptions>()) },
                { "flexisections", (MarkdownPipelineBuilder builder, JObject options) => builder.UseSections(options?["flexisections"]?.ToObject<FlexiSectionsExtensionOptions>()) },
                { "flexialerts", (MarkdownPipelineBuilder builder, JObject options) => builder.UseFlexiAlerts(options?["flexialerts"]?.ToObject<FlexiAlertsExtensionOptions>()) },
                { "flexitables", (MarkdownPipelineBuilder builder, JObject options) => builder.UseResponsiveTables(options?["flexitables"]?.ToObject<FlexiTablesExtensionOptions>()) },
                { "pipetables", (MarkdownPipelineBuilder builder, JObject _) => builder.UsePipeTables() },
                { "gridtables", (MarkdownPipelineBuilder builder, JObject _) => builder.UseGridTables() },
                { "all", (MarkdownPipelineBuilder builder, JObject options) => {
                    builder.
                        UseResponsiveTables(options?["flexitables"]?.ToObject<FlexiTablesExtensionOptions>()).
                        UseSections(options?["flexisections"]?.ToObject<FlexiSectionsExtensionOptions>()).
                        UseFlexiAlerts(options?["flexialerts"]?.ToObject<FlexiAlertsExtensionOptions>()).
                        UseFlexiCode(options?["flexicode"]?.ToObject<FlexiCodeExtensionOptions>()).
                        UsePipeTables().
                        UseGridTables().
                        UseJsonOptions();
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

            Assert.Equal(expectedResult, result);
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
