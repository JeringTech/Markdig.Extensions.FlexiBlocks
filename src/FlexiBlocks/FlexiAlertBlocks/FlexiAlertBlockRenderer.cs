using Markdig.Renderers;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiAlertBlock"/>s as HTML.
    /// </summary>
    public class FlexiAlertBlockRenderer : FlexiBlockRenderer<FlexiAlertBlock>
    {
        /// <summary>
        /// Renders a <see cref="FlexiAlertBlock"/> as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="obj">The <see cref="FlexiAlertBlock"/> to render.</param>
        protected override void WriteFlexiBlock(HtmlRenderer renderer, FlexiAlertBlock obj)
        {
            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteChildren(obj, false);
                return;
            }

            FlexiAlertBlockOptions flexiAlertBlockOptions = obj.FlexiAlertBlockOptions;

            // Add class to attributes
            IDictionary<string, string> attributes = flexiAlertBlockOptions.Attributes;
            if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.Class))
            {
                attributes = new HtmlAttributeDictionary(attributes)
                {
                    { "class", flexiAlertBlockOptions.Class }
                };
            }

            renderer.Write("<div").WriteAttributes(attributes).WriteLine(">");

            // Icon
            if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.IconMarkup))
            {
                renderer.WriteLine(flexiAlertBlockOptions.IconMarkup);
            }

            renderer.Write("<div");

            // Content class
            if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.ContentClass))
            {
                renderer.Write($" class=\"{flexiAlertBlockOptions.ContentClass}\"");
            }

            renderer.
                WriteLine(">").
                WriteChildren(obj, false).
                WriteLine("</div>").
                WriteLine("</div>");
        }
    }
}
