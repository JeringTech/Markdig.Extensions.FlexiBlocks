using System;
using System.Collections;
using System.IO;
using System.Text;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    /// <summary>
    /// Adds embellishements to text. A line can be assigned a line number. A line can also be highlighted.
    /// </summary>
    public class LineEmbellishmentsService
    {
        // TODO to avoid having to iterate over lines multiple times, do all per-line embellishements in this service.
        /// <summary>
        /// Adds table style line numbers. The markup for table style line numbers prevents code from being scrollable horizontally. This style is instead meant for wrapping code, typically using "whitespace: pre-wrap".
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="lineNumberRanges"></param>
        /// <param name="highlightLineRanges"></param>
        /// <param name="prefixForClasses"></param>
        public string EmbellishLines(string lines,
            LineNumberRangeCollection lineNumberRanges = null,
            LineRangeCollection highlightLineRanges = null,
            string prefixForClasses = null)
        {
            if (lineNumberRanges == null && highlightLineRanges == null)
            {
                // Nothing to do
                return lines;
            }

            var result = new StringBuilder();
            using (var stringReader = new StringReader(lines))
            {
                prefixForClasses = prefixForClasses != null ? $"{prefixForClasses}-" : string.Empty;
                string lineStartTag = $"<span class=\"{prefixForClasses}line\">";
                string highlightedLineStartTag = $"<span class=\"{prefixForClasses}line {prefixForClasses}highlight\">";
                string lineNumberStartTag = $"<span class=\"{prefixForClasses}line-number\">";
                string lineTextStartTag = $"<span class=\"{prefixForClasses}line-text\">";
                const string endTag = "</span>";

                int currentLine = 1;

                int currentLineNumberRangeIndex = 0;
                int currentLineNumber = 0;
                LineNumberRange currentLineNumberRange = lineNumberRanges?[0];

                int currentHighlightLineRangeIndex = 0;
                LineRange currentHighlightLineRange = highlightLineRanges?[0];

                string line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    // Set current line number range
                    if (currentLine > currentLineNumberRange?.LineRange.EndLine)
                    {
                        currentLineNumberRange = lineNumberRanges.Count > ++currentLineNumberRangeIndex ? lineNumberRanges[currentLineNumberRangeIndex] : null;
                        currentLineNumber = currentLineNumberRange?.StartLineNumber ?? 0;
                    }

                    // Moved past currentHighlightLineRange 
                    if (currentLine > currentHighlightLineRange?.EndLine)
                    {
                        currentHighlightLineRange = highlightLineRanges.Count > ++currentHighlightLineRangeIndex ? highlightLineRanges[currentHighlightLineRangeIndex] : null;
                    }

                    // If within highlight line range, use highlighted line start tag
                    if (currentHighlightLineRange?.Contains(currentLine) == true)
                    {
                        result.Append(highlightedLineStartTag);
                    }
                    else
                    {
                        result.Append(lineStartTag);
                    }

                    // If within line number range, add line number
                    if (currentLineNumberRange?.LineRange.Contains(currentLine) == true)
                    {
                        result.Append(lineNumberStartTag);
                        result.Append(currentLineNumber++);
                        result.Append(endTag);
                    }

                    // Add line text
                    result.Append(lineTextStartTag);
                    result.Append(line);
                    result.Append(endTag);

                    result.Append(endTag); // End tag for line start tag

                    currentLine++;
                }
            }

            return result.ToString();
        }
    }
}
