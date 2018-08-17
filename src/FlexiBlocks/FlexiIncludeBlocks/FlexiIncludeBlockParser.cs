using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Buffers;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    /// <summary>
    /// TODO creates block, parses json. 
    /// - if include content is markdown, inserts it and parses it from the top.
    /// - if include content is code, creates a code block with the content as the code.
    /// TODO try to use newtonsoft.json to parse json line by line
    /// </summary>
    public class FlexiIncludeBlockParser : BlockParser
    {
        private readonly FlexiIncludeBlocksExtensionOptions _extensionOptions;
        private readonly IContentRetrievalService _contentRetrievalService;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlockParser"/> instance.
        /// </summary>
        /// <param name="extensionOptionsAccessor"></param>
        /// <param name="contentRetrievalService"></param>
        public FlexiIncludeBlockParser(IOptions<FlexiIncludeBlocksExtensionOptions> extensionOptionsAccessor,
            IContentRetrievalService contentRetrievalService)
        {
            _extensionOptions = extensionOptionsAccessor.Value;
            _contentRetrievalService = contentRetrievalService;

            OpeningCharacters = new[] { '+' };
        }

        /// <summary>
        /// Opens a FlexiIncludeBlock if a line begins with "+{".
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not start with +{.
        /// <see cref="BlockState.Break"/> if the current line contains the entire JSON string.
        /// <see cref="BlockState.Continue"/> if the current line contains part of the JSON string.
        /// </returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // First line of a FlexiOptionsBlock must begin with +{
            if (processor.Line.PeekChar() != '{')
            {
                return BlockState.None;
            }

            // Dispose of + (BlockProcessor appends processor.Line to the new FlexiIncludeBlock, so it must start at the curly bracket)
            processor.Line.Start++;

            var flexiIncludeBlock = new FlexiIncludeBlock(this)
            {
                Column = processor.Column,
                Span = { Start = processor.Line.Start }
            };
            processor.NewBlocks.Push(flexiIncludeBlock);

            return TryContinue(processor, flexiIncludeBlock);
        }

        /// <summary>
        /// Determines whether or not the <see cref="FlexiIncludeBlock"/> is complete by checking whether all opening curly brackets have been closed. 
        /// The JSON spec allows for unescaped curly brackets within strings - https://www.json.org/, so this method ignores everything between unescaped quotes.
        /// 
        /// TODO This function can be improved - it does not verify that what has been read is valid JSON. Use JsonTextReader?
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <returns>
        /// <see cref="BlockState.Continue"/> if <paramref name="block"/> is still open.
        /// <see cref="BlockState.Break"/> if <paramref name="block"/> has ended and should be closed.
        /// </returns>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            var flexiIncludeBlock = (FlexiIncludeBlock)block;

            StringSlice line = processor.Line;
            char pc = line.PeekCharExtra(-1);
            char c = line.CurrentChar;

            while (c != '\0')
            {
                if (!flexiIncludeBlock.EndsInString)
                {
                    if (c == '{')
                    {
                        flexiIncludeBlock.NumOpenBrackets++;
                    }
                    else if (c == '}')
                    {
                        if (--flexiIncludeBlock.NumOpenBrackets == 0)
                        {
                            flexiIncludeBlock.UpdateSpanEnd(line.End);

                            // End block
                            return BlockState.Break;
                        }
                    }
                    else if (pc != '\\' && c == '"')
                    {
                        flexiIncludeBlock.EndsInString = true;
                    }
                }
                else if (pc != '\\' && c == '"')
                {
                    flexiIncludeBlock.EndsInString = false;
                }

                pc = c;
                c = line.NextChar();
            }

            return BlockState.Continue;
        }

        // TODO, create more specs, step through, make sure this works for all situations
        public override bool Close(BlockProcessor processor, Block block)
        {
            var flexiIncludeBlock = (FlexiIncludeBlock)block;
            string json = flexiIncludeBlock.Lines.ToString();
            IncludeOptions includeOptions = JsonConvert.DeserializeObject<IncludeOptions>(json);

            // TODO block circular includes

            // Retrieve content (read as lines since we will most probably only be using a subset of all the lines)
            ReadOnlyCollection<string> unclippedContent = _contentRetrievalService.GetContent(includeOptions.Source,
                includeOptions.CacheRemoteSource ? _extensionOptions.FileCacheDirectory : null,
                _extensionOptions.SourceBaseUri);


            // GridTable uses this pattern. Essentially, it creates a fresh context with the same root document. Not having a bunch of 
            // open blocks makes it possible to create the replacement blocks for flexiIncludeBlock.
            BlockProcessor childProcessor = processor.CreateChild();
            var tempContainerBlock = new TempContainerBlock(null);
            childProcessor.Open(tempContainerBlock);

            // TODO what else is line index used for? 
            // TODO printing of errors when in a child processor, line numbers etc
            // MarkdownObject.Line is the line that the block starts at, it is set by BlockProcessor.ProcessNewBlocks. We need to set 
            // LineIndex to the line that the include block starts at for FlexiOptionsBlocks to work.
            childProcessor.LineIndex = flexiIncludeBlock.Line;

            // Process included content
            // TODO clip content
            for (int i = 0; i < unclippedContent.Count; i++)
            {
                if (i == 0 && includeOptions.ContentType != ContentType.Markdown)
                {
                    childProcessor.ProcessLine(new StringSlice("```"));
                }


                childProcessor.ProcessLine(new StringSlice(unclippedContent[i]));

                if (i == unclippedContent.Count - 1 && includeOptions.ContentType != ContentType.Markdown)
                {
                    childProcessor.ProcessLine(new StringSlice("```"));
                }
            }

            // Close temp container block
            childProcessor.Close(tempContainerBlock);

            // A Block with a parent cannot be added to another block. So before we add the replacement blocks to flexiIncludeBlock's parent, 
            // we have to copy the blocks to an array, clear tempContainerBlock (this sets Parent to null for all of its child blocks).
            Block[] replacementBlocks = ArrayPool<Block>.Shared.Rent(tempContainerBlock.Count);
            try
            {
                int numReplacementBlocks = tempContainerBlock.Count;
                tempContainerBlock.CopyTo(replacementBlocks, 0);
                tempContainerBlock.Clear();
                for (int i = 0; i < numReplacementBlocks; i++)
                {
                    flexiIncludeBlock.Parent.Add(replacementBlocks[i]);
                }
            }
            finally
            {
                ArrayPool<Block>.Shared.Return(replacementBlocks);
            }

            // BlockProcessors are pooled. Once we're done with innerProcessor, we must release it. This also removes all references to
            // tempContainerBlock, which should allow it to be collected quickly.
            childProcessor.ReleaseChild();

            // If true is returned, the block is kept as a child of its parent for rendering later on. If false is returned,
            // the block is discarded. We don't need the block any more.
            return false;
        }
    }
}
