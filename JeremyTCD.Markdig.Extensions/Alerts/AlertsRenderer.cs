using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsRenderer : HtmlObjectRenderer<AlertBlock>
    {
        protected override void Write(HtmlRenderer renderer, AlertBlock obj)
        {
            AlertBlockOptions alertBlockOptions = obj.AlertBlockOptions;

            renderer.EnsureLine();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteCustomAttributes(alertBlockOptions.Attributes).WriteLine(">");

                if (alertBlockOptions.IconMarkup != null)
                {
                    renderer.WriteLine(alertBlockOptions.IconMarkup);
                    renderer.WriteLine("<div class=\"alert-content\">");
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
