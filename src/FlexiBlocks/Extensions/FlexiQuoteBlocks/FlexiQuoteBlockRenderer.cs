using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiQuoteBlock"/>s as HTML.
    /// </summary>
    public class FlexiQuoteBlockRenderer : BlockRenderer<FlexiQuoteBlock>
    {
        /// <summary>
        /// Renders a <see cref="FlexiQuoteBlock"/> as HTML.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="block">The <see cref="FlexiQuoteBlock"/> to render.</param>
        protected override void WriteBlock(HtmlRenderer htmlRenderer, FlexiQuoteBlock block)
        {
            if (!htmlRenderer.EnableHtmlForBlock)
            {
                htmlRenderer.
                    WriteChildren(block[0] as ContainerBlock, false).
                    EnsureLine().
                    WriteLeafInline(block[1] as LeafBlock).
                    EnsureLine();

                return;
            }

            string blockName = block.BlockName;
            string icon = block.Icon;
            bool renderIcon = !string.IsNullOrWhiteSpace(icon);
            string citeUrl = block.CiteUrl;
            ReadOnlyDictionary<string, string> attributes = block.Attributes;

            // Root element
            htmlRenderer.
                Write("<div class=\"").
                Write(blockName).
                WriteHasFeatureClass(renderIcon, blockName, "icon").
                WriteAttributeValue(attributes, "class").
                Write('"').
                WriteAttributesExcept(attributes, "class").
                WriteLine(">");

            // Icon
            htmlRenderer.
                WriteHtmlFragment(renderIcon, icon, blockName, "icon").
                EnsureLine();

            // Content
            htmlRenderer.
                WriteStartTagLine("div", blockName, "content");

            // Blockquote
            htmlRenderer.
                Write("<blockquote class=\"").
                WriteElementClass(blockName, "blockquote").
                Write('"').
                WriteAttribute(!string.IsNullOrWhiteSpace(citeUrl), "cite", citeUrl).
                WriteLine(">").
                WriteChildren(block[0] as ContainerBlock, false).
                WriteEndTagLine("blockquote");

            // Citation
            htmlRenderer.
                WriteStartTag("p", blockName, "citation").
                Write("— ").
                WriteLeafInline(block[1] as LeafBlock, true).
                WriteEndTagLine("p").
                WriteEndTagLine("div").
                WriteEndTagLine("div");
        }
    }
}
