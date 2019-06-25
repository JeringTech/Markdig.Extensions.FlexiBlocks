using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// A parser that parses <see cref="IncludeBlock"/>s from markdown.
    /// </summary>
    public class IncludeBlockParser : JsonBlockParser<IncludeBlock, ProxyJsonBlock>
    {
        /// <summary>
        /// Creates an <see cref="IncludeBlockParser"/>.
        /// </summary>
        /// <param name="includeBlockFactory">The factory for building <see cref="IncludeBlock"/>s.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="includeBlockFactory"/> is <c>null</c>.</exception>
        public IncludeBlockParser(IJsonBlockFactory<IncludeBlock, ProxyJsonBlock> includeBlockFactory) : base(includeBlockFactory)
        {
            OpeningCharacters = new[] { '+' };
        }
    }
}
