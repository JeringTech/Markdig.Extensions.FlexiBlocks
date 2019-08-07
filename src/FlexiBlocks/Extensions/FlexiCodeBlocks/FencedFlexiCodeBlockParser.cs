using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks
{
    /// <summary>
    /// A parser that parses fenced <see cref="FlexiCodeBlock"/>s in markdown.
    /// </summary>
    public class FencedFlexiCodeBlockParser : FencedBlockParser<FlexiCodeBlock, ProxyFencedLeafBlock>
    {
        /// <summary>
        /// Creates a <see cref="FencedFlexiCodeBlockParser"/>.
        /// </summary>
        /// <param name="flexiCodeBlockFactory">The factory for creating <see cref="FlexiCodeBlock"/>s.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="flexiCodeBlockFactory"/> is <c>null</c>.</exception>
        public FencedFlexiCodeBlockParser(IFlexiCodeBlockFactory flexiCodeBlockFactory) : base(flexiCodeBlockFactory)
        {
            OpeningCharacters = new[] { '`', '~' };
        }
    }
}
