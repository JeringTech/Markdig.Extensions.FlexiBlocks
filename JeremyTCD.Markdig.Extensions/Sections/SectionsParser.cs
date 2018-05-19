using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Linq;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionsParser : BlockParser
    {
        private readonly HeadingBlockParser _headingBlockParser;
        private readonly SectionsOptions _sectionsOptions;
        private readonly AutoLinkService _autoLinkService;
        private readonly IdentifierService _identifierService;
        private readonly JsonOptionsService _jsonOptionsService;

        public SectionsParser(SectionsOptions sectionsOptions,
            HeadingBlockParser headingBlockParser,
            AutoLinkService autoLinkService,
            IdentifierService identifierService,
            JsonOptionsService jsonOptionsService)
        {
            OpeningCharacters = new[] { '#' };
            Closed += SectionBlockOnClosed;

            _sectionsOptions = sectionsOptions;
            _headingBlockParser = headingBlockParser;
            _autoLinkService = autoLinkService;
            _identifierService = identifierService;
            _jsonOptionsService = jsonOptionsService;
        }

        /// <summary>
        /// Attempts to open a <see cref="HeadingBlock"/>. If a <see cref="HeadingBlock"/> is be opened, optionally wraps it in a <see cref="SectionBlock"/>.
        /// Adds any new blocks to <paramref name="processor"/>'s NewBlocks property.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if a <see cref="HeadingBlock"/> cannot be opened.
        /// <see cref="BlockState.Break"/> if a <see cref="HeadingBlock"/> is opened but a <see cref="SectionBlock"/> is not opened (closes the <see cref="HeadingBlock"/>).
        /// <see cref="BlockState.Continue"/> if a <see cref="SectionBlock"/> is opened.
        /// </returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            if (headingBlockState == BlockState.None)
            {
                // Not a heading line
                return BlockState.None;
            }

            SectionBlockOptions sectionBlockOptions = _sectionsOptions.DefaultSectionBlockOptions.Clone();

            // Apply JSON options if they are provided
            _jsonOptionsService.TryPopulateOptions(processor, sectionBlockOptions);

            var newHeadingBlock = (HeadingBlock)processor.NewBlocks.Peek();

            // Optionally, don't open a section block (typically useful for an outermost, level 1 heading that will reside in an existing SectioningContentElement)
            if (newHeadingBlock.Level == 1 && sectionBlockOptions.Level1WrapperElement == SectioningContentElement.None ||
                newHeadingBlock.Level > 1 && sectionBlockOptions.Level2PlusWrapperElement == SectioningContentElement.None)
            {
                // Close heading block
                return BlockState.Break;
            }

            var sectionBlock = new SectionBlock(this)
            {
                Level = newHeadingBlock.Level,
                SectionBlockOptions = sectionBlockOptions,
                Column = newHeadingBlock.Column,
                Span = newHeadingBlock.Span
            };

            processor.NewBlocks.Push(sectionBlock);

            // Keep section block open (note that this keeps the heading block open unecessarily, TryContinue remedies this, but should keep an eye out for a cleaner solution)
            return BlockState.Continue;
        }

        /// <summary>
        /// Attempts to continue a <see cref="SectionBlock"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <returns>
        /// <see cref="BlockState.Continue"/> if the current line is not a <see cref="HeadingBlock"/> or if the current line is a <see cref="HeadingBlock"/> and the new section is a descendant of <paramref name="block"/>.
        /// <see cref="BlockState.None"/> if the current line is a <see cref="HeadingBlock"/> and the new section is a sibling or an uncle of <paramref name="block"/>.
        /// </returns>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            // If first non-whitespace char is not #, continue
            if (processor.CurrentChar != _headingBlockParser.OpeningCharacters[0])
            {
                return BlockState.Continue;
            }

            int initialColumn = processor.Column;
            BlockState headingBlockState = _headingBlockParser.TryOpen(processor);

            // Current line is not a heading block
            if (headingBlockState == BlockState.None)
            {
                return BlockState.Continue;
            }

            // Creating then removing a new HeadingBlock instance isn't efficient, should extract logic for determining if a line is a heading.
            var newHeadingBlock = (HeadingBlock)processor.NewBlocks.Pop();

            // Return to initial column so that TryOpen can open the HeadingBlock
            processor.GoToColumn(initialColumn);

            var sectionBlock = block as SectionBlock;

            // New section is a sibling or uncle of sectionBlock
            if (newHeadingBlock.Level <= sectionBlock.Level)
            {
                // Close current section block and all of its children, but don't discard line so that a new section block is opened
                return BlockState.None;
            }

            // Keep section block open
            return BlockState.Continue;
        }

        /// <summary>
        /// Called when a section block is closed. Sets up callbacks that handle section IDs and auto links.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        internal virtual void SectionBlockOnClosed(BlockProcessor processor, Block block)
        {
            var sectionBlock = (SectionBlock)block;
            SectionBlockOptions sectionBlockOptions = sectionBlock.SectionBlockOptions;

            // Setup identifier generation and auto links
            if (sectionBlockOptions.GenerateIdentifier)
            {
                var headingBlock = (HeadingBlock)sectionBlock.FirstOrDefault(child => child is HeadingBlock);

                if (headingBlock == null)
                {
                    throw new InvalidOperationException("A section block must contain a heading block.");
                }

                _identifierService.SetupIdentifierGeneration(headingBlock);

                if (sectionBlockOptions.AutoLinkable)
                {
                    _autoLinkService.SetupAutoLink(processor, sectionBlock, headingBlock);
                }
            }
        }
    }
}
