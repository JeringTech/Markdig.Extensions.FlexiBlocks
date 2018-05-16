using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockRenderer : HtmlObjectRenderer<SectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, SectionBlock sectionBlock)
        {
            SectionBlockOptions sectionBlockOptions = sectionBlock.SectionBlockOptions;
            string elementName = (sectionBlock.Level == 1 ? sectionBlockOptions.Level1WrapperElement : sectionBlockOptions.Level2PlusWrapperElement).ToString().ToLower();

            renderer.EnsureLine();
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
