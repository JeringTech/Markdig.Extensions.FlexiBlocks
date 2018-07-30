using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockParser : BlockParser
    {
        public const string FLEXI_OPTIONS_BLOCK = "flexiOptionsBlock";

        public FlexiOptionsBlockParser()
        {
            // If options block is not consumed by the following block, it is rendered as a paragraph or in the preceding paragraph, so {, despite being common, should work fine.
            OpeningCharacters = new[] { '@' };
        }

        /// <summary>
        /// Opens a FlexiOptionsBlock if a line begins with "@{".
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not start with @{.
        /// <see cref="BlockState.Break"/> if the current line contains the entire JSON string.
        /// <see cref="BlockState.Continue"/> if the current line contains part of the JSON string.
        /// </returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // First line of a FlexiOptionsBlock must begin with @{
            if (processor.Line.PeekChar() != '{')
            {
                return BlockState.None;
            }

            // Dispose of @ (BlockProcessor appends processor.Line to the new FlexiOptionsBlock, so it must start at the curly bracket)
            processor.Line.Start++;

            var flexiOptionsBlock = new FlexiOptionsBlock(this)
            {
                Column = processor.Column,
                Span = { Start = processor.Line.Start }
            };
            processor.NewBlocks.Push(flexiOptionsBlock);

            return TryContinue(processor, flexiOptionsBlock);
        }

        /// <summary>
        /// Determines whether or not the <see cref="FlexiOptionsBlock"/> is complete by checking whether all opening curly brackets have been closed. 
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
            var flexiOptionsBlock = (FlexiOptionsBlock)block;
            StringSlice line = processor.Line;
            char pc = line.PeekCharExtra(-1);
            char c = line.CurrentChar;

            while (c != '\0')
            {
                if (!flexiOptionsBlock.EndsInString)
                {
                    if (c == '{')
                    {
                        flexiOptionsBlock.NumOpenBrackets++;
                    }
                    else if (c == '}')
                    {
                        if (--flexiOptionsBlock.NumOpenBrackets == 0)
                        {
                            flexiOptionsBlock.UpdateSpanEnd(line.End);
                            flexiOptionsBlock.EndLine = processor.LineIndex;

                            // Unused FlexiOptionsBlock
                            if (processor.Document.GetData(FLEXI_OPTIONS_BLOCK) is FlexiOptionsBlock pendingFlexiOptionsBlock)
                            {
                                throw new InvalidOperationException(string.Format(
                                    Strings.InvalidOperationException_UnusedFlexiOptionsBlock,
                                    pendingFlexiOptionsBlock.Lines.ToString(),
                                    pendingFlexiOptionsBlock.Line,
                                    pendingFlexiOptionsBlock.Column));
                            }

                            // Save the options block to document data. There are two reasons for this. Firstly, it makes it easy to detect if an options block goes unused.
                            // Secondly, it means that the options block does not need to be a sibling of the block that consumes it. This can occur in
                            // when extensions like FlexiSectionBlocks is used - when a container block only ends when a new container block
                            // is encountered, the options block ends up being a child of the container block that precedes the container block that the options apply to. Leave 
                            // the options block in the AST so that the current line gets assigned to it.
                            processor.Document.SetData(FLEXI_OPTIONS_BLOCK, flexiOptionsBlock);

                            return BlockState.Break;
                        }
                    }
                    else if (pc != '\\' && c == '"')
                    {
                        flexiOptionsBlock.EndsInString = true;
                    }
                }
                else if (pc != '\\' && c == '"')
                {
                    flexiOptionsBlock.EndsInString = false;
                }

                pc = c;
                c = line.NextChar();
            }

            return BlockState.Continue;
        }

        public override bool Close(BlockProcessor processor, Block block)
        {
            // If true is returned, the block is kept as a child of its parent for rendering later on. If false is returned,
            // the block is discarded. We don't need the block any more.
            return false;
        }
    }
}
