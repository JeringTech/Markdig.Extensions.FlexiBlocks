using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Parsers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Syntax;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    public class FlexiTableBlocksExtension : IMarkdownExtension
    {
        public const string FLEXI_TABLE_BLOCK_OPTIONS_KEY = "flexiTableBlockOptions";
        private readonly FlexiTableBlocksExtensionOptions _options;
        private readonly FlexiOptionsBlockService _flexiOptionsBlockService;

        public FlexiTableBlocksExtension(FlexiTableBlocksExtensionOptions options)
        {
            _options = options ?? new FlexiTableBlocksExtensionOptions();
            _flexiOptionsBlockService = new FlexiOptionsBlockService();
        }

        /// <summary>
        /// Adds grid tables and pipe tables if they haven't already been added.
        /// </summary>
        /// <param name="pipeline"></param>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // If GridTableParser hasn't been added to parsers, add it
            GridTableParser gridTableParser = pipeline.BlockParsers.Find<GridTableParser>();
            if (gridTableParser == null)
            {
                gridTableParser = new GridTableParser();
                pipeline.BlockParsers.Insert(0, gridTableParser);
            }
            gridTableParser.Closed += TableBlockOnClosed;

            // If PipeTable parsers have not been added, add them.
            // Note PipeTableParser is an inline parser and equivalent to the Closed delegate, so FlexiOptionsBlock only works
            // for grid tables.
            pipeline.PreciseSourceLocation = true;
            if (!pipeline.BlockParsers.Contains<PipeTableBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, new PipeTableBlockParser());
            }
            LineBreakInlineParser lineBreakParser = pipeline.InlineParsers.FindExact<LineBreakInlineParser>();
            if (!pipeline.InlineParsers.Contains<PipeTableParser>())
            {
                pipeline.InlineParsers.InsertBefore<EmphasisInlineParser>(new PipeTableParser(lineBreakParser));
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<FlexiTableBlockRenderer>())
                {
                    HtmlTableRenderer htmlTableRenderer = htmlRenderer.ObjectRenderers.Find<HtmlTableRenderer>();
                    if (htmlTableRenderer != null)
                    {
                        htmlRenderer.ObjectRenderers.Remove(htmlTableRenderer);
                    }

                    htmlRenderer.ObjectRenderers.Insert(0, new FlexiTableBlockRenderer(_options.DefaultFlexiTableBlockOptions));
                }
            }
        }

        /// <summary>
        /// Called when a <see cref="Table"/> is closed. Creates the <see cref="FlexiTableBlockOptions"/> for <paramref name="block"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        internal virtual void TableBlockOnClosed(BlockProcessor processor, Block block)
        {
            // A Table has child TableCellBlocks, this delegate is invoked each time one of them is closed. Ignore all such invocations.
            if(!(block is Table))
            {
                return;
            }

            FlexiTableBlockOptions flexiTableBlockOptions = _options.DefaultFlexiTableBlockOptions.Clone();

            // Apply FlexiOptionsBlock options if they exist
            _flexiOptionsBlockService.TryPopulateOptions(processor, flexiTableBlockOptions, block.Line);

            block.SetData(FLEXI_TABLE_BLOCK_OPTIONS_KEY, flexiTableBlockOptions);
        }
    }
}
