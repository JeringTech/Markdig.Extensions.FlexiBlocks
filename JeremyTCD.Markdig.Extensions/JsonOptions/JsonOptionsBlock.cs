using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.JsonOptions
{
    /// <summary>
    /// A <see cref="LeafBlock"/> that holds a JSON string to be consumed by a subsequent block.
    /// </summary>
    public class JsonOptionsBlock : LeafBlock
    {
        public JsonOptionsBlock(JsonOptionsParser parser) : base(parser)
        {
        }

        /// <summary>
        /// Number of open brackets in the JSON, used to determine when to stop parsing.
        /// </summary>
        public int NumOpenBrackets { get; set; } = 0;

        /// <summary>
        /// True if the JSON ends in a string, for example if the JSON is "{ \"Opti". False otherwise.
        /// </summary>
        public bool EndsInString { get; set; } = false;


        /// <summary>
        /// Gets or sets the line that this block ends at.
        /// </summary>
        public int EndLine { get; set; }
    }
}
