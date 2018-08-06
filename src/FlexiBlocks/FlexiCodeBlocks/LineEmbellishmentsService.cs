using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// Adds embellishements to lines in a block of text.
    /// </summary>
    public class LineEmbellishmentsService
    {
        private static readonly string[] _newLineStrings = new string[] { "\r\n", "\n", "\r" };
        private const string _spanEndTag = "</span>";

        /// <summary>
        /// Adds line numbers and highlights lines.
        /// </summary>
        /// <param name="text">Source of lines to embellish.</param>
        /// <param name="lineNumberRanges">Ranges of lines to add line numbers for and line numbers to use for each line. If null, line numbers will not be added.</param>
        /// <param name="highlightLineRanges">Ranges of lines to highlight. If null, no lines will be highlighted.</param>
        /// <param name="prefixForClasses">Optional prefix for classes.</param>
        /// <returns><paramref name="text"/> if <paramref name="highlightLineRanges"/> and <paramref name="lineNumberRanges"/> are both null or empty. Otherwise, 
        /// <paramref name="text"/> with added line numbers and highlighted lines.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="highlightLineRanges"/> or <paramref name="lineNumberRanges"/> contain overlapping ranges.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the full ranges of <paramref name="highlightLineRanges"/> or <paramref name="lineNumberRanges"/> exceeds 
        /// the lines in <paramref name="text"/>.</exception>
        public string EmbellishLines(string text,
            List<LineNumberRange> lineNumberRanges,
            List<LineRange> highlightLineRanges,
            string prefixForClasses = null)
        {
            if ((lineNumberRanges == null || lineNumberRanges.Count == 0) &&
                (highlightLineRanges == null || highlightLineRanges.Count == 0))
            {
                // Nothing to do
                return text;
            }

            // Sort ranges in ascending order
            lineNumberRanges?.Sort(CompareLineNumberRanges);
            highlightLineRanges?.Sort(CompareHighlightLineRanges);

            // TODO use Span
            // Get lines, we need to know the number of lines in the text to verify that the provided ranges are valid
            string[] lines = text.Split(_newLineStrings, StringSplitOptions.None);
            int numLines = lines.Length;

            // Validate ranges
            ValidateRanges(lineNumberRanges, highlightLineRanges, numLines);

            // Embellishments
            if (string.IsNullOrWhiteSpace(prefixForClasses))
            {
                prefixForClasses = string.Empty;
            }
            string lineStartTag = $"<span class=\"{prefixForClasses}line\">";
            string highlightedLineStartTag = $"<span class=\"{prefixForClasses}line {prefixForClasses}highlight\">";
            string lineNumberStartTag = $"<span class=\"{prefixForClasses}line-number\">";
            string lineTextStartTag = $"<span class=\"{prefixForClasses}line-text\">";

            // Embellish lines
            var result = new StringBuilder();
            int currentLine = 1; // Actual line we are at

            int currentLineNumberRangeIndex = 0;
            LineNumberRange currentLineNumberRange = lineNumberRanges?.FirstOrDefault();
            int currentLineNumber = currentLineNumberRange?.StartLineNumber ?? 0; // Line number to render
            int currentLineNumberRangeEndLine = currentLineNumberRange?.LineRange.EndLine ?? 0;

            int currentHighlightLineRangeIndex = 0;
            LineRange currentHighlightLineRange = highlightLineRanges?.FirstOrDefault();
            int currentHighlightLineRangeEndLine = currentHighlightLineRange?.EndLine ?? 0;

            foreach (string line in lines)
            {
                // Set current line number range
                if (currentLineNumberRange != null &&
                    currentLineNumberRangeEndLine != -1 &&
                    currentLine > currentLineNumberRangeEndLine)
                {
                    currentLineNumberRange = lineNumberRanges.ElementAtOrDefault(++currentLineNumberRangeIndex);
                    if (currentLineNumberRange != null)
                    {
                        currentLineNumberRangeEndLine = currentLineNumberRange.LineRange.EndLine;
                        currentLineNumber = currentLineNumberRange.StartLineNumber;
                    }
                }

                // Set current highlight range
                if (currentHighlightLineRange != null &&
                    currentHighlightLineRangeEndLine != -1 &&
                    currentLine > currentHighlightLineRangeEndLine)
                {
                    currentHighlightLineRange = highlightLineRanges.ElementAtOrDefault(++currentHighlightLineRangeIndex);
                    if (currentHighlightLineRange != null)
                    {
                        currentHighlightLineRangeEndLine = currentHighlightLineRange.EndLine;
                    }
                }

                // Line start tag
                result.Append(currentHighlightLineRange?.Contains(currentLine) == true ? highlightedLineStartTag : lineStartTag);

                // If within line number range, add line number
                if (currentLineNumberRange?.LineRange.Contains(currentLine) == true)
                {
                    result.Append(lineNumberStartTag).Append(currentLineNumber++).Append(_spanEndTag);
                }

                // Add line text
                result.Append(lineTextStartTag).Append(line).Append(_spanEndTag);

                // End tag for line start tag
                result.AppendLine(_spanEndTag);

                currentLine++;
            }

            // Remove last new line character(s)
            result.Length -= Environment.NewLine.Length;

            return result.ToString();
        }

        internal void ValidateRanges(List<LineNumberRange> lineNumberRanges,
            List<LineRange> highlightLineRanges,
            int numLines)
        {
            // Ranges must be a subset of the lines
            LineRange lastLineNumberLineRange = lineNumberRanges?.LastOrDefault()?.LineRange;
            if (lastLineNumberLineRange != null &&
                (lastLineNumberLineRange.StartLine > numLines ||
                lastLineNumberLineRange.EndLine > numLines))
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_InvalidLineNumberLineRange, lastLineNumberLineRange.ToString(), numLines));
            }
            LineRange lastHighlightLineRange = highlightLineRanges?.LastOrDefault();
            if (lastHighlightLineRange != null &&
               (lastHighlightLineRange.StartLine > numLines ||
               lastHighlightLineRange.EndLine > numLines))
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_InvalidHighlightLineRange, lastHighlightLineRange.ToString(), numLines));
            }
        }

        internal int CompareHighlightLineRanges(LineRange x, LineRange y)
        {
            int result = x.CompareTo(y);

            // Line ranges cannot overlap
            if (result == 0)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_LineRangesForHighlightingCannotOverlap, x.ToString(), y.ToString()));
            }

            return result;
        }

        internal int CompareLineNumberRanges(LineNumberRange x, LineNumberRange y)
        {
            int result = x.LineRange.CompareTo(y.LineRange);

            // Line ranges cannot overlap and line number ranges cannot overlap or be in the wrong order
            if (result == 0 ||
                result == -1 && x.EndLineNumber >= y.StartLineNumber ||
                result == 1 && y.EndLineNumber >= x.StartLineNumber)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_LineNumbersCannotOverlap, x.ToString(), y.ToString()));
            }

            return result;
        }
    }
}
