using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions
{
    public class SectionRenderer : HtmlObjectRenderer<SectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, SectionBlock obj)
        {
            string elementName = obj.HeadingWrapperElement.ToString().ToLower();

            renderer.EnsureLine();
            renderer.
                Write("<").
                Write(elementName).
                WriteAttributes(obj).
                WriteLine(">");
            renderer.WriteChildren(obj);
            renderer.
                Write("</").
                Write(elementName).
                WriteLine(">");
        }
    }
}
