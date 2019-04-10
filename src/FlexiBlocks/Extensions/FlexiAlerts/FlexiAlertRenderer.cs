using Markdig.Renderers;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// A renderer that renders <see cref="FlexiAlert"/>s as HTML.
    /// </summary>
    public class FlexiAlertRenderer : BlockRenderer<FlexiAlert>
    {
        /// <summary>
        /// Renders a <see cref="FlexiAlert"/> as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="block">The <see cref="FlexiAlert"/> to render.</param>
        protected override void WriteBlock(HtmlRenderer renderer, FlexiAlert block)
        {
            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteChildren(block, false);
                return;
            }

            // If blockName is null render without default classes
            if (block.FlexiBlockOptions.BlockName == null)
            {
                WriteWithoutDefaultClasses(renderer, block);
                return;
            }

            WriteWithDefaultClasses(renderer, block);
        }

        internal virtual void WriteWithoutDefaultClasses(HtmlRenderer htmlRenderer, FlexiAlert flexiAlert)
        {
            // Root element
            htmlRenderer.
                Write("<div").
                WriteAttributes(flexiAlert.FlexiBlockOptions.Attributes).
                WriteLine(">");

            // Icon
            if (flexiAlert.IconHtmlFragment != null)
            {
                htmlRenderer.WriteLine(flexiAlert.IconHtmlFragment);
            }

            // Content
            htmlRenderer.
                WriteLine("<div>").
                WriteChildren(flexiAlert, false).
                WriteLine("</div>\n</div>");
        }

        internal virtual void WriteWithDefaultClasses(HtmlRenderer htmlRenderer, FlexiAlert flexiAlert)
        {
            IFlexiAlertOptions flexiAlertOptions = flexiAlert.FlexiBlockOptions;
            string blockName = flexiAlertOptions.BlockName;

            // Root element
            htmlRenderer.
                Write("<div").
                WriteAttributesExcludingClass(flexiAlertOptions.Attributes).
                Write(" class=\"").
                Write(blockName);

            if (flexiAlertOptions.Type != null)
            {
                htmlRenderer.
                    Write(' ').
                    Write(blockName).
                    Write('_').
                    Write(flexiAlertOptions.Type);
            }

            string classValue = null;
            if (flexiAlertOptions.Attributes?.TryGetValue("class", out classValue) == true)
            {
                htmlRenderer.
                    Write(' ').
                    Write(classValue);
            }

            htmlRenderer.
                WriteLine("\">");

            // Icon
            if (flexiAlert.IconHtmlFragment != null)
            {
                htmlRenderer.WriteHtmlFragmentWithClassAttribute(flexiAlert.IconHtmlFragment, blockName, "__icon");
            }

            // Content
            htmlRenderer.
                Write("<div class=\"").
                Write(blockName).
                WriteLine("__content\">").
                WriteChildren(flexiAlert, false).
                WriteLine("</div>\n</div>");
        }
    }
}
