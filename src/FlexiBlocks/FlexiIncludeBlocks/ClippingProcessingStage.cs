namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// <see cref="Clipping"/> processing stages.
    /// </summary>
    public enum ClippingProcessingStage
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Source.
        /// </summary>
        Source,

        /// <summary>
        /// Pre-source content.
        /// </summary>
        BeforeContent,

        /// <summary>
        /// Post-source content.
        /// </summary>
        AfterContent
    }
}
