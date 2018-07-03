using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// A <see cref="LeafBlock"/> that holds a JSON string representing options for the following block.
    /// </summary>
    public class FlexiOptionsBlock : LeafBlock
    {
        public FlexiOptionsBlock(FlexiOptionsBlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// Number of open brackets in the JSON, used to determine when to stop parsing.
        /// </summary>
        public int NumOpenBrackets { get; set; } = 0;

        /// <summary>
        /// True if the JSON parsed so far ends within a string, for example if the JSON parsed so far is "{ \"part". False otherwise.
        /// </summary>
        public bool EndsInString { get; set; } = false;


        /// <summary>
        /// Gets or sets the line that this block ends at.
        /// </summary>
        public int EndLine { get; set; }
    }
}
