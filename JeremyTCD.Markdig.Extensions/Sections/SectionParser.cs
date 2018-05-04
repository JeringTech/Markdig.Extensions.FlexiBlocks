using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

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
            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            if (headingBlockState == BlockState.None)
            {
                return BlockState.Continue;
            }

            // Section followed by heading in OpenedBlocks
            if(processor.NextContinue is HeadingBlock)
            {
                processor.Close(processor.NextContinue);
            }

            HeadingBlock newHeadingBlock = processor.NewBlocks.Peek() as HeadingBlock;
            SectionBlock currentSectionBlock = block as SectionBlock;

            // TODO if levels are skipped, e.g 2 to 4
            if(newHeadingBlock.Level > currentSectionBlock.Level)
            {
                var sectionBlock = new SectionBlock(this) { Level = newHeadingBlock.Level };

                processor.NewBlocks.Push(sectionBlock);

                // Keep section block open (heading block remains open unecessarily)
                return BlockState.Continue;
            }

            // Equal or lower level, remove new heading block, close current section (ListBlockParser does somthing similar for thematic breaks)
            processor.NewBlocks.Pop();

            return BlockState.None;
        }
    }
}
