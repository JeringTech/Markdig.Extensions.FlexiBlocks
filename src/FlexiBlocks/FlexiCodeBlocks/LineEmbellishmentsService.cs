using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// The default implementation of <see cref="ILineEmbellishmentsService"/>.
    /// </summary>
    public class LineEmbellishmentsService : ILineEmbellishmentsService
    {
        private const string _spanEndTag = "</span>";

        /// <inheritdoc />
        public string EmbellishLines(string text,
            IEnumerable<LineNumberRange> lineNumberRanges,
            IEnumerable<LineRange> highlightLineRanges,
            string prefixForClasses = null)
        {
            if (!(lineNumberRanges?.Count() > 0 || highlightLineRanges?.Count() > 0))
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
            LineNumberRange currentLineNumberRange = lineNumberRanges?.FirstOrDefault();
            int currentLineNumberRangeIndex = 0, currentLineNumberToRender = currentLineNumberRange?.FirstLineNumber ?? 0;
            LineRange currentHighlightLineRange = highlightLineRanges?.FirstOrDefault();
            int currentHighlightLineRangeIndex = 0;

            var stringReader = new StringReader(text);
            string line = null;
            while ((line = stringReader.ReadLine()) != null)
            {
                currentLineNumber++;

                // Set current line number range
                if (currentLineNumberRange?.LineRange.Before(currentLineNumber) == true)
                {
                    currentLineNumberRange = lineNumberRanges.ElementAtOrDefault(++currentLineNumberRangeIndex);
                    if (currentLineNumberRange != null)
                    {
                        currentLineNumberToRender = currentLineNumberRange.FirstLineNumber;
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
                if (currentLineNumberRange?.LineRange.Contains(currentLineNumber) == true)
                {
                    result.Append(lineNumberStartTag).Append(currentLineNumberToRender++).Append(_spanEndTag);
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
