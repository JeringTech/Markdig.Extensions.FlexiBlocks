using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace JeremyTCD.Markdig.Extensions
{
    public class SectionParser : BlockParser
    {
        private HeadingBlockParser _headingBlockParser;
        private SectionExtensionOptions _options;

        public SectionParser(SectionExtensionOptions options)
        {
            OpeningCharacters = new[] { '#' };
            _headingBlockParser = new HeadingBlockParser();
            _options = options;
        }

        public override BlockState TryOpen(BlockProcessor processor)
        {
            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            if (headingBlockState == BlockState.None)
            {
                // Not a heading line
                return BlockState.None;
            }


            if (!(processor.NewBlocks.Peek() is HeadingBlock newHeadingBlock))
            {
                throw new InvalidOperationException($"Opened a heading block but BlockProcessor.NewBlocks does not contain any blocks.");
            }


            SectionOptions sectionOptions = _options.DefaultSectionOptions;
            JsonOptionsTools.PopulateObject(processor, sectionOptions);

            // Section has a section element specified
            if (sectionOptions.WrapperElement.CompareTo(SectioningContentElement.None) > 0 ||
                newHeadingBlock.Level == 1 && _options.Level1WrapperElement.CompareTo(SectioningContentElement.None) > 0 ||
                newHeadingBlock.Level > 1 && _options.Level2PlusWrapperElement.CompareTo(SectioningContentElement.None) > 0)
            {
                var sectionBlock = new SectionBlock(this)
                {
                    Level = newHeadingBlock.Level,
                    HeadingWrapperElement = sectionOptions.WrapperElement != SectioningContentElement.Undefined ? sectionOptions.WrapperElement :
                        newHeadingBlock.Level == 1 ? _options.Level1WrapperElement : _options.Level2PlusWrapperElement
                };

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
            if (processor.CurrentChar != _headingBlockParser.OpeningCharacters[0])
            {
                return BlockState.Continue;
            }

            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            // Current line is not a heading block
            if (headingBlockState == BlockState.None)
            {
                return BlockState.Continue;
            }

            if (!(processor.NewBlocks.Peek() is HeadingBlock newHeadingBlock))
            {
                throw new InvalidOperationException($"Opened a heading block but BlockProcessor.NewBlocks does not contain any blocks.");
            }

            SectionBlock currentSectionBlock = block as SectionBlock;

            if (newHeadingBlock.Level <= currentSectionBlock.Level)
            {
                // Equal or lower level, remove new heading block (ListBlockParser does somthing similar for thematic breaks)
                processor.NewBlocks.Pop();
                processor.GoToColumn(0);

                // Close current section
                return BlockState.None;
            }

            // Nest if new heading block has higher level than current section block and current section block has no child section block
            // or has a child section block with level higher than or equal to the new heading block's level
            if (!(processor.NextContinue is SectionBlock) ||
                (processor.NextContinue as SectionBlock).Level >= newHeadingBlock.Level)
            {
                // Blocks can only be added to the last open block, close any open children of the section block
                if (processor.NextContinue != null)
                {
                    processor.Close(processor.NextContinue);
                }

                var sectionBlock = new SectionBlock(this)
                {
                    Level = newHeadingBlock.Level,
                    HeadingWrapperElement = SectioningContentElement.Section
                };

                processor.NewBlocks.Push(sectionBlock);
            }
            else
            {
                // New heading block should be a child of a child section block
                processor.NewBlocks.Pop();
                processor.GoToColumn(0);
            }

            // Keep section block open
            return BlockState.Continue;
        }
    }
}
