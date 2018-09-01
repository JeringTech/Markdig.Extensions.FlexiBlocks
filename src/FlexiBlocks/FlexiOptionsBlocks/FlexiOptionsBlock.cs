namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// Represents a block containing options for the following block.
    /// </summary>
    public class FlexiOptionsBlock : JsonBlock
    {
        /// <summary>
        /// Creates a <see cref="FlexiOptionsBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this block.</param>
        public FlexiOptionsBlock(FlexiOptionsBlockParser parser) : base(parser)
        {
        }
    }
}
