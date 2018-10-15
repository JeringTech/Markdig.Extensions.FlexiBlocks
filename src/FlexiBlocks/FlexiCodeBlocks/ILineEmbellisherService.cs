using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// An abstraction for embellishing lines with markup elements that facilitate styling.
    /// </summary>
    public interface ILineEmbellisherService
    {
        /// <summary>
        /// Embellishes lines in a block of text.
        /// </summary>
        /// <param name="text">Text containing lines to embellish.</param>
        /// <param name="lineNumberRanges">Ranges of lines to add line numbers for and line numbers to use for each line. 
        /// If null, line numbers will not be added.</param>
        /// <param name="highlightLineRanges">Ranges of lines to highlight. If null, no lines will be highlighted.</param>
        /// <param name="prefixForClasses">Optional prefix for classes.</param>
        /// <param name="hiddenLinesIconMarkup">The markup for the icon representing hidden lines.</param>
        /// <returns><paramref name="text"/> with embellished lines.</returns>
        string EmbellishLines(string text, IEnumerable<LineNumberRange> lineNumberRanges, IEnumerable<LineRange> highlightLineRanges, string prefixForClasses = null, string hiddenLinesIconMarkup = null);
    }
}
