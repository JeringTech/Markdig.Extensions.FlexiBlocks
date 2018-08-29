using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// A <see cref="LeafBlock"/> that holds a JSON string representing options for the following block.
    /// </summary>
    public class FlexiOptionsBlock : JsonBlock
    {
        public FlexiOptionsBlock(FlexiOptionsBlockParser parser) : base(parser)
        {
        }
    }
}
