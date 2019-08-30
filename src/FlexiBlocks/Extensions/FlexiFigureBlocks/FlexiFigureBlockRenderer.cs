using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiFigureBlock"/>s as HTML.
    /// </summary>
    public class FlexiFigureBlockRenderer : BlockRenderer<FlexiFigureBlock>
    {
        /// <summary>
        /// Renders a <see cref="FlexiFigureBlock"/> as HTML.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="block">The <see cref="FlexiFigureBlock"/> to render.</param>
        protected override void WriteBlock(HtmlRenderer htmlRenderer, FlexiFigureBlock block)
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
            ReadOnlyDictionary<string, string> attributes = block.Attributes;
            string name = block.Name;
            bool renderName = block.RenderName && !string.IsNullOrWhiteSpace(name);
            string id = block.ID;

            // Root element
            htmlRenderer.
                Write("<figure").
                Write(" class=\"").
                Write(blockName).
                WriteHasFeatureClass(renderName, blockName, "name").
                WriteAttributeValue(attributes, "class").
                Write("\"").
                WriteAttribute(!string.IsNullOrWhiteSpace(id), "id", id).
                WriteAttributesExcept(attributes, "class", "id").
                WriteLine(">");

            // Content
            htmlRenderer.
                WriteStartTagLine("div", blockName, "content").
                WriteChildren(block[0] as ContainerBlock, false).
                WriteEndTagLine("div");

            // Caption
            htmlRenderer.
                WriteStartTag("figcaption", blockName, "caption").
                WriteStartTag("span", blockName, "name").
                Write(renderName, name, ". ").
                WriteEndTag("span").
                WriteLeafInline(block[1] as LeafBlock).
                WriteEndTagLine("figcaption").
                WriteEndTagLine("figure");
        }
    }
}
