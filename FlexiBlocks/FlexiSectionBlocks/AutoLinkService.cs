using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;

namespace FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// Logic for generating link references.
    /// </summary>
    public class AutoLinkService
    {
        public const string AUTO_LINKS_KEY = "AutoLinks";

        /// <summary>
        /// Create a <see cref="SectionLinkReferenceDefinition"/> from a <see cref="FlexiSectionBlock"/> and store it in the MarkdownDocument.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="sectionBlock"></param>
        /// <param name="headingBlock"></param>
        public virtual void SetupAutoLink(BlockProcessor processor, FlexiSectionBlock sectionBlock, HeadingBlock headingBlock)
        {
            string headingBlockText = headingBlock.Lines.Lines[0].ToString();

            var sectionLinkReferenceDefinition = new SectionLinkReferenceDefinition()
            {
                SectionBlock = sectionBlock,
                CreateLinkInline = CreateLinkInline
            };

            MarkdownDocument document = processor.Document;
            if (!(document.GetData(AUTO_LINKS_KEY) is Dictionary<string, SectionLinkReferenceDefinition> sectionLinkReferenceDefinitions))
            {
                sectionLinkReferenceDefinitions = new Dictionary<string, SectionLinkReferenceDefinition>();
                document.SetData(AUTO_LINKS_KEY, sectionLinkReferenceDefinitions);
                document.ProcessInlinesBegin += DocumentOnProcessInlinesBegin;
            }
            sectionLinkReferenceDefinitions[headingBlockText] = sectionLinkReferenceDefinition;
        }

        /// <summary>
        /// Inserts <see cref="SectionLinkReferenceDefinition"/>s into the <see cref="MarkdownDocument"/>'s <see cref="LinkReferenceDefinition" />s. 
        /// This allows for auto linking to sections via header text.
        /// Logic in this method should be called just before inline processing begins to avoid overriding vanilla <see cref="LinkReferenceDefinition"/>s.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="inline"></param>
        internal void DocumentOnProcessInlinesBegin(InlineProcessor processor, Inline inline)
        {
            // Remove callback
            MarkdownDocument document = processor.Document;
            document.ProcessInlinesBegin -= DocumentOnProcessInlinesBegin;

            // Get SectionLinkReferenceDefinition map
            var sectionLinkReferenceDefinitions = (Dictionary<string, SectionLinkReferenceDefinition>)document.GetData(AUTO_LINKS_KEY);
            foreach (var keyPair in sectionLinkReferenceDefinitions)
            {
                // Avoid overriding existing LinkReferenceDefinitions
                if (!document.TryGetLinkReferenceDefinition(keyPair.Key, out LinkReferenceDefinition linkReferenceDefinition))
                {
                    document.SetLinkReferenceDefinition(keyPair.Key, keyPair.Value);
                }
            }

            // Once we are done, we don't need to keep the intermediate dictionary arround
            document.RemoveData(AUTO_LINKS_KEY);
        }

        /// <summary>
        /// Creates a <see cref="LinkInline"/> from an <see cref="SectionLinkReferenceDefinition"/>.
        /// </summary>
        /// <param name="inlineState"></param>
        /// <param name="linkReferenceDefinition"></param>
        /// <param name="child"></param>
        internal Inline CreateLinkInline(InlineProcessor inlineState, LinkReferenceDefinition linkReferenceDefinition, Inline child)
        {
            var sectionLinkReferenceDefinition = (SectionLinkReferenceDefinition)linkReferenceDefinition;
            return new LinkInline()
            {
                // Use GetDynamicUrl to allow late binding of the Url, since a link may occur before the heading is declared and
                // the inlines of the heading are actually processed by HeadingBlockProcessInlinesEndCallback()
                GetDynamicUrl = () => HtmlHelper.Unescape("#" + sectionLinkReferenceDefinition.SectionBlock.SectionBlockOptions.Attributes["id"]),
                Title = HtmlHelper.Unescape(linkReferenceDefinition.Title)
            };
        }
    }
}
