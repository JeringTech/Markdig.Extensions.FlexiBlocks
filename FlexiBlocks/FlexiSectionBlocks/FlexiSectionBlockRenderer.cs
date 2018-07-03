using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    public class FlexiSectionBlockRenderer : HtmlObjectRenderer<FlexiSectionBlock>
    {
        protected override void Write(HtmlRenderer renderer, FlexiSectionBlock obj)
        {
            FlexiSectionBlockOptions flexiSectionBlockOptions = obj.FlexiSectionBlockOptions;

            renderer.EnsureLine();

            // Undefined or None
            if (flexiSectionBlockOptions.WrapperElement.CompareTo(SectioningContentElement.None) <= 0)
            {
                renderer.WriteChildren(obj);
                return;
            }

            string elementName = flexiSectionBlockOptions.WrapperElement.ToString().ToLower();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.
                    Write("<").
                    Write(elementName).
                    WriteHtmlAttributeDictionary(flexiSectionBlockOptions.Attributes).
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
