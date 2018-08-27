using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// Represents a range of line numbers for a range of lines.
    /// </summary>
    public class LineNumberRange
    {
        /// <summary>
        /// Creates a <see cref="LineNumberRange"/> instance.
        /// </summary>
        /// <param name="startLineNumber">Start line number of the range of lines that this <see cref="LineNumberRange"/> applies to. Must be greater than 0.</param>
        /// <param name="endLineNumber">End line number of the range of lines that his <see cref="LineNumberRange"/> applies to. -1 denotes a range that extends to infinity. If 
        /// <paramref name="endLineNumber"/> is not -1, it must be greater than or equal to <paramref name="startLineNumber"/></param>
        /// <param name="firstLineNumber">Line number of the first line in the range of lines that this <see cref="LineNumberRange"/> applies to. Must
        /// be greater than 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="firstLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        public LineNumberRange(int startLineNumber, int endLineNumber, int firstLineNumber)
        {
            if (firstLineNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(firstLineNumber), 
                    string.Format(Strings.ArgumentOutOfRangeException_LineNumberMustBeGreaterThan0, firstLineNumber));
            }

            LineRange = new LineRange(startLineNumber, endLineNumber);
            FirstLineNumber = firstLineNumber;
        }

        /// <summary>
        /// Gets the line number of the first line in the range of lines that this instance applies to.
        /// </summary>
        public int FirstLineNumber { get; }

        /// <summary>
        /// Gets the line number of the last line in the range of lines that this instance applies to.
        /// </summary>
        public int LastLineNumber => LineRange.NumLines == -1 ? -1 : FirstLineNumber + LineRange.NumLines - 1;

        /// <summary>
        /// Gets the <see cref="LineRange"/> representing the range of lines that this instance applies to.
        /// </summary>
        public LineRange LineRange { get; }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"Lines: {LineRange}, Line numbers: {FirstLineNumber} - {LastLineNumber}";
        }
    }
}
