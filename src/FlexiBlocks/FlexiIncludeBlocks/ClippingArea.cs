namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// Represents a clipping from a text document.
    /// </summary>
    public abstract class ClippingArea
    {
        public abstract LineRange GetLineRange();

        public int DedentLength { get; set; } = -1;

        public int CollapseLength { get; set; } = -1;

        public string Before { get; set; }

        public string After { get; set; }
    }
}
