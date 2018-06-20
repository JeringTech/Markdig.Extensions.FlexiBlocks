using System;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    /// <summary>
    /// Represents a range of line numbers for a range of lines.
    /// </summary>
    public class LineNumberRange
    {
        public LineNumberRange(int startLine, int endLine, int startLineNumber) :
            this(new LineRange(startLine, endLine), startLineNumber)
        {
        }

        public LineNumberRange(LineRange lineRange, int startLineNumber)
        {
            LineRange = lineRange;

            if(startLineNumber < 1)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_InvalidStartLineNumber, startLineNumber));
            }

            StartLineNumber = startLineNumber;
        }

        /// <summary>
        /// Gets the value that will be used as the start line number for the lines specified by <see cref="LineRange"/>.
        /// The value must be positive.
        /// </summary>
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the <see cref="LineRange"/> that specifies the range of lines to apply line numbers to.
        /// </summary>
        public LineRange LineRange { get; }
    }
}
