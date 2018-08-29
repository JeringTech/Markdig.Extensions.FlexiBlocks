namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a flexible include block.
    /// </summary>
    public class FlexiIncludeBlock : JsonBlock
    {
        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlock"/> instance.
        /// </summary>
        /// <param name="parser">The parser for this instance.</param>
        public FlexiIncludeBlock(FlexiIncludeBlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// Gets the current processing stage.
        /// </summary>
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
    }
}
