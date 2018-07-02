using System.Collections.Generic;

namespace FlexiBlocks.FlexiCodeBlocks
{
    public class FlexiCodeBlockOptions : IMarkdownObjectOptions<FlexiCodeBlockOptions>
    {
        /// <summary>
        /// Gets or sets the value used as the markup for the FlexiCodeBlock's copy icon. 
        /// If the value is null, whitespace or an empty string, no copy icon is rendered.
        /// 
        /// The default SVG is part of the excellent material design icon set - https://material.io/tools/icons/?style=baseline
        /// It is licensed under an Apache License Version 2 license - https://www.apache.org/licenses/LICENSE-2.0.html
        /// </summary>
        public string CopyIconMarkup { get; set; } = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0,0h24v24H0V0z\"/><path d=\"M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z\"/></svg>";

        /// <summary>
        /// Gets or sets the value used as the FlexiCodeBlock's title.
        /// If the value is null, whitespace or an empty string, no title is rendered.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value used as the language for syntax highlighting of the FlexiCodeBlock's code.
        /// The value must be a valid language alias for the chosen <see cref="SyntaxHighlighter"/> (defaults to <see cref="SyntaxHighlighter.Prism"/>).
        /// Valid langauge aliases for Prism can be found here: https://prismjs.com/index.html#languages-list.
        /// Valid language aliases for HighlightJS can be found here: http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the value used as the format for the FlexiCodeBlock's code element's language class.
        /// <see cref="Language"/> will be inserted into the format.
        /// If either this format or <see cref="Language"/> is null, whitespace or an empty string, no language class is assigned to the code element.
        /// </summary>
        public string CodeLanguageClassNameFormat { get; set; } = "language-{0}";

        /// <summary>
        /// Gets or sets the value indicating whether code syntax should be highlighted.
        /// </summary>
        public bool HighlightSyntax { get; set; } = true;

        /// <summary>
        /// Gets or sets the value indicating which <see cref="SyntaxHighlighter"/> to use for syntax highlighting.
        /// Defaults to <see cref="SyntaxHighlighter.Prism"/>.
        /// </summary>
        public SyntaxHighlighter SyntaxHighlighter { get; set; }

        /// <summary>
        /// Gets or sets the value used as the prefix for HighlightJS classes. Only relevant if <see cref="SyntaxHighlighter"/>
        /// is set to <see cref="SyntaxHighlighter.HighlightJS"/>.
        /// </summary>
        public string HighlightJSClassPrefix { get; set; } = "hljs-";

        /// <summary>
        /// Gets or sets the boolean value indicating whether or not line numbers should be rendered.
        /// </summary>
        public bool RenderLineNumbers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LineNumberRange"/>s that specify the line number for each line of code. 
        /// If this List is null but <see cref="RenderLineNumbers"/> is true, the first line of code will have line number 1, and the line number will be
        /// incremented for each subsequent line of code.
        /// </summary>
        public List<LineNumberRange> LineNumberRanges { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LineRange"/>s that specify which lines of code to highlight.
        /// </summary>
        public List<LineRange> HighlightLineRanges { get; set; }

        /// <summary>
        /// Gets or sets the value used to prefix line number and line highlighting classes (line embellishment classes). 
        /// If the value is null, whitespace or an empty string, no prefix is added to line embellishment classes.
        /// </summary>
        public string LineEmbellishmentClassesPrefix { get; set; }

        /// <summary>
        /// HTML attributes for the outermost element of the FlexiCodeBlock. Includes a "class" attribute with value "fcb" by default.
        /// </summary>
        public HtmlAttributeDictionary Attributes { get; set; } = new HtmlAttributeDictionary { { "class", "fcb" } };

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public FlexiCodeBlockOptions Clone()
        {
            var result = (FlexiCodeBlockOptions)MemberwiseClone();
            result.Attributes = new HtmlAttributeDictionary(Attributes);

            return result;
        }
    }
}
