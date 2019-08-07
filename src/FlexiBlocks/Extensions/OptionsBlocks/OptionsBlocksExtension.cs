using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks
{
    /// <summary>
    /// A Markdig extension for <see cref="OptionsBlock"/>s.
    /// </summary>
    public class OptionsBlocksExtension : BlockExtension<OptionsBlock>
    {
        /// <summary>
        /// Creates an <see cref="OptionsBlocksExtension"/>.
        /// </summary>
        /// <param name="optionsBlockParser">The <see cref="ProxyBlockParser{TMain, TProxy}"/> for creating <see cref="OptionsBlock"/>s from markdown.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="optionsBlockParser"/> is <c>null</c>.</exception>
        public OptionsBlocksExtension(ProxyBlockParser<OptionsBlock, ProxyJsonBlock> optionsBlockParser) : base(null, optionsBlockParser)
        {
            if (optionsBlockParser == null)
            {
                throw new ArgumentNullException(nameof(optionsBlockParser));
            }
        }
    }
}
