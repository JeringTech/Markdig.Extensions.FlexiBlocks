using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks
{
    /// <summary>
    /// A markdig extension for <see cref="IncludeBlock"/>s.
    /// </summary>
    public class IncludeBlocksExtension : BlockExtension<IncludeBlock>
    {
        /// <summary>
        /// Creates an <see cref="IncludeBlocksExtension"/>.
        /// </summary>
        /// <param name="includeBlockParser">The <see cref="ProxyBlockParser{TMain, TProxy}"/> for creating <see cref="IncludeBlock"/>s from markdown.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="includeBlockParser"/> is <c>null</c>.</exception>
        public IncludeBlocksExtension(ProxyBlockParser<IncludeBlock, ProxyJsonBlock> includeBlockParser) : base(null, includeBlockParser)
        {
            if (includeBlockParser == null)
            {
                throw new ArgumentNullException(nameof(includeBlockParser));
            }
        }
    }
}
