using System;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// An abstraction for embellishing lines of a markup fragment with elements that facilitate per-line styling.
    /// </summary>
    public interface ILineEmbellisherService
    {
        /// <summary>
        /// Embellishes lines in a markup fragment with elements that facilitate per-line styling.
        /// </summary>
        /// <param name="markupFragment">
        /// <para>The markup fragment containing the lines to embellish.</para>
        /// <para>This value must be a valid markup fragment that does not contain any void elements (https://www.w3.org/TR/html5/syntax.html#void-elements). If 
        /// either of these requirements are not met, an exception might be thrown or the resulting markup may be invalid.</para>
        /// </param>
        /// <param name="lineNumberLineRanges">The <see cref="NumberedLineRange"/>s that specify the line number to render for each line of code</param>
        /// <param name="highlightLineRanges">Ranges of lines to highlight. If null, no lines will be highlighted.</param>
        /// <param name="prefixForClasses">Optional prefix for classes.</param>
        /// <param name="hiddenLinesIconMarkup">The markup for the icon representing hidden lines.</param>
        /// <param name="splitMultilineElements">
        /// <para>The boolean value specifying whether or not multi-line elements should be split.</para>
        /// <para><paramref name="markupFragment"/> may contain multi-line elements. For example, Prism might highlight a comment spanning multiple lines like this:</para>
        /// <para>&lt;span class="token comment"&gt;/* This is a multi-</para>
        /// <para>line comment */&lt;/span&gt;</para>
        /// <para>If we apply line embellishments to the lines as is, we get the following defective outcome:</para>
        /// <para>&lt;span class="line"&gt;&lt;span class="line-number"&gt;1&lt;/span&gt;&lt;span class="line-text"&gt;&lt;span class="token comment"&gt;/* This is a multi-&lt;/span&gt;&lt;/span&gt;</para>
        /// <para>&lt;span class="line"&gt;&lt;span class="line-number"&gt;1&lt;/span&gt;&lt;span class="line-text"&gt;line comment */&lt;/span&gt;&lt;/span&gt;&lt;/span&gt;</para>
        /// <para>To remedy this issue, we must split multi-line elements before applying line embellishments:</para>
        /// <para>&lt;span class="token comment"&gt;/* This is a multi-&lt;/span&gt;</para>
        /// <para>&lt;span class="token comment"&gt;line comment */&lt;/span&gt;</para>
        /// <para>Splitting multi-line elements is unecessary if <paramref name="markupFragment" /> does not contain any multi-line markup elements.</para>
        /// </param>
        /// <returns><paramref name="markupFragment"/> with embellished lines.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="markupFragment"/> is invalid or contains void elements.</exception>
        string EmbellishLines(string markupFragment,
            IEnumerable<NumberedLineRange> lineNumberLineRanges,
            IEnumerable<LineRange> highlightLineRanges,
            string prefixForClasses = null,
            string hiddenLinesIconMarkup = null,
            bool splitMultilineElements = false);
    }
}
