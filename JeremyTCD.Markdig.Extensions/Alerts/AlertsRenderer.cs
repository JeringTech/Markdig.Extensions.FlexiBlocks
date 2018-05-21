using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsRenderer : HtmlObjectRenderer<AlertBlock>
    {
        protected override void Write(HtmlRenderer renderer, AlertBlock obj)
        {
            renderer.EnsureLine();
            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteCustomAttributes(obj.AlertBlockOptions.Attributes).WriteLine(">");
            }
            bool savedImplicitParagraph = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;
            renderer.WriteChildren(obj);
            renderer.ImplicitParagraph = savedImplicitParagraph;
            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine("</div>");
            }
        }
    }
}
