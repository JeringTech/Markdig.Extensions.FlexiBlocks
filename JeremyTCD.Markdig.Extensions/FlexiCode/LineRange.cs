using System;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    /// <summary>
    /// Represents a range of lines.
    /// </summary>
    public class LineRange
    {
        /// <summary>
        /// Creates an instance of type <see cref="LineRange"/>.
        /// </summary>
        /// <param name="startLine">Line that range begins at. Must be greater than 0, <see cref="StartLine"/></param>
        /// <param name="endLine">Line that range ends at, -1 denotes a range that extends to infinity. If <paramref name="endLine"/> is not -1, it must be 
        /// greater than or equal to <paramref name="startLine"/></param>
        public LineRange(int startLine, int endLine)
        {
            if(startLine < 1)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidStartLine, startLine));
            }

            if(endLine != -1 && endLine < startLine)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidEndLine, endLine, startLine));
            }

            StartLine = startLine;
            EndLine = endLine;
        }

        /// <summary>
        /// Gets the value specifying the line at which this line range begins.
        /// </summary>
        public int StartLine { get; }

        /// <summary>
        /// Gets the value specifying the line at which this line range ends.
        /// </summary>
        public int EndLine { get; }

        /// <summary>
        /// Gets the value specifying the number of lines in the range.
        /// </summary>
        public int NumLines => EndLine == -1 ? -1 : EndLine - StartLine + 1;

        /// <summary>
        /// Checks whether <paramref name="line"/> is within the range.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>True if <paramref name="line"/> is within the range, otherwise false.</returns>
        public bool Contains(int line)
        {
            return line >= StartLine && (EndLine == -1 || line <= EndLine);
        }

        /// <summary>
        /// Compares the current instance with <paramref name="lineRange"/> to determine the order in which they occur.
        /// </summary>
        /// <param name="lineRange"></param>
        /// <returns>-1 if the current <see cref="LineRange"/> occurs before <paramref name="lineRange"/>. 0 if the <see cref="LineRange"/>s overlap.
        /// 1 if the current <see cref="LineRange"/> occurs after <paramref name="lineRange"/>.</returns>
        public virtual int CompareTo(LineRange lineRange)
        {
            if(EndLine != -1 && EndLine < lineRange.StartLine)
            {
                return -1;
            }
            else if((StartLine <= lineRange.EndLine && (EndLine == -1 || EndLine >= lineRange.StartLine)) ||
                (lineRange.EndLine == -1 && lineRange.StartLine <= EndLine))
            {
                return 0;
            }
            else
            {
                // StartLine > lineRange.EndLine
                return 1;
            }
        }

        public override string ToString()
        {
            return $"{StartLine} - {EndLine}";
        }
    }
}
