using Markdig;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions
{
    public class JsonOptionsExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<JsonOptionsParser>())
            {
                pipeline.BlockParsers.Insert(0, new JsonOptionsParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // JsonOptions are never rendered
        }
    }
}
