using Markdig;
using Markdig.Renderers;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class FlexiIncludeBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiIncludeBlocksExtensionOptions _options;

        public FlexiIncludeBlocksExtension(FlexiIncludeBlocksExtensionOptions options)
        {
            _options = options ?? new FlexiIncludeBlocksExtensionOptions();
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<FlexiIncludeBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, new FlexiIncludeBlockParser(_options));
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // FlexiIncludeBlocks are never rendered
        }
    }
}
