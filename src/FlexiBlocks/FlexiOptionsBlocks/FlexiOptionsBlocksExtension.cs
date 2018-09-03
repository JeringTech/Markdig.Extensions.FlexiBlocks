using Markdig;
using Markdig.Renderers;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    /// <summary>
    /// A Markdig extension for <see cref="FlexiOptionsBlock"/>s.
    /// </summary>
    public class FlexiOptionsBlocksExtension : IMarkdownExtension
    {
        private readonly FlexiOptionsBlockParser _flexiOptionsBlockParser;

        /// <summary>
        /// Creates a <see cref="FlexiOptionsBlocksExtension"/> instance.
        /// </summary>
        /// <param name="flexiOptionsBlockParser">The parser for creating <see cref="FlexiOptionsBlock"/>s from markdown.</param>
        public FlexiOptionsBlocksExtension(FlexiOptionsBlockParser flexiOptionsBlockParser)
        {
            _flexiOptionsBlockParser = flexiOptionsBlockParser;
        }

        /// <summary>
        /// Registers a <see cref="FlexiOptionsBlock"/> parser if one isn't already registered.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder to register the parser for.</param>
        public void Setup(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.BlockParsers.Contains<FlexiOptionsBlockParser>())
            {
                pipelineBuilder.BlockParsers.Insert(0, _flexiOptionsBlockParser);
            }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="renderer"></param>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // FlexiOptionsBlocks are never rendered.
        }
    }
}
