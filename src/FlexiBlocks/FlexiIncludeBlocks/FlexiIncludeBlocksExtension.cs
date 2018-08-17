using Markdig;
using Markdig.Renderers;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class FlexiIncludeBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiIncludeBlockParser _flexiIncludeBlockParser;

        public FlexiIncludeBlocksExtension(FlexiIncludeBlockParser flexiIncludeBlockParser)
        {
            _flexiIncludeBlockParser = flexiIncludeBlockParser;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<FlexiIncludeBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, _flexiIncludeBlockParser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // FlexiIncludeBlocks are never rendered
        }
    }
}
