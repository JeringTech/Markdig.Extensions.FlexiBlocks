using Markdig.Renderers;

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
        public override void WriteFlexiBlock(HtmlRenderer renderer, FlexiAlertBlock obj)
        {
            renderer.EnsureLine();

            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteChildren(obj, false);
                return;
            }

            FlexiAlertBlockOptions flexiAlertBlockOptions = obj.FlexiAlertBlockOptions;

            renderer.Write("<div").WriteHtmlAttributeDictionary(flexiAlertBlockOptions.Attributes).WriteLine(">");
         
            // Icon
            if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.IconMarkup))
            {
                renderer.WriteLine(flexiAlertBlockOptions.IconMarkup);
            }

            renderer.Write("<div");

            // Content class
            if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.ContentClassName))
            {
                renderer.Write($" class=\"{flexiAlertBlockOptions.ContentClassName}\"");
            }

            renderer.
                WriteLine(">").
                WriteChildren(obj, false).
                WriteLine("</div>").
                WriteLine("</div>");
        }
    }
}
