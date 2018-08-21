using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a clipping from some content.
    /// </summary>
    public class ClippingArea
    {
        /// <summary>
        /// Creates a <see cref="ClippingArea"/> instance. Validates arguments.
        /// </summary>
        /// <param name="startLineNumber">The line number that the clipping starts at. Must be larger than or equal to 1.</param>
        /// <param name="endLineNumber">The line number that the clipping ends at. If this argument is -1, the clipping extends to the end of the content. Otherwise, this argument must be larger than <paramref name="startLineNumber"/>.</param>
        /// <param name="startDemarcationLineSubstring">The substring that the line immediately preceding the clipping contains. This value overrides <paramref name="startLineNumber"/>.</param>
        /// <param name="endDemarcationLineSubstring">The substring that the line immediately after the clipping contains. This value overrides <paramref name="endLineNumber"/>.</param>
        /// <param name="dedentLength">The number of leading white space characters to remove from each line in the clipping. This argument must not be negative.</param>
        /// <param name="collapseRatio">The ratio of the number of leading whitespace characters before collapsing to the number of leading whitespace characters after collapsing. This argument must be larger than 0.</param>
        /// <param name="beforeText">The text to be prepended to the clipping.</param>
        /// <param name="afterText">The text to be appended to the clipping.</param>
        /// <exception cref="ArgumentException">Thrown if neither <paramref name="startLineNumber"/> nor <paramref name="startDemarcationLineSubstring"/> is defined.</exception>
        /// <exception cref="ArgumentException">Thrown if both <paramref name="startLineNumber"/> and <paramref name="startDemarcationLineSubstring"/> are defined.</exception>
        /// <exception cref="ArgumentException">Thrown if neither <paramref name="endLineNumber"/> nor <paramref name="endDemarcationLineSubstring"/> are defined.</exception>
        /// <exception cref="ArgumentException">Thrown if both <paramref name="endLineNumber"/> and <paramref name="endDemarcationLineSubstring"/> are defined.</exception>
        /// <exception cref="ArgumentException">Thrown if both <paramref name="startLineNumber"/> and <paramref name="endLineNumber"/> are defined, <paramref name="endLineNumber"/> is not -1,
        /// but <paramref name="endLineNumber"/> is less than <paramref name="startLineNumber"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="dedentLength"/> is negative.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collapseRatio"/> is less than 1.</exception>
        public ClippingArea(int startLineNumber, int endLineNumber,
            string startDemarcationLineSubstring = null, string endDemarcationLineSubstring = null,
            int dedentLength = 0, int collapseRatio = 1,
            string beforeText = null, string afterText = null)
        {
            bool startLineSubstringDefined = !string.IsNullOrWhiteSpace(startDemarcationLineSubstring);
            bool startLineNumberDefined = startLineNumber >= 1;
            if (!startLineSubstringDefined && !startLineNumberDefined || startLineSubstringDefined && startLineNumberDefined)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_OneAndOnlyOneArgumentMustBeDefined,
                    nameof(startDemarcationLineSubstring),
                    nameof(startLineNumber)));
            }

            bool endLineSubstringDefined = !string.IsNullOrWhiteSpace(endDemarcationLineSubstring);
            bool endLineNumberDefined = endLineNumber != 0;
            if(!endLineSubstringDefined && !endLineNumberDefined || endLineSubstringDefined && endLineNumberDefined)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_OneAndOnlyOneArgumentMustBeDefined,
                    nameof(endDemarcationLineSubstring),
                    nameof(endLineNumber)));
            }

            if(startLineNumberDefined && endLineNumberDefined && endLineNumber != -1 && endLineNumber < startLineNumber)
            {
                throw new ArgumentException(Strings.ArgumentException_EndLineNumberMustNotBeLessThanStartLineNumber);
            }

            if(dedentLength < 0)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ArgumentMustNotBeNegative, nameof(dedentLength)));
            }

            if (collapseRatio < 1)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ArgumentMustBeLargerThan0, nameof(collapseRatio)));
            }

            StartLineNumber = startLineNumber;
            EndLineNumber = endLineNumber;
            StartDemarcationLineSubstring = startDemarcationLineSubstring;
            EndDemarcationLineSubstring = endDemarcationLineSubstring;
            DedentLength = dedentLength;
            CollapseRatio = collapseRatio;
            BeforeText = beforeText;
            AfterText = afterText;
        }

        /// <summary>
        /// Gets the line number that the clipping starts at. If <see cref="StartDemarcationLineSubstring"/> is defined, this value will not be valid.
        /// </summary>
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the line number that the clipping ends at. If <see cref="EndDemarcationLineSubstring"/> is defined, this valid will not be valid.
        /// </summary>
        public int EndLineNumber { get; }

        /// <summary>
        /// Gets the substring that the line immediately preceding the clipping contains. If this value is not defined, the first line of the clipping will be specified by
        /// <see cref="StartLineNumber"/>.
        /// </summary>
        public string StartDemarcationLineSubstring { get; }

        /// <summary>
        /// Gets the substring that the line immediately after the clipping contains. If this value is not defined, the last line of the clipping will be specified by
        /// <see cref="EndLineNumber"/>.
        /// </summary>
        public string EndDemarcationLineSubstring { get; }

        /// <summary>
        /// Gets the number of leading white space characters to remove from each line in the clipping.
        /// </summary>
        public int DedentLength { get; }

        /// <summary>
        /// Gets the ratio of the number of leading whitespace characters before collapsing to the number of leading whitespace characters after collapsing.
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] // If CollapseRatio isn't specified in JSON, the constructor arg collapseRatio is set to 1
        public int CollapseRatio { get; }

        /// <summary>
        /// Gets the text to be prepended to the clipping.
        /// </summary>
        public string BeforeText { get; }

        /// <summary>
        /// Gets the text to be appended to the clipping.
        /// </summary>
        public string AfterText { get; }
    }
}
