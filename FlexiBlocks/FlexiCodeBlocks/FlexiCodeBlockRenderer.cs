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
            var flexiCodeBlockOptions = (FlexiCodeBlockOptions)obj.GetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY);

            renderer.EnsureLine();

            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteLeafRawLines(obj, true, true);
                return;
            }

            renderer.Write("<div").
                WriteHtmlAttributeDictionary(flexiCodeBlockOptions.Attributes).
                WriteLine(">");

            renderer.WriteLine("<header>");
            if (!string.IsNullOrWhiteSpace(flexiCodeBlockOptions.Title))
            {
                // Title
                renderer.WriteLine($"<span>{flexiCodeBlockOptions.Title}</span>");
            }
            if (!string.IsNullOrWhiteSpace(flexiCodeBlockOptions.CopyIconMarkup))
            {
                // Copy icon
                renderer.WriteLine(flexiCodeBlockOptions.CopyIconMarkup);
            }
            renderer.WriteLine("</header>");

            renderer.Write("<pre><code");

            // Add language class to code element
            bool languageIsDefined = !string.IsNullOrWhiteSpace(flexiCodeBlockOptions.Language);
            if (languageIsDefined && !string.IsNullOrWhiteSpace(flexiCodeBlockOptions.CodeLanguageClassNameFormat))
            {
                renderer.Write($" class=\"{string.Format(flexiCodeBlockOptions.CodeLanguageClassNameFormat, flexiCodeBlockOptions.Language)}\"");
            }
            renderer.Write(">");

            // Highlight syntax
            string code = null;
            if (languageIsDefined && flexiCodeBlockOptions.HighlightSyntax)
            {
                _codeRenderer.WriteLeafRawLines(obj, false, false); // Don't escape, prism can't deal with escaped chars

                code = _stringWriter.ToString();
                _stringWriter.GetStringBuilder().Length = 0;

                // Default to Prism
                code = flexiCodeBlockOptions.SyntaxHighlighter == SyntaxHighlighter.HighlightJS ?
                    _highlightJSService.HighlightAsync(code, flexiCodeBlockOptions.Language, flexiCodeBlockOptions.HighlightJSClassPrefix).Result :
                    _prismService.HighlightAsync(code, flexiCodeBlockOptions.Language).Result;
            }

            // Embellish code
            if (flexiCodeBlockOptions.RenderLineNumbers ||
                flexiCodeBlockOptions.HighlightLineRanges?.Count > 0)
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
                    flexiCodeBlockOptions.LineNumberRanges == null || flexiCodeBlockOptions.LineNumberRanges.Count == 0 ? _defaultLineNumberRanges : flexiCodeBlockOptions.LineNumberRanges,
                    flexiCodeBlockOptions.HighlightLineRanges,
                    flexiCodeBlockOptions.LineEmbellishmentClassesPrefix);
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
