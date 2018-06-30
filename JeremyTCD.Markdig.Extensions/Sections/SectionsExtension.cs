using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    /// <summary>
    /// Wraps sections demarcated by ATX headings in sectioning content elements, as recommended by the HTML spec - https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections.
    /// </summary>
    public class SectionsExtension : IMarkdownExtension
    {
        private readonly SectionsExtensionOptions _options;

        public SectionsExtension(SectionsExtensionOptions options)
        {
            _options = options ?? new SectionsExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<SectionBlockParser>())
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

                // TODO For testability - could improve IOC infrastructure, measure impact on performance
                var autoLinkService = new AutoLinkService();
                var identifierService = new IdentifierService();
                var jsonOptionsService = new JsonOptionsService();
                var sectionBlockParser = new SectionBlockParser(_options, headingBlockParser, autoLinkService, identifierService, jsonOptionsService);
                pipeline.BlockParsers.Insert(0, sectionBlockParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<SectionBlockRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, new SectionBlockRenderer());
                }

                if (!htmlRenderer.ObjectRenderers.Contains<SectionHeaderRenderer>())
                {
                    HeadingRenderer headingRenderer = htmlRenderer.ObjectRenderers.Find<HeadingRenderer>();
                    if(headingRenderer != null)
                    {
                        htmlRenderer.ObjectRenderers.Remove(headingRenderer);
                    }

                    htmlRenderer.ObjectRenderers.Insert(0, new SectionHeaderRenderer());
                }
            }
        }
    }
}
