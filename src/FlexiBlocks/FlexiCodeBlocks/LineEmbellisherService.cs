using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// The default implementation of <see cref="ILineEmbellisherService"/>.
    /// </summary>
    public class LineEmbellisherService : ILineEmbellisherService
    {
        private const string _spanEndTag = "</span>";

        /// <inheritdoc />
        public string EmbellishLines(string text,
            IEnumerable<NumberedLineRange> lineNumberLineRanges,
            IEnumerable<LineRange> highlightLineRanges,
            string prefixForClasses = null,
            string hiddenLinesIconMarkup = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text; // Nothing to do
            }

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
            int currentLineNumber = 0;
            bool lineNumbersEnabled = lineNumberLineRanges?.Count() > 0; // If line numbers are enabled, we render a line number element for every line to facilitate table styles
            NumberedLineRange currentLineNumberLineRange = lineNumbersEnabled ? lineNumberLineRanges.First() : null;
            int currentLineNumberLineRangeIndex = 0, currentLineNumberToRender = lineNumbersEnabled ? currentLineNumberLineRange.FirstNumber : 0;
            LineRange currentHighlightLineRange = highlightLineRanges?.FirstOrDefault();
            int currentHighlightLineRangeIndex = 0;
            bool renderHiddenLinesIcon = !string.IsNullOrWhiteSpace(hiddenLinesIconMarkup);

            var stringReader = new StringReader(text);
            string line = null;
            while ((line = stringReader.ReadLine()) != null)
            {
                currentLineNumber++;

                // Set current line number range
                if (currentLineNumberLineRange?.Before(currentLineNumber) == true)
                {
                    currentLineNumberLineRange = lineNumberLineRanges.ElementAtOrDefault(++currentLineNumberLineRangeIndex);
                    if (currentLineNumberLineRange != null)
                    {
                        currentLineNumberToRender = currentLineNumberLineRange.FirstNumber;
                    }
                }

                // Set current highlight range
                if (currentHighlightLineRange?.Before(currentLineNumber) == true)
                {
                    currentHighlightLineRange = highlightLineRanges?.ElementAtOrDefault(++currentHighlightLineRangeIndex);
                }

                // Line start tag
                result.Append(currentHighlightLineRange?.Contains(currentLineNumber) == true ? highlightedLineStartTag : lineStartTag);

                // If within line number range, add line number
                if (lineNumbersEnabled)
                {
                    result.Append(lineNumberStartTag);
                    if (currentLineNumberLineRange?.Contains(currentLineNumber) == true)
                    {
                        result.Append(currentLineNumberToRender++);
                    }
                    else if(renderHiddenLinesIcon)
                    {
                        result.Append(hiddenLinesIconMarkup);
                    }
                    result.Append(_spanEndTag);
                }

                // Add line text
                result.Append(lineTextStartTag).Append(line).Append(_spanEndTag);

                // End tag for line start tag
                result.AppendLine(_spanEndTag);
            }

            // Remove last new line character(s)
            result.Length -= Environment.NewLine.Length;

            return result.ToString();
        }
    }
}
