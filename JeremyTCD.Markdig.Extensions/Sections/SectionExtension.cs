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
                sectionParser.Closed += SectionParserOnClosed;

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

        /// <summary>
        /// Sets up callbacks that handle section IDs and auto links.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        private void SectionParserOnClosed(BlockProcessor processor, Block block)
        {
            SectionBlock sectionBlock = (SectionBlock)block;
            HeadingBlock headingBlock = (HeadingBlock)sectionBlock.FirstOrDefault(child => child is HeadingBlock);

            if (headingBlock == null)
            {
                throw new InvalidOperationException("A section block must contain a heading block.");
            }

            // Assign delegate to headingBlock.ProcessInlinesEnd - sectionBlock is a container block, so sectionBlock.ProcessInlinesEnd does 
            // not receive a reference to the SectionBlock that owns the delegate
            headingBlock.ProcessInlinesEnd += HeadingBlockOnProcessInlinesEnd;

            // If auto link is enabled, create and store a SectionLinkReferenceDefinition in the MarkdownDocument
            if (_options.AutoLink)
            {
                string headingBlockText = headingBlock.Lines.Lines[0].ToString();

                var sectionLinkReferenceDefinition = new SectionLinkReferenceDefinition()
                {
                    SectionBlock = sectionBlock,
                    CreateLinkInline = CreateLinkInlineForHeading
                };

                MarkdownDocument document = processor.Document;
                if (!(document.GetData(this) is Dictionary<string, SectionLinkReferenceDefinition> sectionLinkReferenceDefinitions))
                {
                    sectionLinkReferenceDefinitions = new Dictionary<string, SectionLinkReferenceDefinition>();
                    document.SetData(this, sectionLinkReferenceDefinitions);
                    document.ProcessInlinesBegin += DocumentOnProcessInlinesBegin;
                }
                sectionLinkReferenceDefinitions[headingBlockText] = sectionLinkReferenceDefinition;
            }
        }

        /// <summary>
        /// Inserts <see cref="SectionLinkReferenceDefinition"/>s into the <see cref="MarkdownDocument"/>'s <see cref="LinkReferenceDefinition" />s. 
        /// This allows for auto linking to sections via header text.
        /// Logic in this method should be called just before inline processing begins to avoid overriding vanilla <see cref="LinkReferenceDefinition"/>s.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="inline"></param>
        private void DocumentOnProcessInlinesBegin(InlineProcessor processor, Inline inline)
        {
            // Remove callback
            MarkdownDocument document = processor.Document;
            document.ProcessInlinesBegin -= DocumentOnProcessInlinesBegin;

            // Get SectionLinkReferenceDefinition map
            var sectionLinkReferenceDefinitions = (Dictionary<string, SectionLinkReferenceDefinition>)document.GetData(this);
            foreach (var keyPair in sectionLinkReferenceDefinitions)
            {
                // Avoid overriding existing LinkReferenceDefinitions
                if (!document.TryGetLinkReferenceDefinition(keyPair.Key, out LinkReferenceDefinition linkReferenceDefinition))
                {
                    document.SetLinkReferenceDefinition(keyPair.Key, keyPair.Value);
                }
            }

            // Once we are done, we don't need to keep the intermediate dictionary arround
            document.RemoveData(this);
        }

        /// <summary>
        /// Creates a <see cref="LinkInline"/> from an <see cref="SectionLinkReferenceDefinition"/>.
        /// </summary>
        /// <param name="inlineState"></param>
        /// <param name="linkReferenceDefinition"></param>
        /// <param name="child"></param>
        private Inline CreateLinkInlineForHeading(InlineProcessor inlineState, LinkReferenceDefinition linkReferenceDefinition, Inline child)
        {
            var sectionLinkReferenceDefinition = (SectionLinkReferenceDefinition)linkReferenceDefinition;
            return new LinkInline()
            {
                // Use GetDynamicUrl to allow late binding of the Url, since a link may occur before the heading is declared and
                // the inlines of the heading are actually processed by HeadingBlockProcessInlinesEndCallback()
                GetDynamicUrl = () => HtmlHelper.Unescape("#" + sectionLinkReferenceDefinition.SectionBlock.GetAttributes().Id),
                Title = HtmlHelper.Unescape(linkReferenceDefinition.Title),
            };
        }

        /// <summary>
        /// Creates and assigns section IDs.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="inline"></param>
        private void HeadingBlockOnProcessInlinesEnd(InlineProcessor processor, Inline inline)
        {
            // Retrieve or create document level HashSet of ids - use to prevent duplicate ids
            if (!(processor.Document.GetData(SECTION_IDS_KEY) is Dictionary<string, int> identifiers))
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
