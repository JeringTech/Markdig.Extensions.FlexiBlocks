using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiCardsBlock"/>s as HTML.
    /// </summary>
    public class FlexiCardsBlockRenderer : BlockRenderer<FlexiCardsBlock>
    {
        private readonly string[] _sizes = new string[] { "small", "medium" };

        /// <summary>
        /// Renders a <see cref="FlexiCardsBlock"/> as HTML.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="block">The <see cref="FlexiCardsBlock"/> to render.</param>
        protected override void WriteBlock(HtmlRenderer htmlRenderer, FlexiCardsBlock block)
        {
            int numCardBlocks = block.Count;
            if (!htmlRenderer.EnableHtmlForBlock)
            {
                for (int i = 0; i < numCardBlocks; i++)
                {
                    var flexiCardBlock = block[i] as FlexiCardBlock;
                    htmlRenderer.
                        WriteLeafInline(flexiCardBlock[0] as LeafBlock).
                        EnsureLine().
                        WriteChildren(flexiCardBlock[1] as ContainerBlock, false).
                        EnsureLine().
                        WriteLeafInline(flexiCardBlock[2] as LeafBlock).
                        EnsureLine();
                }

                return;
            }

            string blockName = block.BlockName;
            ReadOnlyDictionary<string, string> attributes = block.Attributes;

            // Root element
            htmlRenderer.
                Write("<div class=\"").
                Write(blockName).
                WriteBlockKeyValueModifierClass(blockName, "size", _sizes[(int)block.CardSize]).
                WriteAttributeValue(attributes, "class").
                Write('"').
                WriteAttributesExcept(attributes, "class").
                WriteLine(">");

            // Cards
            for (int i = 0; i < numCardBlocks; i++)
            {
                WriteCard(htmlRenderer, block[i] as FlexiCardBlock, blockName);
            }
            htmlRenderer.
                WriteEndTagLine("div");
        }

        internal virtual void WriteCard(HtmlRenderer htmlRenderer, FlexiCardBlock cardBlock, string blockName)
        {
            ReadOnlyDictionary<string, string> attributes = cardBlock.Attributes;
            string url = cardBlock.Url;
            string backgroundIcon = cardBlock.BackgroundIcon;
            bool isLink = !string.IsNullOrWhiteSpace(url),
                 hasBackgroundIcon = !string.IsNullOrWhiteSpace(backgroundIcon);
            string cardTagName = isLink ? "a" : "div";

            // Root element
            htmlRenderer.
                Write("<").
                Write(cardTagName).
                Write(" class=\"").
                WriteElementClass(blockName, "card").
                WriteIsTypeClass(isLink, blockName, "card", "link").
                WriteHasFeatureClass(hasBackgroundIcon, blockName, "card", "background-icon").
                WriteAttributeValue(attributes, "class").
                Write('"').
                WriteEscapedUrlAttribute(isLink, "href", url).
                WriteAttributesExcept(attributes, "class", "href").
                WriteLine(">");

            // Background
            htmlRenderer.
                WriteHtmlFragmentLine(hasBackgroundIcon, backgroundIcon, blockName, "card-background-icon");

            // Title
            htmlRenderer.
                WriteStartTag("p", blockName, "card-title").
                WriteLeafInline(cardBlock[0] as LeafBlock).
                WriteEndTagLine("p");

            // Content
            htmlRenderer.
                WriteStartTagLine("div", blockName, "card-content").
                WriteChildren(cardBlock[1] as ContainerBlock, false).
                WriteEndTagLine("div");

            // Footnote
            htmlRenderer.
                WriteStartTag("p", blockName, "card-footnote").
                WriteLeafInline(cardBlock[2] as LeafBlock).
                WriteEndTagLine("p").
                WriteEndTagLine(cardTagName);
        }
    }
}
