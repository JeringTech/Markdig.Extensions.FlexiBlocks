using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace FlexiBlocks.Alerts
{
    public class FlexiAlertBlockRenderer : HtmlObjectRenderer<FlexiAlertBlock>
    {
        protected override void Write(HtmlRenderer renderer, FlexiAlertBlock obj)
        {
            FlexiAlertBlockOptions flexiAlertBlockOptions = obj.FlexiAlertBlockOptions;

            renderer.EnsureLine();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteHtmlAttributeDictionary(flexiAlertBlockOptions.Attributes).WriteLine(">");

                if (flexiAlertBlockOptions.IconMarkup != null)
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
