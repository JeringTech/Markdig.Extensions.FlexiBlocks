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
        /// <param name="beforeContent">The content to be prepended to the clipping.</param>
        /// <param name="afterContent">The content to be appended to the clipping.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="dedentLength"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="collapseRatio"/> is not within range [0, 1].</exception>
        public Clipping(int startLineNumber = 1, int endLineNumber = -1,
            string startDemarcationLineSubstring = null, string endDemarcationLineSubstring = null,
            int dedentLength = 0, float collapseRatio = 1,
            string beforeContent = null, string afterContent = null)
        {
            if(startLineNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(startLineNumber),
                    string.Format(Strings.ArgumentOutOfRangeException_LineNumberMustBeGreaterThan0, startLineNumber));
            }

            if (endLineNumber != -1 && endLineNumber < startLineNumber)
            {
                throw new ArgumentOutOfRangeException(nameof(endLineNumber),
                    string.Format(Strings.ArgumentOutOfRangeException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, endLineNumber, startLineNumber));
            }

            if(dedentLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dedentLength),
                    string.Format(Strings.ArgumentOutOfRangeException_ValueCannotBeNegative, dedentLength));
            }

            if (collapseRatio < 0 || collapseRatio > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(collapseRatio),
                    string.Format(Strings.ArgumentOutOfRangeException_ValueMustBeWithinRange, "[0, 1]", collapseRatio));
            }

            StartLineNumber = startLineNumber;
            EndLineNumber = endLineNumber;
            StartDemarcationLineSubstring = startDemarcationLineSubstring;
            EndDemarcationLineSubstring = endDemarcationLineSubstring;
            DedentLength = dedentLength;
            CollapseRatio = collapseRatio;
            BeforeContent = beforeContent;
            AfterContent = afterContent;
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
        /// Gets the content to be prepended to the clipping.
        /// </summary>
        public string BeforeContent { get; }

        /// <summary>
        /// Gets the content to be appended to the clipping.
        /// </summary>
        public string AfterContent { get; }
    }
}
