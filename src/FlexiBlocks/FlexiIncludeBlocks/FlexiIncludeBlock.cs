using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class FlexiIncludeBlock : LeafBlock
    {
        public FlexiIncludeBlock(FlexiIncludeBlockParser parser) : base(parser)
        {
        }

        public string Source { get; set; }

        public int LineNumberOfLastProcessedLineInSource { get; set; }

        public string ContainingSource { get; set; }

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
