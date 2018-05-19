using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    /// <summary>
    /// Wraps sections demarcated by ATX headings in sectioning content elements, as recommended by the HTML spec - https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections.
    /// </summary>
    public class SectionsExtension : IMarkdownExtension
    {
        private readonly SectionsOptions _options;

        public SectionsExtension(SectionsOptions options)
        {
            _options = options ?? new SectionsOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<SectionsParser>())
            {
                HeadingBlockParser headingBlockParser = pipeline.BlockParsers.Find<HeadingBlockParser>();
                if (headingBlockParser != null)
                {
                    pipeline.BlockParsers.Remove(headingBlockParser);
                }
                else
                {
                    headingBlockParser = new HeadingBlockParser();
                }

                // For testability - could improve IOC infrastructure, measure impact on performance
                var autoLinkService = new AutoLinkService();
                var identifierService = new IdentifierService();
                var jsonOptionsService = new JsonOptionsService();
                var sectionsParser = new SectionsParser(_options, headingBlockParser, autoLinkService, identifierService, jsonOptionsService);
                pipeline.BlockParsers.Insert(0, sectionsParser);
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
