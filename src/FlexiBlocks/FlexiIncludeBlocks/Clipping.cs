using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a clipping from text content.
    /// </summary>
    public class Clipping
    {
        /// <summary>
        /// Creates a <see cref="Clipping"/> instance. Validates arguments.
        /// </summary>
        /// <param name="startLineNumber">The line number that the clipping starts at. Defaults to 1. <paramref name="startDemarcationLineSubstring"/> takes precedence if it is defined. </param>
        /// <param name="endLineNumber">The line number that the clipping ends at. If this argument is -1, the clipping extends to the end of the content.
        /// Otherwise, this argument must be larger than <paramref name="startLineNumber"/>. Defaults to -1. <paramref name="endDemarcationLineSubstring"/> takes precedence if it is defined.</param>
        /// <param name="startDemarcationLineSubstring">The substring that the line immediately preceding the clipping contains. If defined, takes precedence over <paramref name="startLineNumber"/>.</param>
        /// <param name="endDemarcationLineSubstring">The substring that the line immediately after the clipping contains. If defined, takes <paramref name="endLineNumber"/>.</param>
        /// <param name="dedentLength">The number of leading white space characters to remove from each line in the clipping. This argument must not be negative. Defaults to 0.</param>
        /// <param name="collapseRatio">The proportion of leading whitespace characters to keep. For example, if there are intially 9 leading white space characters and this argument is 0.33,
        /// the final number of leading white space characters will be 3. This argument must be in the interval [0, 1]. Defaults to 1.</param>
        /// <param name="beforeText">The text to be prepended to the clipping.</param>
        /// <param name="afterText">The text to be appended to the clipping.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="dedentLength"/> is negative.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collapseRatio"/> is not in interval [0, 1].</exception>
        public Clipping(int startLineNumber = 1, int endLineNumber = -1,
            string startDemarcationLineSubstring = null, string endDemarcationLineSubstring = null,
            int dedentLength = 0, float collapseRatio = 1,
            string beforeText = null, string afterText = null)
        {
            if(startLineNumber < 1)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ArgumentMustBeLargerThan0, nameof(startLineNumber)));
            }

            if (endLineNumber != -1 && endLineNumber < startLineNumber)
            {
                throw new ArgumentException(Strings.ArgumentException_EndLineNumberMustNotBeLessThanStartLineNumber);
            }

            if(dedentLength < 0)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ArgumentMustNotBeNegative, nameof(dedentLength)));
            }

            if (collapseRatio < 0 || collapseRatio > 1)
            {
                throw new ArgumentException(string.Format(Strings.ArgumentException_ArgumentMustBeInInterval, nameof(collapseRatio), "[0, 1]"));
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
        /// Gets the line number that the clipping starts at. If <see cref="StartDemarcationLineSubstring"/> is defined, it takes precedence over this value.
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the line number that the clipping ends at. If <see cref="EndDemarcationLineSubstring"/> is defined, it takes precedence over this value.
        /// </summary>
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
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
        /// Gets the proportion of leading whitespace characters to keep.
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float CollapseRatio { get; }

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
