using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockRenderer : HtmlObjectRenderer<SectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, SectionBlock obj)
        {
            SectionBlockOptions sectionBlockOptions = obj.SectionBlockOptions;

            renderer.EnsureLine();

            // Undefined or None
            if (sectionBlockOptions.WrapperElement.CompareTo(SectioningContentElement.None) <= 0)
            {
                renderer.WriteChildren(obj);
                return;
            }

            string elementName = sectionBlockOptions.WrapperElement.ToString().ToLower();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.
                    Write("<").
                    Write(elementName).
                    WriteAttributeMap(sectionBlockOptions.Attributes).
                    WriteLine(">");
            }

            renderer.WriteChildren(obj, false);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.
                    Write("</").
                    Write(elementName).
                    WriteLine(">");
            }
        }
    }
}
