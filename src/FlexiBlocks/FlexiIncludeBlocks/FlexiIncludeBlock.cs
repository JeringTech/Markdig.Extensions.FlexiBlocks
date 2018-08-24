using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a flexible include block.
    /// </summary>
    public class FlexiIncludeBlock : LeafBlock
    {
        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser opening this instance.</param>
        public FlexiIncludeBlock(FlexiIncludeBlockParser parser) : base(parser)
        {
        }

        public ProcessingStage ProcessingStage { get; set; }

        /// <summary>
        /// Gets or sets the source of the content that this FlexiIncludeBlock includes.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the line number in the source, of the last processed line. This value allows child 
        /// FlexiIncludeBlocks to determine their line numbers in the source.
        /// </summary>
        public int LineNumberOfLastProcessedLineInSource { get; set; }

        /// <summary>
        /// Gets or sets the source that this FlexiIncludeBlock occurs in.
        /// </summary>
        public string ContainingSource { get; set; }

        /// <summary>
        /// Gets or sets this FlexiIncludeBlock's line number in the source that it occurs in.
        /// </summary>
        public int LineNumberInContainingSource { get; set; }

        /// <summary>
        /// Number of open brackets in the JSON, used to determine when to stop parsing.
        /// </summary>
        public int NumOpenBrackets { get; set; } = 0;

        /// <summary>
        /// True if the JSON parsed so far ends within a string, for example if the JSON parsed so far is "{ \"part". False otherwise.
        /// </summary>
        public bool EndsInString { get; set; }
    }
}
