using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Markdig.Renderers;
using Markdig.Syntax;
using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// A renderer that renders FlexiCodeBlocks as HTML.
    /// </summary>
    public class FlexiCodeBlockRenderer : FlexiBlockRenderer<CodeBlock>
    {
        private readonly StringWriter _stringWriter;
        private readonly HtmlRenderer _codeRenderer;

        private readonly IPrismService _prismService;
        private readonly IHighlightJSService _highlightJSService;
        private readonly ILineEmbellisherService _lineEmbellisherService;

        /// <summary>
        /// Creates a <see cref="FlexiCodeBlockRenderer"/> instance.
        /// </summary>
        /// <param name="prismService">The service that will handle syntax highlighting using Prism.</param>
        /// <param name="highlightJSService">The service that will handle syntax highlighting using HighlightJS.</param>
        /// <param name="lineEmbellisherService">The service that will handle line embellishments (line highlighting and line numbers).</param>
        public FlexiCodeBlockRenderer(IPrismService prismService,
            IHighlightJSService highlightJSService,
            ILineEmbellisherService lineEmbellisherService)
        {
            _prismService = prismService ?? throw new ArgumentNullException(nameof(prismService));
            _highlightJSService = highlightJSService ?? throw new ArgumentNullException(nameof(highlightJSService));
            _lineEmbellisherService = lineEmbellisherService ?? throw new ArgumentNullException(nameof(lineEmbellisherService));

            _stringWriter = new StringWriter();
            _codeRenderer = new HtmlRenderer(_stringWriter);
        }

        /// <summary>
        /// Renders a FlexiCodeBlock as HTML.
        /// </summary>
        /// <param name="renderer">The renderer to write to.</param>
        /// <param name="obj">The FlexiCodeBlock to render.</param>
        protected override void WriteFlexiBlock(HtmlRenderer renderer, CodeBlock obj)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            renderer.EnsureLine();

            if (!renderer.EnableHtmlForBlock)
            {
                renderer.WriteLeafRawLines(obj, true, true);
                return;
            }

            var flexiCodeBlockOptions = (FlexiCodeBlockOptions)obj.GetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY);

            renderer.
                Write("<div").
                WriteAttributes(flexiCodeBlockOptions.Attributes).
                WriteLine(">").
                WriteLine("<header>");

            // Title
            if (!string.IsNullOrWhiteSpace(flexiCodeBlockOptions.Title))
            {
                renderer.WriteLine($"<span>{flexiCodeBlockOptions.Title}</span>");
            }

            // Copy icon
            if (!string.IsNullOrWhiteSpace(flexiCodeBlockOptions.CopyIconMarkup))
            {
                renderer.WriteLine(flexiCodeBlockOptions.CopyIconMarkup);
            }

            renderer.
                WriteLine("</header>").
                Write("<pre><code");

            // Language class
            if (!string.IsNullOrWhiteSpace(flexiCodeBlockOptions.CodeClass))
            {
                renderer.Write($" class=\"{flexiCodeBlockOptions.CodeClass}\"");
            }

            renderer.Write(">");

            // Syntax highlighting
            string code = null;
            if (!string.IsNullOrWhiteSpace(flexiCodeBlockOptions.Language) && flexiCodeBlockOptions.SyntaxHighlighter != SyntaxHighlighter.None)
            {
                _codeRenderer.WriteLeafRawLines(obj, false, false); // Don't escape, prism can't deal with escaped chars
                code = _stringWriter.ToString();
                _stringWriter.GetStringBuilder().Length = 0;

                // All code up the stack from HighlightAsync calls ConfigureAwait(false), so there is no need to run this calls in the thread pool.
                // Use GetAwaiter and GetResult to avoid an AggregateException - https://blog.stephencleary.com/2014/12/a-tour-of-task-part-6-results.html
                if (flexiCodeBlockOptions.SyntaxHighlighter == SyntaxHighlighter.HighlightJS)
                {
                    code = _highlightJSService.
                        HighlightAsync(code, flexiCodeBlockOptions.Language, flexiCodeBlockOptions.HighlightJSClassPrefix).GetAwaiter().GetResult();
                }
                else
                {
                    // Default to Prism
                    code = _prismService.HighlightAsync(code, flexiCodeBlockOptions.Language).GetAwaiter().GetResult();
                }
            }

            // Line embellishments (line highlighting and line numbers)
            if (flexiCodeBlockOptions.LineNumberRanges?.Count > 0 ||
                flexiCodeBlockOptions.HighlightLineRanges?.Count > 0)
            {
                // Code still null since syntax highlighting wasn't done
                if (code == null)
                {
                    _codeRenderer.WriteLeafRawLines(obj, false, true); // Escape
                    code = _stringWriter.ToString();
                    _stringWriter.GetStringBuilder().Length = 0;
                }

                code = _lineEmbellisherService.EmbellishLines(code,
                    flexiCodeBlockOptions.LineNumberRanges,
                    flexiCodeBlockOptions.HighlightLineRanges,
                    flexiCodeBlockOptions.LineEmbellishmentClassesPrefix);
            }

            if (code != null)
            {
                renderer.Write(code);
            }
            else
            {
                // No embellishing and no syntax highlighting, write directly to renderer
                renderer.WriteLeafRawLines(obj, false, true); // Escape
            }

            renderer.
                WriteLine("</code></pre>").
                WriteLine("</div>");
        }
    }
}
