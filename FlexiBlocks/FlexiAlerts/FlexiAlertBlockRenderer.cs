using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace FlexiBlocks.Alerts
{
    public class FlexiAlertBlockRenderer : HtmlObjectRenderer<FlexiAlertBlock>
    {
        protected override void Write(HtmlRenderer renderer, FlexiAlertBlock obj)
        {
            FlexiAlertBlockOptions alertBlockOptions = obj.AlertBlockOptions;

            renderer.EnsureLine();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteHtmlAttributeDictionary(alertBlockOptions.Attributes).WriteLine(">");

                if (alertBlockOptions.IconMarkup != null)
                {
                    renderer.WriteLine(alertBlockOptions.IconMarkup);
                    renderer.Write("<div");
                    if (!string.IsNullOrWhiteSpace(alertBlockOptions.ContentClassName))
                    {
                        renderer.Write($" class=\"{alertBlockOptions.ContentClassName}\"");
                    }
                    renderer.WriteLine(">");
                }
            }

            renderer.WriteChildren(obj, false);

            if (renderer.EnableHtmlForBlock)
            {
                if (alertBlockOptions.IconMarkup != null)
                {
                    renderer.WriteLine("</div>");
                }

                renderer.WriteLine("</div>");
            }
        }
    }
}
