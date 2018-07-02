using JeremyTCD.WebUtils.SyntaxHighlighters.HighlightJS;
using JeremyTCD.WebUtils.SyntaxHighlighters.Prism;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Collections.Generic;
using System.IO;

namespace FlexiBlocks.FlexiCodeBlocks
{
    public class FlexiCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private static readonly List<LineNumberRange> _defaultLineNumberRanges = new List<LineNumberRange> { new LineNumberRange(1, -1, 1) };

        private readonly HtmlRenderer _codeRenderer;
        private readonly StringWriter _stringWriter;
        private readonly IPrismService _prismService;
        private readonly IHighlightJSService _highlightJSService;
        private readonly LineEmbellishmentsService _lineEmbellishmentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexiCodeBlockRenderer"/> class.
        /// </summary>
        /// <param name="prismService"></param>
        /// <param name="highlightJSService"></param>
        public FlexiCodeBlockRenderer(IPrismService prismService,
            IHighlightJSService highlightJSService)
        {
            _stringWriter = new StringWriter();
            _codeRenderer = new HtmlRenderer(_stringWriter);
            _prismService = prismService;
            _highlightJSService = highlightJSService;
            _lineEmbellishmentsService = new LineEmbellishmentsService();
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var flexiCodeOptions = (FlexiCodeBlockOptions)obj.GetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY);

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
                // Title
                renderer.WriteLine($"<span>{flexiCodeOptions.Title}</span>");
            }
            if (!string.IsNullOrWhiteSpace(flexiCodeOptions.CopyIconMarkup))
            {
                // Copy icon
                renderer.WriteLine(flexiCodeOptions.CopyIconMarkup);
            }
            renderer.WriteLine("</header>");

            renderer.Write("<pre><code");

            // Add language class to code element
            bool languageIsDefined = !string.IsNullOrWhiteSpace(flexiCodeOptions.Language);
            if (languageIsDefined && !string.IsNullOrWhiteSpace(flexiCodeOptions.CodeLanguageClassNameFormat))
            {
                renderer.Write($" class=\"{string.Format(flexiCodeOptions.CodeLanguageClassNameFormat, flexiCodeOptions.Language)}\"");
            }
            renderer.Write(">");

            // Highlight syntax
            string code = null;
            if (languageIsDefined && flexiCodeOptions.HighlightSyntax)
            {
                _codeRenderer.WriteLeafRawLines(obj, false, false); // Don't escape, prism can't deal with escaped chars

                code = _stringWriter.ToString();
                _stringWriter.GetStringBuilder().Length = 0;

                // Default to Prism
                code = flexiCodeOptions.SyntaxHighlighter == SyntaxHighlighter.HighlightJS ?
                    _highlightJSService.HighlightAsync(code, flexiCodeOptions.Language, flexiCodeOptions.HighlightJSClassPrefix).Result :
                    _prismService.HighlightAsync(code, flexiCodeOptions.Language).Result;
            }

            // Embellish code
            if (flexiCodeOptions.RenderLineNumbers ||
                flexiCodeOptions.HighlightLineRanges?.Count > 0)
            {
                // TODO optimize - possible to pass obj.Lines directly to EmbellishLines?
                // Code still null since syntax highlighting wasn't done
                if (code == null)
                {
                    _codeRenderer.WriteLeafRawLines(obj, false, true); // Escape
                    code = _stringWriter.ToString();
                    _stringWriter.GetStringBuilder().Length = 0;
                }

                code = _lineEmbellishmentsService.EmbellishLines(code,
                    flexiCodeOptions.LineNumberRanges == null || flexiCodeOptions.LineNumberRanges.Count == 0 ? _defaultLineNumberRanges : flexiCodeOptions.LineNumberRanges,
                    flexiCodeOptions.HighlightLineRanges,
                    flexiCodeOptions.LineEmbellishmentClassesPrefix);
            }

            if (code != null)
            {
                renderer.Write(code);
            }
            else
            {
                // No embellishing and no syntax highlighting, write directly to renderer to avoid unecessary string allocation
                renderer.WriteLeafRawLines(obj, false, true);
            }

            renderer.WriteLine("</code></pre>");
            renderer.WriteLine("</div>");

        }
    }
}
