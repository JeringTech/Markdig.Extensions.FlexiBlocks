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
        /// <param name="startLine">Must be larger than 0, <see cref="StartLine"/></param>
        /// <param name="endLine">If not negative, must be greater than or equal to <paramref name="startLine"/></param>
        public LineRange(int startLine, int endLine)
        {
            if(startLine < 1)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidStartLine, startLine));
            }

            if(endLine < startLine)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidEndLine, endLine, startLine));
            }

            StartLine = startLine;
            EndLine = endLine;
        }

        /// <summary>
        /// Gets the value specifying the line at which this line range begins. Lines numbers start from 1, this value cannot be
        /// less than 1.
        /// </summary>
        public int StartLine { get; }

        /// <summary>
        /// Gets the value specifying the line at which this line range ends. If the value is negative, the range continues till the
        /// end of the sequence of lines. If the value isn't negative, it must be greater than or equal to <see cref="StartLine"/>'s value. 
        /// </summary>
        public int EndLine { get; }

        /// <summary>
        /// Checks whether <paramref name="line"/> is within the range.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>True if <paramref name="line"/> is within the range, otherwise false.</returns>
        public bool Contains(int line)
        {
            return line >= StartLine && line <= EndLine;
        }
    }
}
