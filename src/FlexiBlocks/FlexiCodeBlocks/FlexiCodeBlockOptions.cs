using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// <para>An implementation of <see cref="FlexiBlockOptions{T}"/> representing options for a FlexiCodeBlock.</para>
    /// 
    /// <para>This class is primarily used through the <see cref="FlexiOptionsBlocksExtension"/>. To that end, this class is designed to be populated from JSON.
    /// This class may occasionally be created manually for use as the default FlexiCodeBlock options, so it accomodates manual creation as well.</para>
    /// 
    /// <para>Markdig is designed to be extensible, as a result, any third party extension can access a FlexiCodeBlock's options. To prevent inconsistent state, 
    /// this class is immutable.</para>
    /// </summary>
    public class FlexiCodeBlockOptions : FlexiBlockOptions<FlexiCodeBlockOptions>
    {
        /// <summary>
        /// Creates a <see cref="FlexiCodeBlockOptions"/> instance.
        /// </summary>
        /// <param name="class">
        /// <para>The FlexiCodeBlock's outermost element's class.</para>
        /// <para>If this value is null, whitespace or an empty string, no class is assigned.</para>
        /// <para>Defaults to "flexi-code-block".</para>
        /// </param>
        /// <param name="copyIconMarkup">
        /// <para>The markup for the FlexiCodeBlock's copy icon.</para>
        /// <para>If this value is null, whitespace or an empty string, no copy icon is rendered.</para>
        /// <para>Defaults to the material design file copy icon - https://material.io/tools/icons/?icon=file_copy&amp;style=baseline.</para>
        /// </param>
        /// <param name="title">
        /// <para>The FlexiCodeBlock's title.</para>
        /// <para>If this value is null, whitespace or an empty string, no title is rendered.</para>
        /// <para>Defaults to null.</para>
        /// </param>    
        /// <param name="language">
        /// <para>The language for syntax highlighting of the FlexiCodeBlock's code.</para>
        /// <para>The value must be a valid language alias for the chosen syntax highlighter (defaults to <see cref="SyntaxHighlighter.Prism"/>).</para>
        /// <para>Valid langauge aliases for Prism can be found here: https://prismjs.com/index.html#languages-list.
        /// Valid language aliases for HighlightJS can be found here: http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases.</para>
        /// <para>If this value is null, whitespace or an empty string, syntax highlighting is disabled and no class is assigned to the FlexiCodeBlock's code element.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="codeClassFormat">
        /// <para>The format for the FlexiCodeBlock's code element's class.</para>
        /// <para>The FlexiCodeBlock's language will replace "{0}" in the format.</para>
        /// <para>If this value or the FlexiCodeBlock's language are null, whitespace or an empty string, no class is assigned to the code element.</para>
        /// <para>Defaults to "language-{0}".</para>
        /// </param>
        /// <param name="syntaxHighlighter">
        /// <para>The syntax highlighter to use for syntax highlighting.</para>
        /// <para>If this value is <see cref="SyntaxHighlighter.None"/>, syntax highlighting will be disabled.</para>
        /// <para>Defaults to <see cref="SyntaxHighlighter.Prism"/>.</para>
        /// </param>
        /// <param name="highlightJSClassPrefix">
        /// <para>The prefix for HighlightJS classes.</para>
        /// <para>This option is only relevant if <see cref="SyntaxHighlighter.HighlightJS"/> is the selected syntax highlighter.</para>
        /// <para>If this value is null, whitespace or an empty string, no prefix is prepended to HighlightJS classes.</para>
        /// <para>Defaults to "hljs-".</para>
        /// </param>
        /// <param name="lineNumberRanges">
        /// <para>The <see cref="LineNumberRange"/>s that specify the line number for each line of code.</para>
        /// <para>If this value is null, no line numbers will be rendered.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="highlightLineRanges">
        /// <para>The <see cref="LineRange"/>s that specify which lines of code to highlight.</para>
        /// <para>If this value is null, no lines will be highlighted.</para>
        /// <para>Line highlighting should not be confused with syntax highlighting. While syntax highlighting highlights tokens in code, line highlighting highlights
        /// entire lines.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="lineEmbellishmentClassesPrefix">
        /// <para>The prefix for line embellishment classes (line embellishments are markup elements added to facilitate per-line styling).</para>
        /// <para>If this value is null, whitespace or an empty string, no prefix is added to line embellishment classes.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the outermost element of the FlexiCodeBlock.</para>
        /// <para>If this value is null, no attributes will be assigned to the outermost element.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="CodeClassFormat"/> is an invalid format.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="SyntaxHighlighter"/> is not within the range of valid vales for the num <see cref="SyntaxHighlighter"/>.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="HighlightLineRanges"/> line ranges are not sequential or overlap.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="LineNumberRanges"/> line ranges are not sequential or overlap.</exception>
        public FlexiCodeBlockOptions(
            string @class = "flexi-code-block",
            string copyIconMarkup = Icons.MATERIAL_DESIGN_FILE_COPY,
            string title = default,
            string language = default,
            string codeClassFormat = "language-{0}",
            SyntaxHighlighter syntaxHighlighter = SyntaxHighlighter.Prism,
            string highlightJSClassPrefix = "hljs-",
            IList<LineNumberRange> lineNumberRanges = default,
            IList<LineRange> highlightLineRanges = default,
            string lineEmbellishmentClassesPrefix = default,
            IDictionary<string, string> attributes = default) : base(attributes)
        {
            Class = @class;
            CopyIconMarkup = copyIconMarkup;
            Title = title;
            Language = language;
            CodeClassFormat = codeClassFormat;
            SyntaxHighlighter = syntaxHighlighter;
            HighlightJSClassPrefix = highlightJSClassPrefix;
            LineNumberRanges = lineNumberRanges == null ? null : new ReadOnlyCollection<LineNumberRange>(lineNumberRanges);
            HighlightLineRanges = highlightLineRanges == null ? null : new ReadOnlyCollection<LineRange>(highlightLineRanges);
            LineEmbellishmentClassesPrefix = lineEmbellishmentClassesPrefix;

            ValidateAndPopulate();
        }

        /// <summary>
        /// Gets or sets the FlexiCodeBlock's outermost element's class.
        /// </summary>
        [JsonProperty]
        public string Class { get; private set; }

        /// <summary>
        /// Gets or sets the markup for the FlexiCodeBlock's copy icon.
        /// </summary>
        [JsonProperty]
        public string CopyIconMarkup { get; private set; }

        /// <summary>
        /// Gets or sets the FlexiCodeBlock's title.
        /// </summary>
        [JsonProperty]
        public string Title { get; private set; }

        /// <summary>
        /// Gets or sets the language for syntax highlighting.
        /// </summary>
        [JsonProperty]
        public string Language { get; private set; }

        /// <summary>
        /// Gets or sets the format for the FlexiCodeBlock's code element's class.
        /// </summary>
        [JsonProperty]
        public string CodeClassFormat { get; private set; }

        /// <summary>
        /// Gets or sets the FlexiCodeBlock's code element's class.
        /// </summary>
        public string CodeClass { get; private set; }

        /// <summary>
        /// Gets or sets the syntax highlighter to use.
        /// </summary>
        [JsonProperty]
        public SyntaxHighlighter SyntaxHighlighter { get; private set; }

        /// <summary>
        /// Gets or sets the prefix for HighlightJS classes.
        /// </summary>
        [JsonProperty]
        public string HighlightJSClassPrefix { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="LineNumberRange"/>s that specify the line number to render for each line of code.
        /// </summary>
        [JsonProperty]
        public ReadOnlyCollection<LineNumberRange> LineNumberRanges { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="LineRange"/>s that specify which lines of code to highlight.
        /// </summary>
        [JsonProperty]
        public ReadOnlyCollection<LineRange> HighlightLineRanges { get; private set; }

        /// <summary>
        /// Gets or sets the prefix for line embellishment classes (line embellishments are markup elements added to facilitate per-line styling).
        /// </summary>
        [JsonProperty]
        public string LineEmbellishmentClassesPrefix { get; private set; }

        /// <summary>
        /// Validates options and populates generated properties.
        /// </summary>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="CodeClassFormat"/> is an invalid format.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="SyntaxHighlighter"/> is not within the range of valid vales for the enum <see cref="SyntaxHighlighter"/>.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="HighlightLineRanges"/>'s line ranges are not sequential or overlap.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <see cref="LineNumberRanges"/>'s line ranges are not sequential or overlap.</exception>        
        protected override void ValidateAndPopulate()
        {
            if (!string.IsNullOrWhiteSpace(Language) &&
                !string.IsNullOrWhiteSpace(CodeClassFormat))
            {
                try
                {
                    CodeClass = string.Format(CodeClassFormat, Language);
                }
                catch (FormatException formatException)
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionIsAnInvalidFormat, nameof(CodeClassFormat),
                        CodeClassFormat),
                        formatException);
                }
            }
            else
            {
                CodeClass = null;
            }

            if (!Enum.IsDefined(typeof(SyntaxHighlighter), SyntaxHighlighter))
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeAValidEnumValue,
                        SyntaxHighlighter,
                        nameof(SyntaxHighlighter),
                        nameof(FlexiCodeBlocks.SyntaxHighlighter)));
            }

            if (HighlightLineRanges?.Count > 0)
            {
                // Highlight line ranges must be in sequential order and must not overlap (overlapping highlight line ranges does not make sense).
                LineRange lastLineRange = null;
                foreach (LineRange lineRange in HighlightLineRanges)
                {
                    ValidateLineRanges(lineRange, lastLineRange, nameof(HighlightLineRanges));
                    lastLineRange = lineRange;
                }
            }

            if (LineNumberRanges?.Count > 0)
            {
                // Line number ranges must be in sequential order and must not overlap (overlapping line number line ranges does not make sense).
                LineRange lastLineRange = null;
                foreach (LineNumberRange lineNumberRange in LineNumberRanges)
                {
                    ValidateLineRanges(lineNumberRange.LineRange, lastLineRange, nameof(LineNumberRanges));
                    lastLineRange = lineNumberRange.LineRange;
                }
            }
        }

        internal void ValidateLineRanges(LineRange lineRange, LineRange lastLineRange, string propertyName)
        {
            if (lastLineRange != null)
            {
                if(lineRange.EndLineNumber != -1 && lineRange.EndLineNumber < lastLineRange.StartLineNumber)
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionLineRangesMustBeSequential,
                        propertyName,
                        lastLineRange.ToString(),
                        lineRange.ToString()));
                }

                if (lastLineRange.EndLineNumber == -1 || lineRange.StartLineNumber <= lastLineRange.EndLineNumber)
                {
                    throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionLineRangesCannotOverlap,
                        propertyName,
                        lastLineRange.ToString(),
                        lineRange.ToString()));
                }
            }
        }
    }
}
