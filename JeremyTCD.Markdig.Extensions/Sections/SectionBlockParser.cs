using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Linq;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockParser : BlockParser
    {

        private readonly HeadingBlockParser _headingBlockParser;
        private readonly SectionExtensionOptions _sectionExtensionOptions;

        public SectionBlockParser(SectionExtensionOptions sectionExtensionOptions)
        {
            OpeningCharacters = new[] { '#' };
            Closed += SectionBlockOnClosed;

            _headingBlockParser = new HeadingBlockParser();
            _sectionExtensionOptions = sectionExtensionOptions;
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

            SectionBlockOptions sectionBlockOptions = _sectionExtensionOptions.DefaultSectionBlockOptions.Clone();

            if (processor.CurrentContainer.LastChild is JsonOptionsBlock jsonOptionsBlock)
            {
                JsonOptionsTools.PopulateObject(jsonOptionsBlock, sectionBlockOptions);
                processor.CurrentContainer.Remove(jsonOptionsBlock);
            }

            // Section has a section element specified
            if (newHeadingBlock.Level == 1 && sectionBlockOptions.Level1WrapperElement != SectioningContentElement.None ||
                newHeadingBlock.Level > 1 && sectionBlockOptions.Level2PlusWrapperElement != SectioningContentElement.None)
            {
                var sectionBlock = new SectionBlock(this)
                {
                    Level = newHeadingBlock.Level,
                    SectionBlockOptions = sectionBlockOptions
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

            // Not super efficient (ListBlockParser does somthing similar for thematic breaks), should extract logic for determining if a line is a heading.
            if (!(processor.NewBlocks.Pop() is HeadingBlock newHeadingBlock))
            {
                throw new InvalidOperationException($"Opened a heading block but BlockProcessor.NewBlocks does not contain any blocks.");
            }
            processor.GoToColumn(0);

            SectionBlock currentSectionBlock = block as SectionBlock;

            if (newHeadingBlock.Level <= currentSectionBlock.Level)
            {
                // Close current section block
                return BlockState.None;
            }

            // If next opened block is not null and it is not a section block or it is a section block with level higher than or equal to the new heading block's level, 
            // close the next opened block (automatically closes its children as well). The effect of this is that the new section will be added as a direct child of the current
            // section.
            if (processor.NextContinue != null &&
                (!(processor.NextContinue is SectionBlock nextSectionBlock) || nextSectionBlock.Level >= newHeadingBlock.Level))
            {
                processor.Close(processor.NextContinue);
            }

            // Keep section block open
            return BlockState.Continue;
        }

        /// <summary>
        /// Sets up callbacks that handle section IDs and auto links.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        internal void SectionBlockOnClosed(BlockProcessor processor, Block block)
        {
            SectionBlock sectionBlock = (SectionBlock)block;
            SectionBlockOptions sectionBlockOptions = sectionBlock.SectionBlockOptions;

            // Setup identifier generation and auto links
            if (sectionBlockOptions.GenerateIdentifier)
            {
                HeadingBlock headingBlock = (HeadingBlock)sectionBlock.FirstOrDefault(child => child is HeadingBlock);

                if (headingBlock == null)
                {
                    throw new InvalidOperationException("A section block must contain a heading block.");
                }

                IdentifierGenerationUtils.SetupIdentifierGeneration(headingBlock);

                if (sectionBlockOptions.AutoLinkable)
                {
                    AutoLinkUtils.SetupAutoLink(processor, sectionBlock, headingBlock);
                }
            }
        }
    }
}
