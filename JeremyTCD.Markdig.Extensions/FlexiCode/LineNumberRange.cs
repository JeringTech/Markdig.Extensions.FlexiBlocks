using System;

namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    /// <summary>
    /// Represents a range of line numbers for a range of lines.
    /// </summary>
    public class LineNumberRange
    {
        /// <summary>
        /// Creates an instance of type <see cref="LineNumberRange"/>.
        /// </summary>
        /// <param name="startLine">Start line of range that this <see cref="LineNumberRange"/> applies to. Must be greater than 0, <see cref="StartLine"/></param>
        /// <param name="endLine">End line of range that his <see cref="LineNumberRange"/> applies to. -1 denotes a range that extends to infinity. If 
        /// <paramref name="endLine"/> is not -1, it must be greater than or equal to <paramref name="startLine"/></param>
        /// <param name="startLineNumber">Starting line number for the lines specified by <paramref name="startLine"/> and <paramref name="endLine"/>. Must
        /// be greater than 0.</param>
        public LineNumberRange(int startLine, int endLine, int startLineNumber) :
            this(new LineRange(startLine, endLine), startLineNumber)
        {
        }

        /// <summary>
        /// Creates an instance of type <see cref="LineNumberRange"/>.
        /// </summary>
        /// <param name="lineRange"><see cref="LineRange"/> specifying lines that this <see cref="LineNumberRange"/> applies to.</param>
        /// <param name="startLineNumber">Starting line number for the lines specified by <paramref name="startLine"/> and <paramref name="endLine"/>. Must
        /// be greater than 0.</param>
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
        /// </summary>
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the value specifying the last line number for the line number range.
        /// </summary>
        public int EndLineNumber => LineRange.NumLines == -1 ? -1 : StartLineNumber + LineRange.NumLines - 1;

        /// <summary>
        /// Gets the <see cref="LineRange"/> that specifies the range of lines to apply line numbers to.
        /// </summary>
        public LineRange LineRange { get; }

        public override string ToString()
        {
            return $"Lines: {LineRange}, Line numbers: {StartLineNumber} - {EndLineNumber}";
        }
    }
}
