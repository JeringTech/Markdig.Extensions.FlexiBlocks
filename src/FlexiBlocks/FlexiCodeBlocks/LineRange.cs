using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// Represents a range of lines.
    /// </summary>
    public class LineRange
    {
        /// <summary>
        /// Creates a <see cref="LineRange"/> instance.
        /// </summary>
        /// <param name="startLineNumber">Start line number of this range. Must be greater than 0.</param>
        /// <param name="endLineNumber">End line number of this range. -1 denotes a range that extends to infinity. If <paramref name="endLineNumber"/> is not -1, it must be 
        /// greater than or equal to <paramref name="startLineNumber"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        public LineRange(int startLineNumber, int endLineNumber)
        {
            if(startLineNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(startLineNumber),
                    string.Format(Strings.ArgumentException_LineNumberMustBeGreaterThan0, startLineNumber));
            }

            if(endLineNumber != -1 && endLineNumber < startLineNumber)
            {
                throw new ArgumentOutOfRangeException(nameof(endLineNumber),
                    string.Format(Strings.ArgumentException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, endLineNumber, startLineNumber));
            }

            StartLineNumber = startLineNumber;
            EndLineNumber = endLineNumber;
        }

        /// <summary>
        /// Gets the start line number of this range.
        /// </summary>
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the end line number of this range.
        /// </summary>
        public int EndLineNumber { get; }

        /// <summary>
        /// Gets the number of lines in this range.
        /// </summary>
        public int NumLines => EndLineNumber == -1 ? -1 : EndLineNumber - StartLineNumber + 1;

        /// <summary>
        /// Checks whether <paramref name="lineNumber"/> is within this range.
        /// </summary>
        /// <param name="lineNumber">The line number to check.</param>
        /// <returns>True if <paramref name="lineNumber"/> is within this range, otherwise false.</returns>
        public bool Contains(int lineNumber)
        {
            return lineNumber >= StartLineNumber && (EndLineNumber == -1 || lineNumber <= EndLineNumber);
        }

        /// <summary>
        /// Compares the current instance with <paramref name="lineRange"/> to determine the order in which they occur.
        /// </summary>
        /// <param name="lineRange">The range to compare to.</param>
        /// <returns>-1 if this range occurs before <paramref name="lineRange"/>. 0 if the line range's overlap.
        /// 1 if this range occurs after <paramref name="lineRange"/>.</returns>
        public virtual int CompareTo(LineRange lineRange)
        {
            if(EndLineNumber != -1 && EndLineNumber < lineRange.StartLineNumber)
            {
                return -1;
            }
            else if((StartLineNumber <= lineRange.EndLineNumber && (EndLineNumber == -1 || EndLineNumber >= lineRange.StartLineNumber)) ||
                (lineRange.EndLineNumber == -1 && lineRange.StartLineNumber <= EndLineNumber))
            {
                return 0;
            }
            else
            {
                // StartLineNumber > lineRange.EndLineNumber
                return 1;
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{StartLineNumber} - {EndLineNumber}";
        }
    }
}
