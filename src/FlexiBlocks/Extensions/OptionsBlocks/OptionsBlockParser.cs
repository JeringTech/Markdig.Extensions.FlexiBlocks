using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks
{
    /// <summary>
    /// A parser that parses <see cref="OptionsBlock"/>s from markdown.
    /// </summary>
    public class OptionsBlockParser : JsonBlockParser<OptionsBlock, ProxyJsonBlock>
    {
        /// <summary>
        /// Creates an <see cref="OptionsBlockParser"/>.
        /// </summary>
        /// <param name="optionsBlockFactory">The factory for building <see cref="OptionsBlock"/>s.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="optionsBlockFactory"/> is <c>null</c>.</exception>
        public OptionsBlockParser(IJsonBlockFactory<OptionsBlock, ProxyJsonBlock> optionsBlockFactory) : base(optionsBlockFactory)
        {
            OpeningCharacters = new[] { '@' };
        }
    }
}
