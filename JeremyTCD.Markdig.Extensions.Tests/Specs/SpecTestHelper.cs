using JeremyTCD.Markdig.Extensions. Alerts;
using JeremyTCD.Markdig.Extensions.Sections;
using Markdig;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests
{
    public static class SpecTestHelper
    {
        private static readonly Dictionary<string, Action<MarkdownPipelineBuilder, JObject>> _extensionAdders =
            new Dictionary<string, Action<MarkdownPipelineBuilder, JObject>>
            {
                { "genericattributes", (MarkdownPipelineBuilder builder, JObject _) => builder.UseGenericAttributes() },
                { "jsonoptions", (MarkdownPipelineBuilder builder, JObject _) => builder.UseJsonOptions() },
                { "sections", (MarkdownPipelineBuilder builder, JObject options) => builder.UseSections(options?["sections"]?.ToObject<SectionsExtensionOptions>()) },
                { "alerts", (MarkdownPipelineBuilder builder, JObject options) => builder.UseAlerts(options?["alerts"]?.ToObject<AlertsExtensionOptions>()) },
                { "all", (MarkdownPipelineBuilder builder, JObject options) => {
                    builder.
                        UseSections(options?["sections"]?.ToObject<SectionsExtensionOptions>()).
                        UseAlerts(options?["alerts"]?.ToObject<AlertsExtensionOptions>()).
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
