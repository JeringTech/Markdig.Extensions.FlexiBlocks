namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// Sectioning content elements - https://html.spec.whatwg.org/multipage/dom.html#sectioning-content
    /// </summary>
    public enum SectioningContentElement
    {
        /// <summary>
        /// https://html.spec.whatwg.org/multipage/sections.html#the-section-element
        /// </summary>
        Section,

        /// <summary>
        /// https://html.spec.whatwg.org/multipage/sections.html#the-article-element
        /// </summary>
        Article,

        /// <summary>
        /// https://html.spec.whatwg.org/multipage/sections.html#the-aside-element
        /// </summary>
        Aside,

        /// <summary>
        /// https://html.spec.whatwg.org/multipage/sections.html#the-nav-element
        /// </summary>
        Nav
    }
}