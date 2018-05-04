using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions
{
    /// <summary>
    /// Wraps implicit sections in <section> elements, as recommended by - https://html.spec.whatwg.org/multipage/sections.html#headings-and-sections
    /// </summary>
    public class SectionExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.BlockParsers.Contains<SectionParser>())
            {
                HeadingBlockParser headingBlockParser = pipelineBuilder.BlockParsers.Find<HeadingBlockParser>();
                if (headingBlockParser != null)
                {
                    pipelineBuilder.BlockParsers.Remove(headingBlockParser);
                }

                // Insert the parser before any other parsers
                pipelineBuilder.BlockParsers.Insert(0, new SectionParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<SectionRenderer>())
            {
                // Must be inserted before CodeBlockRenderer
                htmlRenderer.ObjectRenderers.Insert(0, new SectionRenderer());
            }
        }
    }
}
