using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    public class FlexiCodeRenderer : HtmlObjectRenderer<CodeBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBlockRenderer"/> class.
        /// </summary>
        public FlexiCodeRenderer()
        {
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var flexiCodeOptions = (FlexiCodeOptions)obj.GetData(FlexiCodeExtension.FLEXI_CODE_OPTIONS_KEY);

            renderer.EnsureLine();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").
                    WriteHtmlAttributeDictionary(flexiCodeOptions.Attributes).
                    WriteLine(">");

                renderer.WriteLine("<header>");
                if (!string.IsNullOrWhiteSpace(flexiCodeOptions.Title))
                {
                    renderer.WriteLine($"<span>{flexiCodeOptions.Title}</span>");
                }
                if (!string.IsNullOrWhiteSpace(flexiCodeOptions.IconMarkup))
                {
                    renderer.WriteLine(flexiCodeOptions.IconMarkup);
                }
                renderer.WriteLine("</header>");
                
                renderer.Write("<pre");
                renderer.Write("><code");
                renderer.Write(">");
            }

            // TODO render to string and highlight if flexiCodeOptions.HightlightLanguage is defined
            // TODO add line numbers if flexiCodeOptions.RenderLineNumbers is true
            renderer.WriteLeafRawLines(obj, true, true);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine("</code></pre>");
                renderer.WriteLine("</div>");
            }
        }
    }
}
