using Newtonsoft.Json;
using System;
using System.ComponentModel;

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
        /// <param name="startLineNumber">
        /// <para>The start line number of the range of lines that this <see cref="LineNumberRange"/> applies to.</para>
        /// <para>This value must be greater than 0.</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <param name="endLineNumber">
        /// <para>The end line number of the range of lines that this <see cref="LineNumberRange"/> applies to.</para>
        /// <para>If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to <paramref name="startLineNumber"/></para>
        /// <para>Defaults to -1.</para>
        /// </param>
        /// <param name="firstLineNumber">
        /// <para>The Line number of the first line in the range of lines that this <see cref="LineNumberRange"/> applies to.</para>
        /// <para>This value must be greater than 0.</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="firstLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        public LineNumberRange(
            int startLineNumber = 1,
            int endLineNumber = -1,
            int firstLineNumber = 1)
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
        /// Gets the start line number of the range of lines that this <see cref="LineNumberRange"/> applies to.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the end line number of the range of lines that this <see cref="LineNumberRange"/> applies to.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(-1)]
        public int EndLineNumber { get; }

        /// <summary>
        /// Gets the Line number of the first line in the range of lines that this <see cref="LineNumberRange"/> applies to.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
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
            return $"Lines: {LineRange}, Line numbers: [{FirstLineNumber}, {LastLineNumber}]";
        }

        /// <summary>
        /// Checks for value equality between this <see cref="LineNumberRange"/> and an object.
        /// </summary>
        /// <param name="obj">The object to check for value equality.</param>
        /// <returns>True if this <see cref="LineNumberRange"/>'s value is equal to the object's value, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is LineNumberRange otherLineNumberRange))
            {
                return false;
            }

            return FirstLineNumber == otherLineNumberRange.FirstLineNumber && LineRange.Equals(otherLineNumberRange.LineRange);
        }
    }
}
