using Markdig.Renderers;
using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiSectionBlock"/>s as HTML.
    /// </summary>
    public class FlexiSectionBlockRenderer : FlexiBlockRenderer<FlexiSectionBlock>
    {
        /// <summary>
        /// Renders a <see cref="FlexiSectionBlock"/> as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="obj">The <see cref="FlexiSectionBlock"/> to render.</param>
        protected override void WriteFlexiBlock(HtmlRenderer renderer, FlexiSectionBlock obj)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            renderer.EnsureLine();

            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteChildren(obj, false);
                return;
            }

            FlexiSectionBlockOptions flexiSectionBlockOptions = obj.FlexiSectionBlockOptions;

            // Add class to attributes
            IDictionary<string, string> attributes = new HtmlAttributeDictionary(flexiSectionBlockOptions.Attributes);
            if (!string.IsNullOrWhiteSpace(flexiSectionBlockOptions.Class))
            {
                attributes.Add("class", flexiSectionBlockOptions.Class);
            }

            // Add id to attributes
            if (!string.IsNullOrWhiteSpace(obj.ID))
            {
                attributes.Add("id", obj.ID);
            }

            string elementName = flexiSectionBlockOptions.Element.ToString().ToLower();
            renderer.
                Write("<").
                Write(elementName).
                WriteAttributes(attributes).
                WriteLine(">").
                WriteLine("<header>").
                Write($"<h{obj.Level}>").
                Write(obj.HeaderContent).
                WriteLine($"</h{obj.Level}>");

            // Link icon
            if (!string.IsNullOrWhiteSpace(flexiSectionBlockOptions.LinkIconMarkup))
            {
                renderer.WriteLine(flexiSectionBlockOptions.LinkIconMarkup);
            }

            renderer.
                WriteLine("</header>").
                WriteChildren(obj, false).
                EnsureLine().
                Write("</").
                Write(elementName).
                WriteLine(">");
        }
    }
}
