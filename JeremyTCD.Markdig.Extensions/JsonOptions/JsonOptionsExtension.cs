using Markdig;
using Markdig.Renderers;

namespace JeremyTCD.Markdig.Extensions.JsonOptions
{
    public class JsonOptionsExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<JsonOptionsBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, new JsonOptionsBlockParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // JsonOptions are never rendered
        }
    }
}
