using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// <para>A markdig extension for FlexiTableBlocks.</para>
    /// 
    /// <para>This extension uses the default <see cref="Table"/> parsers. What makes the generated <see cref="Table"/>s "FlexiTableBlocks" is the
    /// addition of <see cref="FlexiTableBlockOptions"/> to them.</para>
    /// </summary>
    public class FlexiTableBlocksExtension : FlexiBlocksExtension
    {
        private readonly FlexiTableBlocksExtensionOptions _extensionOptions;
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;
        private readonly FlexiTableBlockRenderer _flexiTableBlockRenderer;

        /// <summary>
        /// The key used for storing <see cref="FlexiTableBlockOptions"/>.
        /// </summary>
        public const string FLEXI_TABLE_BLOCK_OPTIONS_KEY = "flexiTableBlockOptions";

        /// <summary>
        /// Creates a <see cref="FlexiTableBlocksExtension"/> instance.
        /// </summary>
        /// <param name="flexiTableBlockRenderer">The renderer for rendering FlexiTableBlocks as HTML.</param>
        /// <param name="flexiOptionsBlockService">The service that will handle populating of <see cref="FlexiTableBlockOptions"/>.</param>
        /// <param name="extensionOptions">The options for the instance.</param>
        public FlexiTableBlocksExtension(FlexiTableBlockRenderer flexiTableBlockRenderer,
            IFlexiOptionsBlockService flexiOptionsBlockService,
            FlexiTableBlocksExtensionOptions extensionOptions)
        {
            _flexiTableBlockRenderer = flexiTableBlockRenderer ?? throw new ArgumentNullException(nameof(flexiTableBlockRenderer));
            _extensionOptions = extensionOptions ?? throw new ArgumentNullException(nameof(extensionOptions));
            _flexiOptionsBlockService = flexiOptionsBlockService ?? throw new ArgumentNullException(nameof(flexiOptionsBlockService));
        }

        /// <summary>
        /// Registers a <see cref="GridTableParser"/> if one isn't already registered.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder to register the parsers for.</param>
        protected override void SetupParsers(MarkdownPipelineBuilder pipelineBuilder)
        {
            // If GridTableParser hasn't been added to parsers, add it
            GridTableParser gridTableParser = pipelineBuilder.BlockParsers.Find<GridTableParser>();
            if (gridTableParser == null)
            {
                gridTableParser = new GridTableParser();
                pipelineBuilder.BlockParsers.Insert(0, gridTableParser);
            }
            gridTableParser.Closed += OnFlexiBlockClosed;

            // If PipeTable parsers have not been added, add them.
            // Note PipeTableParser is an inline parser with no equivalent to the BlockParser.Closed event, so FlexiOptionsBlock only works
            // for grid tables.
            pipelineBuilder.PreciseSourceLocation = true; // PipeTables require this, refer to PipeTableExtension
            if (!pipelineBuilder.BlockParsers.Contains<PipeTableBlockParser>())
            {
                pipelineBuilder.BlockParsers.Insert(0, new PipeTableBlockParser());
            }
            if (!pipelineBuilder.InlineParsers.Contains<PipeTableParser>())
            {
                LineBreakInlineParser lineBreakParser = pipelineBuilder.InlineParsers.FindExact<LineBreakInlineParser>();
                pipelineBuilder.InlineParsers.InsertBefore<EmphasisInlineParser>(new PipeTableParser(lineBreakParser));
            }
        }

        /// <summary>
        /// Registers a <see cref="FlexiTableBlockRenderer"/> if one isn't already registered.
        /// </summary>
        /// <param name="pipeline">Unused.</param>
        /// <param name="renderer">The root renderer to register the renderer for.</param>
        protected override void SetupRenderers(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiTableBlockRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, _flexiTableBlockRenderer);
                }

                HtmlTableRenderer htmlTableRenderer = htmlRenderer.ObjectRenderers.Find<HtmlTableRenderer>();
                if (htmlTableRenderer != null)
                {
                    htmlRenderer.ObjectRenderers.Remove(htmlTableRenderer);
                }
            }
        }

        /// <summary>
        /// Called when a FlexiTableBlock is closed. Creates the <see cref="FlexiTableBlockOptions"/> for the block.
        /// </summary>
        /// <param name="processor">The block processor for the FlexiTableBlock that has been closed.</param>
        /// <param name="block">The FlexiTableBlock that has been closed.</param>
        protected override void OnFlexiBlockClosed(BlockProcessor processor, Block block)
        {
            // A Table has child TableCellBlocks, this delegate is invoked each time one of them is closed. Ignore all such invocations.
            if (!(block is Table))
            {
                return;
            }

            FlexiTableBlockOptions flexiTableBlockOptions = _extensionOptions.DefaultBlockOptions.Clone();

            // Apply FlexiOptionsBlock options if they exist
            _flexiOptionsBlockService.TryPopulateOptions(processor, flexiTableBlockOptions, block.Line);

            block.SetData(FLEXI_TABLE_BLOCK_OPTIONS_KEY, flexiTableBlockOptions);
        }
    }
}
