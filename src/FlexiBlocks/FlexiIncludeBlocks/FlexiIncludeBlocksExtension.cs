using Markdig;
using System;
using System.Collections.Generic;

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
        public override void SetupParsers(MarkdownPipelineBuilder pipelineBuilder)
        {
            if (!pipelineBuilder.BlockParsers.Contains<FlexiIncludeBlockParser>())
            {
                pipelineBuilder.BlockParsers.Insert(0, _flexiIncludeBlockParser);
            }
        }

        /// <summary>
        /// Gets all <see cref="FlexiIncludeBlock"/> trees processed by this <see cref="FlexiIncludeBlocksExtension"/> instance.
        /// </summary>
        public List<FlexiIncludeBlock> GetFlexiIncludeBlockTrees()
        {
            return _flexiIncludeBlockParser.GetFlexiIncludeBlockTrees();
        }

        /// <summary>
        /// Gets the absolute URIs of all sources included by this <see cref="FlexiIncludeBlocksExtension"/> instance.
        /// </summary>
        public HashSet<string> GetIncludedSourcesAbsoluteUris()
        {
            List<FlexiIncludeBlock> flexiIncludeBlockTrees = GetFlexiIncludeBlockTrees();
            var includedSourcesAbsoluteUris = new HashSet<string>();

            foreach(FlexiIncludeBlock flexiIncludeBlock in flexiIncludeBlockTrees)
            {
                GetIncludedSourcesAbsoluteUrisCore(flexiIncludeBlock, includedSourcesAbsoluteUris);
            }

            return includedSourcesAbsoluteUris;
        }

        private void GetIncludedSourcesAbsoluteUrisCore(FlexiIncludeBlock flexiIncludeBlock, HashSet<string> includedSourcesAbsoluteUris)
        {
            includedSourcesAbsoluteUris.Add(flexiIncludeBlock.AbsoluteSourceUri.AbsoluteUri);

            foreach(FlexiIncludeBlock childFlexiIncludeBlock in flexiIncludeBlock.ChildFlexiIncludeBlocks)
            {
                GetIncludedSourcesAbsoluteUrisCore(childFlexiIncludeBlock, includedSourcesAbsoluteUris);
            }
        }
    }
}
