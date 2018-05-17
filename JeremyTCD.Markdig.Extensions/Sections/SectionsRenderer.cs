using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionsRenderer : HtmlObjectRenderer<SectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, SectionBlock sectionBlock)
        {
            SectionBlockOptions sectionBlockOptions = sectionBlock.SectionBlockOptions;
            SectioningContentElement sectioningContentElement = sectionBlock.Level == 1 ? sectionBlockOptions.Level1WrapperElement : sectionBlockOptions.Level2PlusWrapperElement;

            renderer.EnsureLine();

            if (sectioningContentElement == SectioningContentElement.None)
            {
                renderer.WriteChildren(sectionBlock);
                return;
            }

            string elementName = sectioningContentElement.ToString().ToLower();

            renderer.
                Write("<").
                Write(elementName).
                WriteCustomAttributes(sectionBlockOptions.Attributes).
                WriteLine(">");
            renderer.WriteChildren(sectionBlock);
            renderer.
                Write("</").
                Write(elementName).
                WriteLine(">");
        }
    }
}
