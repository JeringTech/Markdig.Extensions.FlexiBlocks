using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks
{
    /// <summary>
    /// <para>An abstraction for FlexiBlock extensions.</para>
    /// 
    /// <para>FlexiBlocks extensions utilize <see cref="FlexiOptionsBlocksExtension"/> - it is what makes FlexiBlocks flexible. Typical usage of <see cref="FlexiOptionsBlocksExtension"/>
    /// involves extracting a <see cref="FlexiOptionsBlock"/> in a parser's <see cref="FlexiBlockParser.TryOpenFlexiBlock(BlockProcessor)"/> method.</para>
    /// 
    /// <para>Not all extensions define their own parsers though. For such extensions, a method must be assigned to the <see cref="BlockParser.Closed"/> event of the parser(s)
    /// that the extension depends on.</para>
    /// 
    /// <para>For example, <see cref="FlexiCodeBlocksExtension"/> does not defined its own parser, instead it reuses <see cref="FencedCodeBlockParser"/> and 
    /// <see cref="IndentedCodeBlockParser"/>. To ensure that <see cref="FlexiOptionsBlock"/>s are generated for code blocks, it adds
    /// methods to the <see cref="BlockParser.Closed"/> events of both parsers.</para>
    /// 
    /// <para>The primary functionality that this class provides is consistent exception handling for methods assigned to <see cref="BlockParser.Closed"/>.
    /// Implementors that are unable to define <see cref="FlexiBlockParser.TryOpenFlexiBlock(BlockProcessor)"/> for their parsers should override 
    /// <see cref="OnFlexiBlockClosed(BlockProcessor, Block)"/> and register <see cref="OnClosed(BlockProcessor, Block)"/> to <see cref="BlockParser.Closed"/>. </para>
    /// </summary>
    public abstract class FlexiBlocksExtension : IMarkdownExtension
    {
        /// <summary>
        /// Registers FlexiBlock parsers.
        /// </summary>
        /// <param name="pipelineBuilder">The pipeline builder to register the parsers for.</param>
        public abstract void Setup(MarkdownPipelineBuilder pipelineBuilder);

        /// <summary>
        /// Registers FlexiBlock renderers. Extensions whose blocks aren't rendered do not need to override this method.
        /// </summary>
        /// <param name="pipeline">The pipeline to register renderers for.</param>
        /// <param name="renderer">The root renderer to register renderers for.</param>
        public virtual void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // Do nothing by default
        }

        /// <summary>
        /// Handles a closed FlexiBlock.
        /// </summary>
        /// <param name="processor">The block processor for the closed FlexiBlock.</param>
        /// <param name="block">The FlexiBlock that has just been closed.</param>
        protected virtual void OnFlexiBlockClosed(BlockProcessor processor, Block block) {
            // Do nothing by default
        }

        /// <summary>
        /// Handles a closed FlexiBlock.
        /// </summary>
        /// <param name="processor">The block processor for the closed FlexiBlock.</param>
        /// <param name="block">The FlexiBlock that has just been closed.</param>
        /// <exception cref="FlexiBlocksException">Thrown if an exception is thrown while handling a closed FlexiBlock.</exception>
        protected void OnClosed(BlockProcessor processor, Block block)
        {
            try
            {
                OnFlexiBlockClosed(processor, block);
            }
            catch (Exception exception) when ((exception as FlexiBlocksException)?.Context != Context.Block)
            {
                throw new FlexiBlocksException(block, exception);
            }
        }
    }
}
