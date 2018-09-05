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
            FlexiAlertBlockOptions flexiAlertBlockOptions = obj.FlexiAlertBlockOptions;

            renderer.EnsureLine();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteHtmlAttributeDictionary(flexiAlertBlockOptions.Attributes).WriteLine(">");

                if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.IconMarkup))
                {
                    renderer.WriteLine(flexiAlertBlockOptions.IconMarkup);
                }

                renderer.Write("<div");
                if (!string.IsNullOrWhiteSpace(flexiAlertBlockOptions.ContentClassName))
                {
                    renderer.Write($" class=\"{flexiAlertBlockOptions.ContentClassName}\"");
                }
                renderer.WriteLine(">");
            }

            renderer.WriteChildren(obj, false);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.
                    WriteLine("</div>").
                    WriteLine("</div>");
            }
        }
    }
}
