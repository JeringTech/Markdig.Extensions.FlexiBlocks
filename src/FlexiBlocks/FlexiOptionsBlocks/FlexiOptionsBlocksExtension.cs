using Markdig;
using Markdig.Renderers;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    public class FlexiOptionsBlocksExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<FlexiOptionsBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, new FlexiOptionsBlockParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // FlexiOptionsBlocks are never rendered
        }
    }
}
