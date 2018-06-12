using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.IO;
using JeremyTCD.WebUtils.SyntaxHighlighters.Prism;
using Microsoft.Extensions.DependencyInjection;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    public class FlexiCodeRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private readonly HtmlRenderer _codeRenderer;
        private readonly StringWriter _stringWriter;
        private readonly IPrismService _prismService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBlockRenderer"/> class.
        /// </summary>
        public FlexiCodeRenderer()
        {
            _stringWriter = new StringWriter();
            _codeRenderer = new HtmlRenderer(_stringWriter);

            // TODO should be shared amongst extension so that there aren't multiple nodeservices instances
            var services = new ServiceCollection();
            services.AddPrism();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _prismService = serviceProvider.GetRequiredService<IPrismService>();
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var flexiCodeOptions = (FlexiCodeOptions)obj.GetData(FlexiCodeExtension.FLEXI_CODE_OPTIONS_KEY);

            renderer.EnsureLine();

            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteLeafRawLines(obj, true, true);
                return;
            }

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

            renderer.Write("<pre><code");
            if (!string.IsNullOrWhiteSpace(flexiCodeOptions.Language))
            {
                if (flexiCodeOptions.Highlight)
                {
                    renderer.Write(">");
                    _codeRenderer.WriteLeafRawLines(obj, true, false); // Don't escape, prism can't deal with escaped chars
                    string code = _stringWriter.ToString();
                    _stringWriter.GetStringBuilder().Length = 0;

                    string highlightedCode = _prismService.Highlight(code, flexiCodeOptions.Language).Result;

                    renderer.Write(highlightedCode);
                }
                else if (!string.IsNullOrWhiteSpace(flexiCodeOptions.CodeLanguageClassNameFormat))
                {
                    // No highlighting, but include class
                    renderer.Write($" class=\"{string.Format(flexiCodeOptions.CodeLanguageClassNameFormat, flexiCodeOptions.Language)}\"");
                    renderer.Write(">");
                    renderer.WriteLeafRawLines(obj, true, true);
                }
            }
            else
            {
                // No highlighting, no class - close and write raw
                renderer.Write(">");
                renderer.WriteLeafRawLines(obj, true, true);
            }

            // TODO add line numbers if flexiCodeOptions.RenderLineNumbers is true

            renderer.WriteLine("</code></pre>");
            renderer.WriteLine("</div>");

        }
    }
}
