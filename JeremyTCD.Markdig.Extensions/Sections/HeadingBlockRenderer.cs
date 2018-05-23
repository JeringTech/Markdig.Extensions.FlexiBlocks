using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    /// <summary>
    /// An HTML renderer for a <see cref="HeadingBlock"/>.
    /// </summary>
    public class HeadingBlockRenderer : HtmlObjectRenderer<HeadingBlock>
    {
        protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
        {
            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine($"<header class=\"header-level-{obj.Level}\">");
                renderer.Write($"<h{obj.Level}>");
            }

            renderer.WriteLeafInline(obj);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine($"</h{obj.Level}>");

                if (renderer.EnableHtmlForBlock && obj.GetData(SectionBlockParser.ICON_MARKUP_KEY) is string iconMarkup)
                {
                    renderer.WriteLine(iconMarkup);
                }

                renderer.WriteLine("</header>");
            }
        }
    }
}
