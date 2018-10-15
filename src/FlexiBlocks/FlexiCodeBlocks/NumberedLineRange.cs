using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// Represents a range of lines with an associated sequence of numbers.
    /// </summary>
    public class NumberedLineRange
    {
        /// <summary>
        /// Creates a <see cref="NumberedLineRange"/> instance.
        /// </summary>
        /// <param name="startLineNumber">
        /// <para>The line number of this <see cref="NumberedLineRange"/>'s start line.</para>
        /// <para>This value must be greater than 0.</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <param name="endLineNumber">
        /// <para>The line number of this <see cref="NumberedLineRange"/>'s end line.</para>
        /// <para>If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to <paramref name="startLineNumber"/></para>
        /// <para>Defaults to -1.</para>
        /// </param>
        /// <param name="firstNumber">
        /// <para>The number associated with this <see cref="NumberedLineRange"/>'s start line.</para>
        /// <para>The number associated with each subsequent line is incremented by 1.</para>
        /// <para>This value must be greater than 0.</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="firstNumber"/> is less than 1.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        public NumberedLineRange(
            int startLineNumber = 1,
            int endLineNumber = -1,
            int firstNumber = 1)
        {
            if (firstNumber < 1)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeGreaterThan0, nameof(FirstNumber), firstNumber));
            }

            // We need to define these two properties for JSON population to work
            StartLineNumber = startLineNumber;
            EndLineNumber = endLineNumber;

            LineRange = new LineRange(startLineNumber, endLineNumber);
            FirstNumber = firstNumber;
        }

        /// <summary>
        /// Gets the line number of this <see cref="NumberedLineRange"/>'s start line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the line number of this <see cref="NumberedLineRange"/>'s end line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(-1)]
        public int EndLineNumber { get; }

        /// <summary>
        /// Gets the number associated with this <see cref="NumberedLineRange"/>'s start line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
        public int FirstNumber { get; }

        /// <summary>
        /// Gets the number associated with this <see cref="NumberedLineRange"/>'s end line.
        /// </summary>
        public int LastLineNumber => LineRange.NumLines == -1 ? -1 : FirstNumber + LineRange.NumLines - 1;

        /// <summary>
        /// Gets the <see cref="LineRange"/> representing the range of lines that this instance applies to.
        /// </summary>
        public LineRange LineRange { get; }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"Lines: {LineRange}, Line numbers: [{FirstNumber}, {LastLineNumber}]";
        }

        /// <summary>
        /// Checks for value equality between this <see cref="NumberedLineRange"/> and an object.
        /// </summary>
        /// <param name="obj">The object to check for value equality.</param>
        /// <returns>True if this <see cref="NumberedLineRange"/>'s value is equal to the object's value, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is NumberedLineRange otherNumberedLineRange))
            {
                return false;
            }

            return FirstNumber == otherNumberedLineRange.FirstNumber && LineRange.Equals(otherNumberedLineRange.LineRange);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int hashCode = -1329750522;
            hashCode = hashCode * -1521134295 + FirstNumber.GetHashCode();
            return hashCode * -1521134295 + EqualityComparer<LineRange>.Default.GetHashCode(LineRange);
        }
    }
}
