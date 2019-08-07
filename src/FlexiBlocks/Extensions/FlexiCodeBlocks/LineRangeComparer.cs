using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// A comparer for <see cref="LineRange"/>s.
    /// </summary>
    public class LineRangeComparer : IComparer<LineRange>
    {
        private readonly int _numLines;

        /// <summary>
        /// Creates a <see cref="LineRangeComparer"/>.
        /// </summary>
        /// <param name="numLines">Number of lines in the sequence of lines the <see cref="LineRange"/>s will be applied to.</param>
        public LineRangeComparer(int numLines)
        {
            _numLines = numLines;
        }

        /// <summary>
        /// Compares two <see cref="LineRange"/>s.
        /// </summary>
        /// <param name="x">The first <see cref="LineRange"/> to compare.</param>
        /// <param name="y">The second <see cref="LineRange"/> to compare.</param>
        /// <returns>
        /// -1 if the first <see cref="LineRange"/> occurs before the second <see cref="LineRange"/>.
        /// 0 if the <see cref="LineRange"/>s occur over the exact same lines.
        /// -1 if the first <see cref="LineRange"/> occurs after the second <see cref="LineRange"/>.
        /// </returns>
        public int Compare(LineRange x, LineRange y)
        {
            (int xStart, int xEnd) = x.GetNormalizedStartAndEnd(_numLines);
            (int yStart, int yEnd) = y.GetNormalizedStartAndEnd(_numLines);

            if (xStart < yStart)
            {
                return -1;
            }

            if (xStart > yStart ||
                xEnd < yEnd) // If two line ranges start at the same index, we want the longer line range ordered before the shorter one so we can skip the shorter one when rendering
            {
                return 1;
            }

            if (xEnd > yEnd)
            {
                return -1;
            }

            return 0;
        }
    }
}
