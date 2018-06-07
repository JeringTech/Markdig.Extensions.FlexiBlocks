using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Linq;

namespace JeremyTCD.Markdig.Extensions.Sections
{
    public class SectionBlockParser : BlockParser
    {
        public const string HEADER_ICON_MARKUP_KEY = "headerIconMarkup";
        public const string HEADER_CLASS_NAME_FORMAT_KEY = "headerClassNameFormat";
        private readonly HeadingBlockParser _headingBlockParser;
        private readonly SectionsExtensionOptions _sectionsExtensionOptions;
        private readonly JsonOptionsService _jsonOptionsService;
        private readonly AutoLinkService _autoLinkService;
        private readonly IdentifierService _identifierService;

        /// <summary>
        /// Initializes an instance of type <see cref="SectionBlockParser"/>.
        /// </summary>
        /// <param name="sectionsExtensionOptions"></param>
        /// <param name="headingBlockParser"></param>
        /// <param name="autoLinkService"></param>
        /// <param name="identifierService"></param>
        /// <param name="jsonOptionsService"></param>
        public SectionBlockParser(SectionsExtensionOptions sectionsExtensionOptions,
            HeadingBlockParser headingBlockParser,
            AutoLinkService autoLinkService,
            IdentifierService identifierService,
            JsonOptionsService jsonOptionsService)
        {
            OpeningCharacters = new[] { '#' };
            Closed += SectionBlockOnClosed;

            _sectionsExtensionOptions = sectionsExtensionOptions;
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

            var newHeadingBlock = (HeadingBlock)processor.NewBlocks.Peek();

            SectionBlockOptions sectionBlockOptions = CreateSectionOptions(processor, newHeadingBlock.Level);

            // Set heading block icon markup 
            newHeadingBlock.SetData(HEADER_ICON_MARKUP_KEY, sectionBlockOptions.HeaderIconMarkup);
            newHeadingBlock.SetData(HEADER_CLASS_NAME_FORMAT_KEY, sectionBlockOptions.HeaderClassNameFormat);

            // Optionally, don't open a section block (typically useful for an outermost, level 1 heading that will reside in an existing SectioningContentElement)
            if (sectionBlockOptions.WrapperElement.CompareTo(SectioningContentElement.None) <= 0)
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
        /// Creates the <see cref="SectionBlockOptions"/> for the current block.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="blockLevel"></param>
        internal virtual SectionBlockOptions CreateSectionOptions(BlockProcessor processor, int blockLevel)
        {
            SectionBlockOptions result = _sectionsExtensionOptions.DefaultSectionBlockOptions.Clone();

            // Apply JSON options if they exist
            _jsonOptionsService.TryPopulateOptions(processor, result, processor.LineIndex);

            if (result.WrapperElement == SectioningContentElement.Undefined)
            {
                result.WrapperElement = blockLevel == 1 ? _sectionsExtensionOptions.Level1WrapperElement : _sectionsExtensionOptions.Level2PlusWrapperElement;
            }

            return result;
        }

        /// <summary>
        /// Called when a section block is closed. Sets up callbacks that handle section IDs and auto links.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="block"/> does not contain a <see cref="HeadingBlock"/>.</exception>
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
