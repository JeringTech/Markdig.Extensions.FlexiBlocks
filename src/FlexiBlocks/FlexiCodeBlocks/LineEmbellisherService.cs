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
        private const string _endTagStart = "</";

        /// <inheritdoc />
        public string EmbellishLines(string markupFragment,
            IEnumerable<NumberedLineRange> lineNumberLineRanges,
            IEnumerable<LineRange> highlightLineRanges,
            string prefixForClasses = null,
            string hiddenLinesIconMarkup = null,
            bool splitMultilineElements = true)
        {
            if (string.IsNullOrEmpty(markupFragment))
            {
                return markupFragment; // Nothing to do
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
            var result = new StringBuilder(); // TODO could be pooled
            int currentLineNumber = 0;
            bool lineNumbersEnabled = lineNumberLineRanges?.Count() > 0; // If line numbers are enabled, we render a line number element for every line to facilitate table styles
            NumberedLineRange currentLineNumberLineRange = lineNumbersEnabled ? lineNumberLineRanges.First() : null;
            int currentLineNumberLineRangeIndex = 0, currentLineNumberToRender = lineNumbersEnabled ? currentLineNumberLineRange.FirstNumber : 0;
            LineRange currentHighlightLineRange = highlightLineRanges?.FirstOrDefault();
            int currentHighlightLineRangeIndex = 0;
            bool renderHiddenLinesIcon = !string.IsNullOrWhiteSpace(hiddenLinesIconMarkup);

            var stringReader = new StringReader(markupFragment);
            Stack<StartTagInfo> openStartTagInfos = splitMultilineElements ? new Stack<StartTagInfo>() : null;
            Stack<StartTagInfo> pendingRenderStartTagInfos = splitMultilineElements ? new Stack<StartTagInfo>() : null;
            string line;
            while ((line = stringReader.ReadLine()) != null) // TODO this allocation can be avoided
            {
                currentLineNumber++;

                if (splitMultilineElements)
                {
                    // Set start tags to render
                    pendingRenderStartTagInfos.Clear();
                    foreach (StartTagInfo startTagInfo in openStartTagInfos)
                    {
                        pendingRenderStartTagInfos.Push(startTagInfo);
                    }

                    try
                    {
                        ExtractOpenStartTagInfos(line, openStartTagInfos);
                    }
                    catch (Exception exception) // If markupFragment is not a valid markup fragment or contains void elements, exceptions may be thrown
                    {
                        throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_LineEmbellisherService_InvalidMarkupFragmentWithInnerException,
                            markupFragment,
                            currentLineNumber),
                            exception);
                    }
                }

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
                    else if (renderHiddenLinesIcon)
                    {
                        result.Append(hiddenLinesIconMarkup);
                    }
                    result.Append(_spanEndTag);
                }

                // Add line text
                result.Append(lineTextStartTag);
                if (line.Length > 0)
                {
                    if (splitMultilineElements)
                    {
                        foreach (StartTagInfo startTagInfo in pendingRenderStartTagInfos)
                        {
                            result.
                                Append('<').
                                Append(startTagInfo.Line, startTagInfo.StartIndex, startTagInfo.Length).
                                Append('>');
                        }
                        result.Append(line);
                        foreach (StartTagInfo startTagInfo in openStartTagInfos)
                        {
                            result.
                                Append(_endTagStart).
                                Append(startTagInfo.Line, startTagInfo.StartIndex, startTagInfo.NameLength).
                                Append('>');
                        }
                    }
                    else
                    {
                        result.Append(line);
                    }
                }
                result.Append(_spanEndTag);

                // End tag for line start tag
                result.AppendLine(_spanEndTag);
            }

            // If openStartTagInfos isn't empty, there is no way that embellishing was completed properly. 
            // markupFragment likely contains void elements or has more opening than closing tags.
            if (openStartTagInfos?.Count > 0)
            {
                throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_LineEmbellisherService_InvalidMarkupFragment,
                    markupFragment));
            }

            // Remove last new line character(s)
            result.Length -= Environment.NewLine.Length;

            return result.ToString();
        }

        internal virtual void ExtractOpenStartTagInfos(string line, Stack<StartTagInfo> openStartTagInfos)
        {
            int state = 0; // 0 == base state, 1 == in start tag name, 2 == in start tag attributes
            int currentStartIndex = 0;
            int currentNameEndIndex = 0;
            for (int i = 0; i < line.Length; i++)
            {
                char currentChar = line[i];

                if (state == 0 && currentChar == '<')
                {
                    if (line[++i] != '/') // Throws if line ends with '<'
                    {
                        state = 1;
                        currentStartIndex = i;
                    }
                    else
                    {
                        openStartTagInfos.Pop(); // Assumes that tags match, throws if more end tags than start tags (openStartTagInfos is empty and we encountered an end tag)
                        i += 2; // Tag name must be at least 1 character, and it must be followed by >
                    }
                }
                else if (state == 1)
                {
                    if (currentChar == '>')
                    {
                        currentNameEndIndex = i - 1;
                        openStartTagInfos.Push(new StartTagInfo(line, currentStartIndex, currentNameEndIndex, currentNameEndIndex));
                        state = 0;
                    }
                    else if (currentChar == '\n' ||
                            currentChar == '\r' ||
                            currentChar == '\t' ||
                            currentChar == ' ')
                    {
                        currentNameEndIndex = i - 1;
                        state = 2;
                    }
                }
                else if (state == 2 && currentChar == '>')
                {
                    openStartTagInfos.Push(new StartTagInfo(line, currentStartIndex, i - 1, currentNameEndIndex));
                    state = 0;
                }
            }
        }

        internal readonly struct StartTagInfo
        {
            public readonly string Line;
            public readonly int StartIndex, EndIndex, NameEndIndex;

            public StartTagInfo(string line, int startIndex, int endIndex, int nameEndIndex)
            {
                Line = line;
                StartIndex = startIndex;
                EndIndex = endIndex;
                NameEndIndex = nameEndIndex;
            }

            public int Length => EndIndex - StartIndex + 1;

            public int NameLength => NameEndIndex - StartIndex + 1;
        }
    }
}
