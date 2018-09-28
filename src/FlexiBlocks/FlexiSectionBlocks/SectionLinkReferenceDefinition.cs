using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// Represents a <see cref="LinkReferenceDefinition"/> for a <see cref="FlexiSectionBlock"/>.
    /// </summary>
    public class SectionLinkReferenceDefinition : LinkReferenceDefinition
    {
        /// <summary>
        /// Gets or sets the <see cref="FlexiSectionBlock"/> that this link reference definition applies to.
        /// </summary>
        public FlexiSectionBlock FlexiSectionBlock { get; set; }
    }
}
