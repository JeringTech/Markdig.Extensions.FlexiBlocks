using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace FlexiBlocks.Sections
{
    public class FlexiSectionBlockRenderer : HtmlObjectRenderer<FlexiSectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, FlexiSectionBlock obj)
        {
            FlexiSectionBlockOptions sectionBlockOptions = obj.SectionBlockOptions;

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
                    WriteHtmlAttributeDictionary(sectionBlockOptions.Attributes).
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
