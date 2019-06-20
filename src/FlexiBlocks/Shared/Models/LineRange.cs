using Newtonsoft.Json;
using System.ComponentModel;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents a range of lines.
    /// </summary>
    public class LineRange
    {
        /// <summary>
        /// Default start line line number.
        /// </summary>
        protected const int _defaultStart = 1;

        /// <summary>
        /// Default end line line number.
        /// </summary>
        protected const int _defaultEnd = -1;

        /// <summary>
        /// Creates a <see cref="LineRange"/>.
        /// </summary>
        /// <param name="start">
        /// <para>The line number of the <see cref="LineRange"/>'s start line.</para>
        /// <para>If this value is <c>-n</c>, the start line is the nth last line. For example, if this value is <c>-2</c>, the start line is the 2nd last line.</para>
        /// <para>This value must not be <c>0</c>.</para>
        /// <para>Defaults to <c>1</c>.</para>
        /// </param>
        /// <param name="end">
        /// <para>The line number of the <see cref="LineRange"/>'s end line.</para>
        /// <para>If this value is <c>-n</c>, the end line is the nth last line. For example, if this value is <c>-2</c>, the end line is the 2nd last line.</para>
        /// <para>This value must not be <c>0</c> or an integer representing a line before the start line.</para>
        /// <para>Defaults to <c>-1</c>.</para>
        /// </param>
        /// <exception cref="OptionsException">Thrown if <paramref name="start"/> is 0.</exception>
        /// <exception cref="OptionsException">Thrown if <paramref name="end"/> is 0.</exception>
        /// <exception cref="OptionsException">Thrown if the end line is a line before the start line.</exception>
        public LineRange(int start = _defaultStart, int end = _defaultEnd)
        {
            if (start == 0)
            {
                throw new OptionsException(nameof(Start),
                    string.Format(Strings.OptionsException_Shared_InvalidValue, start));
            }

            if (end == 0)
            {
                throw new OptionsException(nameof(End),
                    string.Format(Strings.OptionsException_Shared_InvalidValue, end));
            }

            if (end > 0 && start > 0 && end < start ||
                end < 0 && start < 0 && end < start)
            {
                throw new OptionsException(string.Format(Strings.OptionsException_LineRange_EndLineBeStartLineOrALineAfterIt,
                    start, end));
            }

            Start = start;
            End = end;
        }

        /// <summary>
        /// Gets the line number of the <see cref="LineRange"/>'s start line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(_defaultStart)]
        public int Start { get; private set; }

        /// <summary>
        /// Gets the line number of the <see cref="LineRange"/>'s end line.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(_defaultEnd)]
        public int End { get; private set; }

        /// <summary>
        /// <para>Gets the line numbers of the <see cref="LineRange"/>'s start and end lines normalized for a given number of lines.</para>
        /// <para>Normalization converts negative values and <c>0</c> to positive values.</para>
        /// </summary>
        /// <param name="numLines">The number of lines to normalize for.</param>
        /// <exception cref="OptionsException">Thrown if start and end cannot be normalized for the specified number of lines.</exception>
        public (int normalizedStart, int normalizedEnd) GetNormalizedStartAndEnd(int numLines)
        {
            int normalizedStart = Start > 0 ? Start : numLines + Start + 1;
            int normalizedEnd = End > 0 ? End : End == 0 ? numLines : numLines + End + 1;

            if (normalizedStart < 1 || normalizedStart > numLines ||
                normalizedEnd < normalizedStart || normalizedEnd > numLines)
            {
                throw new OptionsException(string.Format(Strings.OptionsException_LineRange_UnableToNormalize, ToString(),
                    numLines, normalizedStart, normalizedEnd));
            }

            return (normalizedStart, normalizedEnd);
        }

        /// <summary>
        /// Gets the position of the <see cref="LineRange"/> relative to a line in a sequence of lines.
        /// </summary>
        /// <param name="lineNumber">The line number of the line to find the <see cref="LineRange"/>'s position relative to.</param>
        /// <param name="numLines">The number of lines in the sequence of lines.</param>
        /// <returns>
        /// -1 if the <see cref="LineRange"/> occurs before the line.
        /// 0 if the <see cref="LineRange"/> contains the line.
        /// 1 if the <see cref="LineRange"/> occurs after the line.
        /// </returns>
        /// <exception cref="OptionsException">Thrown if start and end cannot be normalized for the specified number of lines.</exception>
        public int GetRelativePosition(int lineNumber, int numLines)
        {
            (int normalizedStart, int normalizedEnd) = GetNormalizedStartAndEnd(numLines);

            if (lineNumber < normalizedStart)
            {
                return 1;
            }

            if (lineNumber > normalizedEnd)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the string representation of the instance.
        /// </summary>
        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }

        /// <summary>
        /// Checks for value equality between the <see cref="LineRange"/> and an object.
        /// </summary>
        /// <param name="obj">The object to check for value equality.</param>
        /// <returns>True if the <see cref="LineRange"/>'s value is equal to the object's value, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is LineRange range &&
                   Start == range.Start &&
                   End == range.End;
        }

        /// <summary>
        /// Returns the hash code for the object.
        /// </summary>
        /// <returns>The hash code for the object.</returns>
        public override int GetHashCode()
        {
            int hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            return hashCode * -1521134295 + End.GetHashCode();
        }
    }
}
