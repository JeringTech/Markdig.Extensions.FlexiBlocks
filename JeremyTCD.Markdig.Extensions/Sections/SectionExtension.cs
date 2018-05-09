using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JeremyTCD.Markdig.Extensions
{
    /// <summary>
    /// Wraps implicit sections in <section> elements, as recommended by - https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections.
    /// 
    /// Includes similar functionality to that provided by AutoIdentifierExtension.
    /// </summary>
    public class SectionExtension : IMarkdownExtension
    {
        private const string SECTION_IDS_KEY = "SectionIDs";
        private SectionOptions _options;
        private readonly HtmlRenderer _stripRenderer;
        private readonly StringWriter _headingWriter;

        public SectionExtension(SectionOptions options)
        {
            _options = options ?? new SectionOptions();
            _headingWriter = new StringWriter();
            // Use internally a HtmlRenderer to strip links from a heading
            _stripRenderer = new HtmlRenderer(_headingWriter)
            {
                // Set to false both to avoid having any HTML tags in the output
                EnableHtmlForInline = false,
                EnableHtmlEscape = false
            };
        }

        public void Setup(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.BlockParsers.Contains<SectionParser>())
            {
                HeadingBlockParser headingBlockParser = pipelineBuilder.BlockParsers.Find<HeadingBlockParser>();
                if (headingBlockParser != null)
                {
                    pipelineBuilder.BlockParsers.Remove(headingBlockParser);
                }

                var sectionParser = new SectionParser();
                sectionParser.Closed += SectionParserClosedCallback;

                // Insert the parser before any other parsers
                pipelineBuilder.BlockParsers.Insert(0, sectionParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<SectionRenderer>())
            {
                // Must be inserted before CodeBlockRenderer
                htmlRenderer.ObjectRenderers.Insert(0, new SectionRenderer());
            }
        }

        private void SectionParserClosedCallback(BlockProcessor processor, Block block)
        {
            HeadingBlock headingBlock = (block as SectionBlock)?.Descendants<HeadingBlock>().FirstOrDefault();

            if (headingBlock == null)
            {
                throw new InvalidOperationException("A section block does not contain a heading block.");
            }

            headingBlock.ProcessInlinesEnd += HeadingBlockProcessInlinesEndCallback;
        }

        private void HeadingBlockProcessInlinesEndCallback(InlineProcessor processor, Inline inline)
        {
            // Retrieve or create document level HashSet of ids - use to prevent duplicate ids
            var identifiers = processor.Document.GetData(SECTION_IDS_KEY) as Dictionary<string, int>;
            if (identifiers == null)
            {
                identifiers = new Dictionary<string, int>();
                processor.Document.SetData(SECTION_IDS_KEY, identifiers);
            }

            var headingBlock = (HeadingBlock)processor.Block;
            var sectionBlock = (SectionBlock)headingBlock.Parent;

            // If section block already has an id, return
            HtmlAttributes attributes = sectionBlock.GetAttributes();
            if (!string.IsNullOrWhiteSpace(attributes.Id))
            {
                return;
            }

            // Get heading text
            _stripRenderer.Render(headingBlock.Inline);
            string headingText = _headingWriter.ToString();
            _headingWriter.GetStringBuilder().Length = 0;

            // Urilize the heading text
            string uri = LinkHelper.UrilizeAsGfm(headingText);

            // If the heading is empty, use the word "section" instead
            string headingId = string.IsNullOrWhiteSpace(uri) ? "section" : uri;

            // Check for duplicate ids and append integer if necessary
            if (identifiers.TryGetValue(headingId, out int duplicates))
            {
                identifiers[headingId] = ++duplicates;
                headingId = $"{headingId}-{duplicates}";
            }
            else
            {
                identifiers.Add(headingId, 0);
            }

            attributes.Id = headingId;
        }
    }
}
