using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace JeremyTCD.Markdig.Extensions
{
    public class SectionParser : BlockParser
    {
        private HeadingBlockParser _headingBlockParser;

        public SectionParser()
        {
            OpeningCharacters = new[] { '#' };
            _headingBlockParser = new HeadingBlockParser();
        }

        public override BlockState TryOpen(BlockProcessor processor)
        {
            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            if (headingBlockState == BlockState.None)
            {
                // Not a heading line
                return BlockState.None;
            }

            HeadingBlock newHeadingBlock = processor.NewBlocks.Peek() as HeadingBlock;

            if(newHeadingBlock == null)
            {
                throw new InvalidOperationException($"Opened a heading block but BlockProcessor.NewBlocks does not contain any blocks.");
            }

            if (newHeadingBlock.Level > 1)
            {
                var sectionBlock = new SectionBlock(this) { Level = newHeadingBlock.Level };

                processor.NewBlocks.Push(sectionBlock);

                // Keep section block open (heading block remains open unecessarily)
                return BlockState.Continue;
            }

            // Close heading block
            return BlockState.Break;
        }

        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            // If first non-whitespace char is not #, continue
            if(processor.CurrentChar != _headingBlockParser.OpeningCharacters[0])
            {
                return BlockState.Continue;
            }

            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            // Current line is not a heading block
            if (headingBlockState == BlockState.None)
            {
                return BlockState.Continue;
            }

            // Blocks can only be added to the last open block, close any open children of the section block
            if(processor.NextContinue != null) // TODO is it ever null?
            {
                processor.Close(processor.NextContinue);
            }

            HeadingBlock newHeadingBlock = processor.NewBlocks.Peek() as HeadingBlock;

            if (newHeadingBlock == null)
            {
                throw new InvalidOperationException($"Opened a heading block but BlockProcessor.NewBlocks does not contain any blocks.");
            }

            SectionBlock currentSectionBlock = block as SectionBlock;

            // Nest if new heading block has higher level than current section block
            if(newHeadingBlock.Level > currentSectionBlock.Level)
            {
                var sectionBlock = new SectionBlock(this) { Level = newHeadingBlock.Level };

                processor.NewBlocks.Push(sectionBlock);

                // Keep section block open (heading block remains open unecessarily)
                return BlockState.Continue;
            }

            // Equal or lower level, remove new heading block (ListBlockParser does somthing similar for thematic breaks)
            processor.NewBlocks.Pop();
            processor.GoToColumn(0);

            // Close current section
            return BlockState.None;
        }
    }
}
