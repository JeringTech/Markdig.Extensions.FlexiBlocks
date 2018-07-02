using Markdig;
using Markdig.Renderers;

namespace FlexiBlocks.JsonOptions
{
    public class FlexiOptionsExtension : IMarkdownExtension
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
