using Markdig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JeremyTCD.Markdig.Extensions.Tests
{
    public class PipelineHelper
    {
        private static Dictionary<string, Action<MarkdownPipelineBuilder>> ExtensionsAdders =
            new Dictionary<string, Action<MarkdownPipelineBuilder>>
            {
                {"sections", (MarkdownPipelineBuilder builder) => builder.UseSections() },
                {"all", (MarkdownPipelineBuilder builder) => builder.UseSections() },
                {"commonmark", (MarkdownPipelineBuilder builder) => { } }
            };

        public static MarkdownPipeline CreatePipeline(string extensionsOption)
        {
            string[] extensions = extensionsOption.Split('_');

            MarkdownPipelineBuilder builder = new MarkdownPipelineBuilder();

            foreach (string extension in extensions)
            {
                ExtensionsAdders[extension.ToLower()](builder);
            }

            return builder.Build();
        }

        public static string Compact(string html)
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
