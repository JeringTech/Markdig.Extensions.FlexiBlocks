﻿using Markdig.Renderers;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiAlertBlock"/>s as HTML.
    /// </summary>
    public class FlexiAlertBlockRenderer : BlockRenderer<FlexiAlertBlock>
    {
        /// <summary>
        /// Renders a <see cref="FlexiAlertBlock"/> as HTML.
        /// </summary>
        /// <param name="htmlRenderer">The renderer to write to.</param>
        /// <param name="block">The <see cref="FlexiAlertBlock"/> to render.</param>
        protected override void WriteBlock(HtmlRenderer htmlRenderer, FlexiAlertBlock block)
        {
            if (!htmlRenderer.EnableHtmlForBlock)
            {
                htmlRenderer.WriteChildren(block, false);
                return;
            }

            string blockName = block.BlockName;
            string icon = block.Icon;
            bool renderIcon = !string.IsNullOrWhiteSpace(icon);
            ReadOnlyDictionary<string, string> attributes = block.Attributes;

            // Root element
            htmlRenderer.
                Write("<div class=\"").
                Write(blockName).
                WriteBlockKeyValueModifierClass(blockName, "type", block.Type).
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
                WriteStartTagLine("div", blockName, "content").
                WriteChildren(block, false). // Default blockquotes require explicit paragraphs, we copy that here
                WriteEndTagLine("div").
                WriteEndTagLine("div");
        }
    }
}
