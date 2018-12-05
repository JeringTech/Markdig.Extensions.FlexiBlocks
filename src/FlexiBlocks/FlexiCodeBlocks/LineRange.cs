using Newtonsoft.Json;
using System.ComponentModel;

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
        /// <param name="startLineNumber">
        /// <para>The line number of this <see cref="LineRange"/>'s start line.</para>
        /// <para>This value must be greater than 0.</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <param name="endLineNumber">
        /// <para>The line number of this <see cref="LineRange"/>'s end line.</para>
        /// <para>If this value is -1, this range extends to the last line. If it is not -1, it must be greater than or equal to <paramref name="startLineNumber"/></para>
        /// <para>Defaults to -1.</para>
        /// </param>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        public LineRange(int startLineNumber = 1, int endLineNumber = -1)
        {
            if(startLineNumber < 1)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_Shared_OptionMustBeGreaterThan0, nameof(StartLineNumber), startLineNumber));
            }

            if(endLineNumber != -1 && endLineNumber < startLineNumber)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_Shared_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber,
                    nameof(EndLineNumber),
                    endLineNumber,
                    startLineNumber));
            }

            StartLineNumber = startLineNumber;
            EndLineNumber = endLineNumber;
        }

        /// <summary>
        /// Gets the line number of this <see cref="LineRange"/>'s start line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the line number of this <see cref="LineRange"/>'s end line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(-1)]
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
        /// Checks whether this range occurs before <paramref name="lineNumber"/>.
        /// </summary>
        /// <param name="lineNumber">The line number to check.</param>
        /// <returns>True if this range occurs before <paramref name="lineNumber"/>, otherwise false.</returns>
        public bool Before(int lineNumber)
        {
            return EndLineNumber != -1 && lineNumber > EndLineNumber;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"[{StartLineNumber}, {EndLineNumber}]";
        }

        /// <summary>
        /// Checks for value equality between this <see cref="LineRange"/> and an object.
        /// </summary>
        /// <param name="obj">The object to check for value equality.</param>
        /// <returns>True if this <see cref="LineRange"/>'s value is equal to the object's value, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if(!(obj is LineRange otherLineRange))
            {
                return false;
            }

            return StartLineNumber == otherLineRange.StartLineNumber && EndLineNumber == otherLineRange.EndLineNumber;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int hashCode = -1779710799;
            hashCode = hashCode * -1521134295 + StartLineNumber.GetHashCode();
            return hashCode * -1521134295 + EndLineNumber.GetHashCode();
        }
    }
}
