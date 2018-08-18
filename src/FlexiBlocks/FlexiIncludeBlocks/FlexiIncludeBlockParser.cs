using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockParser : BlockParser
    {
        private readonly FlexiIncludeBlocksExtensionOptions _extensionOptions;
        private readonly IContentRetrievalService _contentRetrievalService;
        private readonly List<ClippingArea> _defaultClippingAreas = new List<ClippingArea> { new ClippingArea() };
        private readonly StringSlice _codeBlockFence = new StringSlice("```");

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

        public override bool Close(BlockProcessor processor, Block block)
        {
            var flexiIncludeBlock = (FlexiIncludeBlock)block;
            string json = flexiIncludeBlock.Lines.ToString();
            IncludeOptions includeOptions = JsonConvert.DeserializeObject<IncludeOptions>(json);

            // TODO block circular includes

            // Retrieve content (read as lines since we will most probably only be using a subset of all the lines)
            ReadOnlyCollection<string> content = _contentRetrievalService.GetContent(includeOptions.Source,
                includeOptions.CacheOnDisk ? _extensionOptions.FileCacheDirectory : null,
                _extensionOptions.SourceBaseUri);

            // Convert content into blocks and replace flexiIncludeBlock with the newly created blocks
            ReplaceFlexiIncludeBlock(processor, flexiIncludeBlock, content, includeOptions);

            // If true is returned, the block is kept as a child of its parent for rendering later on. If false is returned,
            // the block is discarded. We don't need the block any more.
            return false;
        }

        internal virtual void ProcessText(BlockProcessor processor, string text)
        {
            var lineReader = new LineReader(text);
            while (true)
            {
                // Get the precise position of the begining of the line
                StringSlice? lineText = lineReader.ReadLine();

                // If this is the end of file and the last line is empty
                if (lineText == null)
                {
                    break;
                }
                processor.ProcessLine(lineText.Value);
            }
        }

        // TODO Tests
        // - integration tests
        //      - ContentType.Code produces a code block
        //      - before text gets processed
        //      - exception thrown if no line contains start sub string
        //      - exception thrown if no line contains end substring
        //      - correct lines get clipped when using line numbers indicate start and end lines
        //      - correct lines get clipped when using substrings to indicate start and end lines
        // TODO dedenting, collapsing
        internal virtual void ReplaceFlexiIncludeBlock(BlockProcessor processor, FlexiIncludeBlock flexiIncludeBlock, ReadOnlyCollection<string> content, IncludeOptions includeOptions)
        {
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

            // Clip content
            List<ClippingArea> clippingAreas = includeOptions.ClippingAreas ?? _defaultClippingAreas;

            // If content is code, start with ```
            if (includeOptions.ContentType != ContentType.Markdown) // Code by default
            {
                childProcessor.ProcessLine(_codeBlockFence);
            }

            // Clipping areas need not be sequential, they can also overlap
            foreach(ClippingArea clippingArea in clippingAreas)
            {
                if(clippingArea.BeforeText != null)
                {
                    ProcessText(childProcessor, clippingArea.BeforeText);
                }

                int startLineNumber = -1;
                if(clippingArea.StartLineSubString != null)
                {
                    for (int i = 0; i < content.Count; i++)
                    {
                        if (content[i].Contains(clippingArea.StartLineSubString))
                        {
                            startLineNumber = i + 1;
                            break;
                        }
                    }

                    if (startLineNumber == -1)
                    {
                        throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_InvalidClippingAreaNoLineContainsStartLineSubstring, clippingArea.StartLineSubString));
                    }
                }
                else
                {
                    startLineNumber = clippingArea.StartLineNumber;
                }

                for(int lineNumber = startLineNumber; lineNumber <= content.Count; lineNumber++)
                {
                    string line = content[lineNumber - 1];

                    childProcessor.ProcessLine(new StringSlice(line));

                    // Check whether we've reached the end of the clipping area
                    if(clippingArea.EndLineSubString != null)
                    {
                        if(lineNumber == content.Count)
                        {
                            throw new InvalidOperationException(string.Format(Strings.InvalidOperationException_InvalidClippingAreaNoLineContainsEndLineSubstring, clippingArea.EndLineSubString));
                        }

                        // Check if next line contains the end line substring
                        if (content[lineNumber].Contains(clippingArea.EndLineSubString)){
                            break;
                        }
                    }
                    else if(clippingArea.EndLineNumber == clippingArea.EndLineNumber)
                    {
                        break;
                    }
                }

                if (clippingArea.AfterText != null)
                {
                    ProcessText(childProcessor, clippingArea.AfterText);
                }
            }

            if (includeOptions.ContentType != ContentType.Markdown) // TODO last line number isn't necessarily count
            {
                childProcessor.ProcessLine(_codeBlockFence);
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
        }
    }
}
