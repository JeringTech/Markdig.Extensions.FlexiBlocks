using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// The default implementation of <see cref="IFlexiSectionHeadingBlockFactory"/>.
    /// </summary>
    public class FlexiSectionHeadingBlockFactory : IFlexiSectionHeadingBlockFactory
    {
        internal const string AUTO_LINKABLE_FLEXI_SECTION_HEADING_BLOCKS = "flexiSectionHeadingBlocks";
        internal const string GENERATED_IDS = "generatedIDs";

        /// <inheritdoc />
        public FlexiSectionHeadingBlock Create(BlockProcessor blockProcessor, IFlexiSectionBlockOptions flexiSectionBlockOptions, BlockParser blockParser)
        {
            // Create
            int lineIndex = blockProcessor.LineIndex;
            int column = blockProcessor.Column;
            ref StringSlice line = ref blockProcessor.Line;
            var flexiSectionHeadingBlock = new FlexiSectionHeadingBlock(blockParser)
            {
                Column = column,
                Span = new SourceSpan(line.Start, line.End),
                Line = lineIndex
            };

            // Add line to block
            flexiSectionHeadingBlock.AppendLine(ref line, column, lineIndex, blockProcessor.CurrentLineStartPosition);

            // ID generation and auto-linking
            SetupIDGenerationAndAutoLinking(flexiSectionHeadingBlock, flexiSectionBlockOptions, blockProcessor);

            return flexiSectionHeadingBlock;
        }

        internal virtual void SetupIDGenerationAndAutoLinking(FlexiSectionHeadingBlock flexiSectionHeadingBlock,
            IFlexiSectionBlockOptions flexiSectionBlockOptions,
            BlockProcessor blockProcessor)
        {
            if (flexiSectionBlockOptions.GenerateID && // Can only auto link to FlexiSectionBlock GenerateID is true
                flexiSectionBlockOptions.AutoLinkable)
            {
                GetOrCreateAutoLinkableFlexiSectionHeadingBlocks(blockProcessor.Document).Add(flexiSectionHeadingBlock);
            }
        }

        internal virtual List<FlexiSectionHeadingBlock> GetOrCreateAutoLinkableFlexiSectionHeadingBlocks(MarkdownDocument markdownDocument)
        {
            if (!(markdownDocument.GetData(AUTO_LINKABLE_FLEXI_SECTION_HEADING_BLOCKS) is List<FlexiSectionHeadingBlock> autoLinkableFlexiSectionHeadingBlocks))
            {
                autoLinkableFlexiSectionHeadingBlocks = new List<FlexiSectionHeadingBlock>();
                markdownDocument.SetData(AUTO_LINKABLE_FLEXI_SECTION_HEADING_BLOCKS, autoLinkableFlexiSectionHeadingBlocks);
                // If user messes with data, handler could be attached multiple times. To avoid issues, handler is written to avoid corrupting 
                // LinkReferenceDefinitions even if called more than once.
                markdownDocument.ProcessInlinesBegin += DocumentOnProcessInlinesBegin;
            }

            return autoLinkableFlexiSectionHeadingBlocks;
        }

        /// <summary>
        /// Inserts <see cref="LinkReferenceDefinition"/>s into the <see cref="MarkdownDocument"/>'s <see cref="LinkReferenceDefinition" />s,
        /// allowing for auto linking to sections via heading content.
        /// </summary>
        internal virtual void DocumentOnProcessInlinesBegin(InlineProcessor inlineProcessor, Inline _)
        {
            Dictionary<string, int> generatedIDs = GetOrCreateGeneratedIDs(inlineProcessor.Document);

            foreach (FlexiSectionHeadingBlock autoLinkableFlexiSectionHeadingBlock in (List<FlexiSectionHeadingBlock>)inlineProcessor.Document.GetData(AUTO_LINKABLE_FLEXI_SECTION_HEADING_BLOCKS)) // TODO may throw if user messes with data
            {
                var headingWriter = new StringWriter();
                var stripRenderer = new HtmlRenderer(headingWriter)
                {
                    EnableHtmlForInline = false,
                    EnableHtmlEscape = false
                };

                // Generate key
                inlineProcessor.ProcessInlineLeaf(autoLinkableFlexiSectionHeadingBlock);
                autoLinkableFlexiSectionHeadingBlock.ProcessInlines = false; // Don't process it again
                stripRenderer.WriteLeafInline(autoLinkableFlexiSectionHeadingBlock);
                string label = headingWriter.ToString();

                // Set ID
                string id = (string.IsNullOrWhiteSpace(label) ? "section" : LinkHelper.UrilizeAsGfm(label));
                while (generatedIDs.TryGetValue(id, out int numDuplicates))
                {
                    generatedIDs[id] = ++numDuplicates;
                    id = $"{id}-{numDuplicates}";
                    label = $"{label} {numDuplicates}";
                }
                generatedIDs.Add(id, 0);
                autoLinkableFlexiSectionHeadingBlock.GeneratedID = id;

                // Avoid overriding existing LinkReferenceDefinitions
                if (!inlineProcessor.Document.TryGetLinkReferenceDefinition(label, out LinkReferenceDefinition linkReferenceDefinition))
                {
                    // A markdown link that uses a link reference definition (https://spec.commonmark.org/0.28/#link-reference-definitions)
                    // has form "[<label>]". Markdig searches for a LinkReferenceDefinition with key <label> to handle such a link.
                    inlineProcessor.Document.SetLinkReferenceDefinition(label, new LinkReferenceDefinition(label, $"#{id}", null)); // No need title since anchor element has text content (label)
                }
            }
        }

        internal virtual Dictionary<string, int> GetOrCreateGeneratedIDs(MarkdownDocument markdownDocument)
        {
            if (!(markdownDocument.GetData(GENERATED_IDS) is Dictionary<string, int> generatedIDs))
            {
                generatedIDs = new Dictionary<string, int>();
                markdownDocument.SetData(GENERATED_IDS, generatedIDs);
            }

            return generatedIDs;
        }
    }
}
