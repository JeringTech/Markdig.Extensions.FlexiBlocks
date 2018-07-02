using FlexiBlocks.FlexiOptionBlocks;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace FlexiBlocks.FlexiSectionBlocks
{
    /// <summary>
    /// Wraps sections demarcated by ATX headings in sectioning content elements, as recommended by the HTML spec - https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections.
    /// </summary>
    public class FlexiSectionBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiSectionBlocksExtensionOptions _options;

        public FlexiSectionBlocksExtension(FlexiSectionBlocksExtensionOptions options)
        {
            _options = options ?? new FlexiSectionBlocksExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<FlexiSectionBlockParser>())
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
                var jsonOptionsService = new FlexiOptionBlocksService();
                var sectionBlockParser = new FlexiSectionBlockParser(_options, headingBlockParser, autoLinkService, identifierService, jsonOptionsService);
                pipeline.BlockParsers.Insert(0, sectionBlockParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiSectionBlockRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiSectionBlockRenderer());
                }

                if (!htmlRenderer.ObjectRenderers.Contains<FlexiSectionHeaderBlockRenderer>())
                {
                    HeadingRenderer headingRenderer = htmlRenderer.ObjectRenderers.Find<HeadingRenderer>();
                    if(headingRenderer != null)
                    {
                        htmlRenderer.ObjectRenderers.Remove(headingRenderer);
                    }

                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiSectionHeaderBlockRenderer());
                }
            }
        }
    }
}
