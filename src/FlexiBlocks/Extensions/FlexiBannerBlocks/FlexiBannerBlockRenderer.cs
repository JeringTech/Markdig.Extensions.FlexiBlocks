using Markdig.Renderers;
using Markdig.Syntax;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiBannerBlock"/>s as HTML.
    /// </summary>
    public class FlexiBannerBlockRenderer : BlockRenderer<FlexiBannerBlock>
    {
        /// <summary>
        /// Renders a <see cref="FlexiBannerBlock"/> as HTML.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="block">The <see cref="FlexiBannerBlock"/> to render.</param>
        protected override void WriteBlock(HtmlRenderer htmlRenderer, FlexiBannerBlock block)
        {
            if (!htmlRenderer.EnableHtmlForBlock)
            {
                htmlRenderer.
                    WriteLeafInline(block[0] as LeafBlock).
                    EnsureLine().
                    WriteLeafInline(block[1] as LeafBlock).
                    EnsureLine();
                return;
            }

            string blockName = block.BlockName;
            string logoIcon = block.LogoIcon;
            bool renderLogoIcon = !string.IsNullOrWhiteSpace(logoIcon);
            string backgroundIcon = block.BackgroundIcon;
            bool renderBackgroundIcon = !string.IsNullOrWhiteSpace(backgroundIcon);
            ReadOnlyDictionary<string, string> attributes = block.Attributes;

            // Root element
            htmlRenderer.
                Write("<div class=\"").
                Write(blockName).
                WriteHasFeatureClass(renderLogoIcon, blockName, "logo-icon").
                WriteHasFeatureClass(renderBackgroundIcon, blockName, "background-icon").
                WriteAttributeValue(attributes, "class").
                Write('"').
                WriteAttributesExcept(attributes, "class").
                WriteLine(">");

            // Background
            htmlRenderer.
                WriteHtmlFragment(renderBackgroundIcon, backgroundIcon, blockName, "background-icon").
                EnsureLine();

            // Logo
            htmlRenderer.
                WriteHtmlFragment(renderLogoIcon, logoIcon, blockName, "logo-icon").
                EnsureLine();

            // Title
            htmlRenderer.
                WriteStartTag("h1", blockName, "title").
                WriteLeafInline(block[0] as LeafBlock).
                WriteEndTagLine("h1");

            // Blurb
            htmlRenderer.
                WriteStartTag("p", blockName, "blurb").
                WriteLeafInline(block[1] as LeafBlock).
                WriteEndTagLine("p").
                WriteEndTagLine("div");
        }
    }
}
