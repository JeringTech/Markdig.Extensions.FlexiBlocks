namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// An abstraction for <see cref="FlexiSectionBlock"/> options.
    /// </summary>
    public interface IFlexiSectionBlockOptions : IRenderedBlockOptions<IFlexiSectionBlockOptions>
    {
        /// <summary>
        /// Gets the <see cref="FlexiSectionBlock"/>'s root element's type.
        /// </summary>
        SectioningContentElement Element { get; }

        /// <summary>
        /// Gets the value specifying whether or not to generate an ID for the <see cref="FlexiSectionBlock"/>.
        /// </summary>
        bool GenerateID { get; }

        /// <summary>
        /// Gets the <see cref="FlexiSectionBlock" />'s link icon as an HTML fragment.
        /// </summary>
        string LinkIcon { get; }

        /// <summary>
        /// Gets the value specifying whether or not the <see cref="FlexiSectionBlock"/> is auto-linkable.
        /// </summary>
        bool AutoLinkable { get; }

        /// <summary>
        /// Gets the <see cref="FlexiSectionBlock"/>'s rendering mode.
        /// </summary>
        FlexiSectionBlockRenderingMode RenderingMode { get; }
    }
}