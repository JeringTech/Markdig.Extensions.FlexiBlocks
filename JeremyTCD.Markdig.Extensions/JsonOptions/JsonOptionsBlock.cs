using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions
{
    public class JsonOptionsBlock : LeafBlock
    {
        public JsonOptionsBlock(JsonOptionsParser parser) : base(parser)
        {
        }

        public int NumOpenBrackets { get; set; } = 0;
        public bool InString { get; set; } = false;
    }
}
