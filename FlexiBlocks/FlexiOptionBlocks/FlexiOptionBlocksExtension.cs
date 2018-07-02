using Markdig;
using Markdig.Renderers;

namespace FlexiBlocks.FlexiOptionBlocks
{
    public class FlexiOptionBlocksExtension : IMarkdownExtension
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
            // JsonOptions are never rendered
        }
    }
}
