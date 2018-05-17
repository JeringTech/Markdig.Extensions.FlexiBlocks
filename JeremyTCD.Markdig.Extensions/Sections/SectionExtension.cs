using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    /// <summary>
    /// Wraps sections demarcated by ATX headings in sectioning content elements, as recommended by the HTML spec - https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections.
    /// </summary>
    public class SectionExtension : IMarkdownExtension
    {
        private readonly SectionExtensionOptions _options;

        public SectionExtension(SectionExtensionOptions options)
        {
            _options = options ?? new SectionExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.BlockParsers.Contains<SectionsParser>())
            {
                HeadingBlockParser headingBlockParser = pipelineBuilder.BlockParsers.Find<HeadingBlockParser>();
                if (headingBlockParser != null)
                {
                    pipelineBuilder.BlockParsers.Remove(headingBlockParser);
                }

                var sectionsParser = new SectionsParser(_options);
                pipelineBuilder.BlockParsers.Insert(0, sectionsParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<SectionsRenderer>())
            {
                htmlRenderer.ObjectRenderers.Insert(0, new SectionsRenderer());
            }
        }
    }
}
