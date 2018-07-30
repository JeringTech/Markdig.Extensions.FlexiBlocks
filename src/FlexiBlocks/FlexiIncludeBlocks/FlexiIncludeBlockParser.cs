using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Newtonsoft.Json;
using System.IO;

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
        private FlexiIncludeBlocksExtensionOptions _flexiIncludeBlocksExtensionOptions;

        /// <summary>
        /// Creates a <see cref="FlexiIncludeBlockParser"/> instance.
        /// </summary>
        public FlexiIncludeBlockParser(FlexiIncludeBlocksExtensionOptions flexiIncludeBlocksExtensionOptions)
        {
            _flexiIncludeBlocksExtensionOptions = flexiIncludeBlocksExtensionOptions;

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

                            // This is usually done by BlockProcessor, but we need the complete text content of the block here
                            flexiIncludeBlock.AppendLine(ref line, processor.Column, processor.LineIndex, processor.CurrentLineStartPosition);

                            // Force close flexiIncludeBlock so that it is removed from processor.OpenBlocks and so doesn't interfere
                            // with the addition of its replacement blocks.
                            processor.Close(flexiIncludeBlock);

                            ParseIncludedContent(processor, flexiIncludeBlock);

                            // TODO stop processing for the line completely
                            return BlockState.BreakDiscard;
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

        // TODO check how docfx's include works
        // TODO, create more specs, step through, make sure this works for all situations
        private void ParseIncludedContent(BlockProcessor processor, FlexiIncludeBlock flexiIncludeBlock)
        {
            string json = flexiIncludeBlock.Lines.ToString();
            IncludeOptions includeOptions = JsonConvert.DeserializeObject<IncludeOptions>(json);

            // TODO is file a local path or a url? (copy FileRetrievalService)

            // TODO Retrieve unclipped content
            string fullPath = Path.Combine(_flexiIncludeBlocksExtensionOptions.ProjectPath, includeOptions.Source);
            string unclippedContent = File.ReadAllText(fullPath);

            // TODO clip content (copy DedentingService and FileClippingService)


            // Default to ContentType.Code
            if (includeOptions.ContentType != ContentType.Markdown)
            {
                // TODO wrap clipped content in a code block (just append and prepend ```)
            }

            // TODO Save processor state ( look through all block processor variables, what needs to be saved?)
            int cachedLineIndex = processor.LineIndex;
            processor.LineIndex = 0;

            // Process clipped content as though it is part of the original document
            var lineReader = new LineReader(unclippedContent);
            while (true)
            {
                StringSlice? lineText = lineReader.ReadLine();

                // If this is the end of file and the last line is empty
                if (lineText == null)
                {
                    break;
                }
                processor.ProcessLine(lineText.Value);
            }

            // Reset processor state
            processor.LineIndex = cachedLineIndex;
        }

        public override bool Close(BlockProcessor processor, Block block)
        {
            // If true is returned, the block is kept as a child of its parent for rendering later on. If false is returned,
            // the block is discarded. We don't need the block any more.
            return false;
        }
    }
}
