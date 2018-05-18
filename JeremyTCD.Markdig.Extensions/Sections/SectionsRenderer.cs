using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionsRenderer : HtmlObjectRenderer<SectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, SectionBlock obj)
        {
            SectionBlockOptions sectionBlockOptions = obj.SectionBlockOptions;
            SectioningContentElement sectioningContentElement = obj.Level == 1 ? sectionBlockOptions.Level1WrapperElement : sectionBlockOptions.Level2PlusWrapperElement;

            renderer.EnsureLine();

            if (sectioningContentElement == SectioningContentElement.None)
            {
                renderer.WriteChildren(obj);
                return;
            }

            string elementName = sectioningContentElement.ToString().ToLower();

            renderer.
                Write("<").
                Write(elementName).
                WriteCustomAttributes(sectionBlockOptions.Attributes).
                WriteLine(">");
            renderer.WriteChildren(obj);
            renderer.
                Write("</").
                Write(elementName).
                WriteLine(">");
        }
    }
}
