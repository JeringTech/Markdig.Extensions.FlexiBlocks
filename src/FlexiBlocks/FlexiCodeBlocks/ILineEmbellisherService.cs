using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// An abstraction for adding line numbers and highlighting lines.
    /// </summary>
    public interface ILineEmbellisherService
    {
        /// <summary>
        /// Adds line numbers and highlights lines in a block of text.
        /// </summary>
        /// <param name="text">Text containing lines to embellish.</param>
        /// <param name="lineNumberRanges">Ranges of lines to add line numbers for and line numbers to use for each line. If null, line numbers will not be added.</param>
        /// <param name="highlightLineRanges">Ranges of lines to highlight. If null, no lines will be highlighted.</param>
        /// <param name="prefixForClasses">Optional prefix for classes.</param>
        /// <returns><paramref name="text"/> with line numbers and highlighted lines.</returns>
        string EmbellishLines(string text, IEnumerable<LineNumberRange> lineNumberRanges, IEnumerable<LineRange> highlightLineRanges, string prefixForClasses = null);
    }
}
