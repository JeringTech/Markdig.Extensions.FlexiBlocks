using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions
{
    public class SectionRenderer : HtmlObjectRenderer<SectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, SectionBlock obj)
        {
            renderer.EnsureLine();
            renderer.Write("<section").WriteAttributes(obj).Write(">");
            renderer.EnsureLine();
            renderer.WriteChildren(obj);
            renderer.WriteLine("</section>");
        }
    }
}
