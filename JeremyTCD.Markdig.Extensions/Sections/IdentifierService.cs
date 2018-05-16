using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.IO;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class IdentifierService
    {
        public const string SECTION_IDS_KEY = "SectionIDs";
        private readonly HtmlRenderer _stripRenderer;
        private readonly StringWriter _headingWriter;

        public IdentifierService()
        {
            _headingWriter = new StringWriter();
            _stripRenderer = new HtmlRenderer(_headingWriter)
            {
                // Set to false both to avoid having any HTML tags in the output
                EnableHtmlForInline = false,
                EnableHtmlEscape = false
            };
        }

        /// <summary>
        /// Register delegate that generates IDs.
        /// </summary>
        /// <param name="headingBlock"></param>
        public void SetupIdentifierGeneration(HeadingBlock headingBlock)
        {
            headingBlock.ProcessInlinesEnd += HeadingBlockOnProcessInlinesEnd;
        }

        /// <summary>
        /// Creates and assigns IDs to sections.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="inline"></param>
        /// <exception cref="InvalidOperationException">Throw if <see cref="HeadingBlock"/>'s parent is not a <see cref="SectionBlock"/>.</exception>
        internal void HeadingBlockOnProcessInlinesEnd(InlineProcessor processor, Inline inline)
        {
            // Retrieve or create document level HashSet of ids - use to prevent duplicate ids
            if (!(processor.Document.GetData(SECTION_IDS_KEY) is Dictionary<string, int> identifiers))
            {
                identifiers = new Dictionary<string, int>();
                processor.Document.SetData(SECTION_IDS_KEY, identifiers);
            }

            var headingBlock = (HeadingBlock)processor.Block;

            if (!(headingBlock.Parent is SectionBlock sectionBlock))
            {
                throw new InvalidOperationException("HeadingBlock's parent must be a SectionBlock.");
            }

            // If section block already has an id, return
            Dictionary<string, string> attributes = sectionBlock.SectionBlockOptions.Attributes;
            if (attributes.ContainsKey("id"))
            {
                return;
            }

            // Get heading text
            _stripRenderer.Render(headingBlock.Inline);
            string headingText = _headingWriter.ToString();
            _headingWriter.GetStringBuilder().Length = 0;

            // Convert heading text to kebab case
            string uri = LinkHelper.UrilizeAsGfm(headingText);

            // If the heading is empty, use the word "section" instead
            string headingId = string.IsNullOrWhiteSpace(uri) ? "section" : uri;

            // Check for duplicate ids and append integer if necessary
            if (identifiers.TryGetValue(headingId, out int numDuplicates))
            {
                identifiers[headingId] = ++numDuplicates;
                headingId = $"{headingId}-{numDuplicates}";
            }
            else
            {
                identifiers.Add(headingId, 0);
            }

            attributes["id"] = headingId;
        }
    }
}
