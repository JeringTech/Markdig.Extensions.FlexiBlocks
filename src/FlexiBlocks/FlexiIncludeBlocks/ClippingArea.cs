namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a clipping from a text document.
    /// </summary>
    public class ClippingArea
    {
        // TODO will be created from json, how to handle optional parameters?
        public ClippingArea()
        {
            // TODO either StartLineNumber must be >= 1 or StartLine must be defined
            // TODO eitehr EndLineNumber must be >= StartLineNumber or EndLine must be defined
        }

        public int StartLineNumber { get; } = 1;

        public int EndLineNumber { get; } = -1;

        public string StartLineSubString { get; }

        public string EndLineSubString { get; }

        public int DedentLength { get; } = -1;

        public int CollapseRatio { get; } = -1;

        public string BeforeText { get; }

        public string AfterText { get; }
    }
}
