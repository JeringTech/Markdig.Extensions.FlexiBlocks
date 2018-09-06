using Markdig;
using Markdig.Renderers;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// A markdig extension for <see cref="FlexiAlertBlock"/>s.
    /// </summary>
    public class FlexiAlertBlocksExtension : FlexiBlocksExtension
    {
        private readonly FlexiAlertBlockParser _flexiAlertBlockParser;
        private readonly FlexiAlertBlockRenderer _flexiAlertBlockRenderer;

        /// <summary>
        /// Creates a <see cref="FlexiAlertBlocksExtension"/> instance.
        /// </summary>
        /// <param name="flexiAlertBlockParser">The parser for creating <see cref="FlexiAlertBlock"/>s from markdown.</param>
        /// <param name="flexiAlertBlockRenderer">The renderer for rendering <see cref="FlexiAlertBlock"/>s as HTML.</param>
        public FlexiAlertBlocksExtension(FlexiAlertBlockParser flexiAlertBlockParser, FlexiAlertBlockRenderer flexiAlertBlockRenderer)
        {
            _flexiAlertBlockParser = flexiAlertBlockParser;
            _flexiAlertBlockRenderer = flexiAlertBlockRenderer;
        }

        /// <summary>
        /// Registers a <see cref="FlexiAlertBlock"/> parser if one isn't already registered.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder to register the parser for.</param>
        public override void Setup(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.BlockParsers.Contains<FlexiAlertBlockParser>())
            {
                pipelineBuilder.BlockParsers.Insert(0, _flexiAlertBlockParser);
            }
        }

        /// <summary>
        /// Registers a <see cref="FlexiAlertBlock"/> renderer if one isn't already registered.
        /// </summary>
        /// <param name="pipeline">The pipeline to register the renderer for.</param>
        /// <param name="renderer">The root renderer to register the renderer for.</param>
        public override void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer && !htmlRenderer.ObjectRenderers.Contains<FlexiAlertBlockRenderer>())
            {
                htmlRenderer.ObjectRenderers.Insert(0, _flexiAlertBlockRenderer);
            }
        }
    }
}
