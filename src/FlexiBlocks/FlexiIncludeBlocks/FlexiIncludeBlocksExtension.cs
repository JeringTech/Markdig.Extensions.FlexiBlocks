using Markdig;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// A markdig extension for <see cref="FlexiIncludeBlock"/>s.
    /// </summary>
    public class FlexiIncludeBlocksExtension : FlexiBlocksExtension
    {
        private readonly FlexiIncludeBlockParser _flexiIncludeBlockParser;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlocksExtension"/> instance.
        /// </summary>
        /// <param name="flexiIncludeBlockParser">The parser for creating <see cref="FlexiIncludeBlock"/>s from markdown.</param>
        public FlexiIncludeBlocksExtension(FlexiIncludeBlockParser flexiIncludeBlockParser)
        {
            _flexiIncludeBlockParser = flexiIncludeBlockParser ?? throw new ArgumentNullException(nameof(flexiIncludeBlockParser));
        }

        /// <summary>
        /// Registers a <see cref="FlexiIncludeBlockParser"/> if one isn't already registered.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder to register the parser for.</param>
        public override void Setup(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (pipelineBuilder == null)
            {
                throw new ArgumentNullException(nameof(pipelineBuilder));
            }

            if (!pipelineBuilder.BlockParsers.Contains<FlexiIncludeBlockParser>())
            {
                pipelineBuilder.BlockParsers.Insert(0, _flexiIncludeBlockParser);
            }
        }
    }
}
