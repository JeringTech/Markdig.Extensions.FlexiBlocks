namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// Represents the possible contexts of <see cref="FlexiBlocksException"/>s.
    /// </summary>
    public enum FlexiBlockExceptionContext
    {
        /// <summary>
        /// No context.
        /// </summary>
        None = 0,

        /// <summary>
        /// Line that caused the unrecoverable situation is known but the kind of block at the line is unknown.
        /// </summary>
        Line,

        /// <summary>
        /// Block that caused the unrecoverable situation is known.
        /// </summary>
        Block
    }
}
