using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;

namespace JeremyTCD.Markdig.Extensions
{
    public static class IdentifierGenerationUtils
    {
        private const string SECTION_IDS_KEY = "SectionIDs";
        private static readonly HtmlRenderer _stripRenderer;
        private static readonly StringWriter _headingWriter;

        static IdentifierGenerationUtils()
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
        /// Register delegate that generates identifier
        /// </summary>
        /// <param name="headingBlock"></param>
        public static void SetupIdentifierGeneration(HeadingBlock headingBlock)
        {
            headingBlock.ProcessInlinesEnd += HeadingBlockOnProcessInlinesEnd;
        }

        /// <summary>
        /// Creates and assigns section IDs.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="inline"></param>
        internal static void HeadingBlockOnProcessInlinesEnd(InlineProcessor processor, Inline inline)
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
            if (identifiers.TryGetValue(headingId, out int duplicates))
            {
                identifiers[headingId] = ++duplicates;
                headingId = $"{headingId}-{duplicates}";
            }
            else
            {
                identifiers.Add(headingId, 0);
            }

            attributes["id"] = headingId;
        }
    }
}
