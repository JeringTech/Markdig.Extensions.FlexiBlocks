using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents a proxy fenced <see cref="LeafBlock"/>.
    /// </summary>
    public class ProxyFencedLeafBlock : ProxyLeafBlock, IProxyFencedBlock
    {
        /// <summary>
        /// Creates a <see cref="ProxyFencedLeafBlock"/>.
        /// </summary>
        /// <param name="openingFenceIndent">The indent of the fenced <see cref="LeafBlock"/>'s opening fence.</param>
        /// <param name="openingFenceCharCount">The number of characters in the fenced <see cref="LeafBlock"/>'s opening fence.</param>
        /// <param name="fenceChar">The character used in the fenced <see cref="LeafBlock"/>'s fences.</param>
        /// <param name="mainTypeName">Type name of the fenced <see cref="LeafBlock"/> this <see cref="ProxyFencedLeafBlock"/> is proxying for.</param>
        /// <param name="blockParser">The <see cref="BlockParser"/> parsing the fenced <see cref="LeafBlock"/>.</param>
        public ProxyFencedLeafBlock(int openingFenceIndent, int openingFenceCharCount, char fenceChar, string mainTypeName, BlockParser blockParser) : base(mainTypeName, blockParser)
        {
            OpeningFenceIndent = openingFenceIndent;
            OpeningFenceCharCount = openingFenceCharCount;
            FenceChar = fenceChar;
            IsBreakable = false;
        }

        /// <inheritdoc />
        public int OpeningFenceIndent { get; }

        /// <inheritdoc />
        public int OpeningFenceCharCount { get; }

        /// <inheritdoc />
        public char FenceChar { get; }
    }
}
