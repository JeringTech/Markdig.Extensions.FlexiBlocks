using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a clipping from a sequence of lines.
    /// </summary>
    public class Clipping
    {
        /// <summary>
        /// Creates a <see cref="Clipping"/> instance. Validates arguments.
        /// </summary>
        /// <param name="startLineNumber">
        /// <para>The line number of the line that this clipping starts at.</para>
        /// <para>This value must be greater than 0.</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <param name="endLineNumber">
        /// <para>The line number of the line that this clipping ends at.</para>
        /// <para>If this value is -1, this clipping extends to the last line. If it is not -1, it must be greater than or equal to <paramref name="startLineNumber"/>.</para>
        /// <para>Defaults to -1.</para>
        /// </param>
        /// <param name="startDemarcationLineSubstring">
        /// <para>A substring that the line immediately preceding this clipping contains.</para>
        /// <para>If this value is not null, whitespace or an empty string, it takes precedence over <paramref name="startLineNumber"/>.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="endDemarcationLineSubstring">
        /// <para>A substring that the line immediately after this clipping contains.</para>
        /// <para>If this value is not null, whitespace or an empty string, it takes precedence over <paramref name="endLineNumber"/>.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="dedentLength">
        /// <para>The number of leading whitespace characters to remove from each line in this clipping.</para>
        /// <para>This value must not be negative.</para>
        /// <para>Defaults to 0.</para>
        /// </param>
        /// <param name="collapseRatio">
        /// <para>The proportion of leading whitespace characters (after dedenting) to keep.</para>
        /// <para>For example, if there are 9 leading whitespace characters after dedenting, and this value is 0.33, the final number of leading whitespace characters will be 3.</para> 
        /// <para>This value must be in the range [0, 1].</para>
        /// <para>Defaults to 1.</para>
        /// </param>
        /// <param name="beforeContent">
        /// <para>The content to be prepended to this clipping.</para>
        /// <para>This value will be processed as markdown if the <see cref="FlexiIncludeBlock"/> that this clipping belongs to has Markdown as its content type.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <param name="afterContent">
        /// <para>The content to be appended to this clipping.</para>
        /// <para>This value will be processed as markdown if the <see cref="FlexiIncludeBlock"/> that this clipping belongs to has Markdown as its content type.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="startLineNumber"/> is less than 1.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="endLineNumber"/> is not -1 and is less than <paramref name="startLineNumber"/>.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="dedentLength"/> is negative.</exception>
        /// <exception cref="FlexiBlocksException">Thrown if <paramref name="collapseRatio"/> is not in the range [0, 1].</exception>
        public Clipping(int startLineNumber = 1,
            int endLineNumber = -1,
            string startDemarcationLineSubstring = null,
            string endDemarcationLineSubstring = null,
            int dedentLength = 0,
            float collapseRatio = 1,
            string beforeContent = null,
            string afterContent = null)
        {
            if(startLineNumber < 1)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeGreaterThan0, nameof(StartLineNumber), startLineNumber));
            }

            if (endLineNumber != -1 && endLineNumber < startLineNumber)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber, nameof(EndLineNumber), endLineNumber, startLineNumber));
            }

            if(dedentLength < 0)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeGreaterThan0, nameof(DedentLength), dedentLength));
            }

            if (collapseRatio < 0 || collapseRatio > 1)
            {
                throw new FlexiBlocksException(string.Format(Strings.FlexiBlocksException_OptionMustBeWithinRange, nameof(CollapseRatio), "[0, 1]", collapseRatio));
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
        /// Gets the line number of the line that this clipping starts at.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
        public int StartLineNumber { get; }

        /// <summary>
        /// Gets the line number of the line that this clipping ends at.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(-1)]
        public int EndLineNumber { get; }

        /// <summary>
        /// Gets a substring that the line immediately preceding this clipping contains.
        /// </summary>
        public string StartDemarcationLineSubstring { get; }

        /// <summary>
        /// Gets a substring that the line immediately after this clipping contains.
        /// </summary>
        public string EndDemarcationLineSubstring { get; }

        /// <summary>
        /// Gets the number of leading whitespace characters to remove from each line in this clipping.
        /// </summary>
        public int DedentLength { get; }

        /// <summary>
        /// Gets the proportion of leading whitespace characters (after dedenting) to keep.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(1)]
        public float CollapseRatio { get; }

        /// <summary>
        /// Gets the content to be prepended to this clipping.
        /// </summary>
        public string BeforeContent { get; }

        /// <summary>
        /// Gets the content to be appended to this clipping.
        /// </summary>
        public string AfterContent { get; }

        /// <summary>
        /// Checks for value equality between this <see cref="Clipping"/> and an object.
        /// </summary>
        /// <param name="obj">The object to check for value equality.</param>
        /// <returns>True if this <see cref="Clipping"/>'s value is equal to the object's value, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Clipping otherClipping))
            {
                return false;
            }

            return StartLineNumber == otherClipping.StartLineNumber &&
                EndLineNumber == otherClipping.EndLineNumber &&
                StartDemarcationLineSubstring == otherClipping.StartDemarcationLineSubstring &&
                EndDemarcationLineSubstring == otherClipping.EndDemarcationLineSubstring &&
                DedentLength == otherClipping.DedentLength &&
                CollapseRatio == otherClipping.CollapseRatio &&
                BeforeContent == otherClipping.BeforeContent &&
                AfterContent == otherClipping.AfterContent;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            int hashCode = -417293665;
            hashCode = hashCode * -1521134295 + StartLineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EndLineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartDemarcationLineSubstring);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EndDemarcationLineSubstring);
            hashCode = hashCode * -1521134295 + DedentLength.GetHashCode();
            hashCode = hashCode * -1521134295 + CollapseRatio.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BeforeContent);
            return hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AfterContent);
        }
    }
}
