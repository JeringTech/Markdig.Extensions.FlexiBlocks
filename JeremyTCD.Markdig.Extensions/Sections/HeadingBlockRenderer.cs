using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Globalization;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    /// <summary>
    /// An HTML renderer for a <see cref="HeadingBlock"/>.
    /// </summary>
    public class HeadingBlockRenderer : HtmlObjectRenderer<HeadingBlock>
    {
        private static readonly string[] HeadingTexts = {
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6",
        };

        protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
        {
            string headingText = obj.Level > 0 && obj.Level <= 6
                ? HeadingTexts[obj.Level - 1]
                : "<h" + obj.Level.ToString(CultureInfo.InvariantCulture);

            if (renderer.EnableHtmlForBlock)
            {
                // Wrap heading in a header element - https://developer.mozilla.org/en-US/docs/Web/HTML/Element/header
                renderer.WriteLine($"<header class=\"header-level-{obj.Level}\">");
                renderer.Write("<").Write(headingText).WriteAttributes(obj).Write(">");
            }

            renderer.WriteLeafInline(obj);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("</").Write(headingText).WriteLine(">");

                if (renderer.EnableHtmlForBlock && obj.GetData(SectionBlockParser.ICON_MARKUP_KEY) is string iconMarkup)
                {
                    renderer.WriteLine(iconMarkup);
                }

                renderer.WriteLine("</header>");
            }
        }
    }
}
