using Markdig.Parsers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks
{
    /// <summary>
    /// Represents a block containing options for another block.
    /// </summary>
    public class OptionsBlock : LeafBlock
    {
        /// <summary>
        /// Creates an <see cref="OptionsBlock"/>.
        /// </summary>
        /// <param name="blockParser">The <see cref="BlockParser"/> parsing the <see cref="OptionsBlock"/>.</param>
        public OptionsBlock(BlockParser blockParser) : base(blockParser)
        {
        }
    }
}
