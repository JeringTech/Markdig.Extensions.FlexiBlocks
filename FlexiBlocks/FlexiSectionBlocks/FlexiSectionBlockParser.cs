using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Linq;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks
{
    public class FlexiSectionBlockParser : BlockParser
    {
        public const string HEADER_ICON_MARKUP_KEY = "headerIconMarkup";
        public const string HEADER_CLASS_NAME_FORMAT_KEY = "headerClassNameFormat";
        private readonly HeadingBlockParser _headingBlockParser;
        private readonly FlexiSectionBlocksExtensionOptions _flexiSectionBlocksExtensionOptions;
        private readonly FlexiOptionsBlockService _flexiOptionsBlockService;
        private readonly AutoLinkService _autoLinkService;
        private readonly IdentifierService _identifierService;

        /// <summary>
        /// Initializes an instance of type <see cref="FlexiSectionBlockParser"/>.
        /// </summary>
        /// <param name="flexiSectionBlocksExtensionOptions"></param>
        /// <param name="headingBlockParser"></param>
        /// <param name="autoLinkService"></param>
        /// <param name="identifierService"></param>
        /// <param name="flexiOptionsBlockService"></param>
        public FlexiSectionBlockParser(FlexiSectionBlocksExtensionOptions flexiSectionBlocksExtensionOptions,
            HeadingBlockParser headingBlockParser,
            AutoLinkService autoLinkService,
            IdentifierService identifierService,
            FlexiOptionsBlockService flexiOptionsBlockService)
        {
            OpeningCharacters = new[] { '#' };
            Closed += FlexiSectionBlockOnClosed;

            _flexiSectionBlocksExtensionOptions = flexiSectionBlocksExtensionOptions;
            _headingBlockParser = headingBlockParser;
            _autoLinkService = autoLinkService;
            _identifierService = identifierService;
            _flexiOptionsBlockService = flexiOptionsBlockService;
        }

        /// <summary>
        /// Attempts to open a <see cref="HeadingBlock"/>. If a <see cref="HeadingBlock"/> is be opened, optionally wraps it in a <see cref="FlexiSectionBlock"/>.
        /// Adds any new blocks to <paramref name="processor"/>'s NewBlocks property.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if a <see cref="HeadingBlock"/> cannot be opened.
        /// <see cref="BlockState.Break"/> if a <see cref="HeadingBlock"/> is opened but a <see cref="FlexiSectionBlock"/> is not opened (closes the <see cref="HeadingBlock"/>).
        /// <see cref="BlockState.Continue"/> if a <see cref="FlexiSectionBlock"/> is opened.
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

            FlexiSectionBlockOptions flexiSectionBlockOptions = CreateFlexiSectionBlockOptions(processor, newHeadingBlock.Level);

            // TODO just add a reference to the entire flexiSectionBlockOptions object?
            // Set heading block icon markup 
            newHeadingBlock.SetData(HEADER_ICON_MARKUP_KEY, flexiSectionBlockOptions.HeaderIconMarkup);
            newHeadingBlock.SetData(HEADER_CLASS_NAME_FORMAT_KEY, flexiSectionBlockOptions.HeaderClassNameFormat);

            // Optionally, don't open a section block (typically useful for an outermost, level 1 heading that will reside in an existing SectioningContentElement)
            if (flexiSectionBlockOptions.WrapperElement.CompareTo(SectioningContentElement.None) <= 0)
            {
                // Close heading block
                return BlockState.Break;
            }

            var flexiSectionBlock = new FlexiSectionBlock(this)
            {
                Level = newHeadingBlock.Level,
                FlexiSectionBlockOptions = flexiSectionBlockOptions,
                Column = newHeadingBlock.Column,
                Span = newHeadingBlock.Span
            };

            processor.NewBlocks.Push(flexiSectionBlock);

            // Keep FlexiSectionBlock open (note that this keeps the heading block open unecessarily, TryContinue remedies this, but should look for a cleaner solution)
            return BlockState.Continue;
        }

        /// <summary>
        /// Attempts to continue a <see cref="FlexiSectionBlock"/>.
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

            // Current line is not a HeadingBlock
            if (headingBlockState == BlockState.None)
            {
                return BlockState.Continue;
            }

            // TODO creating then removing a new HeadingBlock instance isn't efficient, should extract logic for determining if a line is a heading.
            var newHeadingBlock = (HeadingBlock)processor.NewBlocks.Pop();

            // Return to initial column so that TryOpen can open the HeadingBlock
            processor.GoToColumn(initialColumn);

            var flexiSectionBlock = block as FlexiSectionBlock;

            // New section is a sibling or an uncle of flexiSectionBlock
            if (newHeadingBlock.Level <= flexiSectionBlock.Level)
            {
                // Close flexiSectionBlock and all of its children, but don't discard line so that a new FlexiSectionBlock is opened
                return BlockState.None;
            }

            // New section is a child of flexiSectionBlock, keep flexiSectionBlock open
            return BlockState.Continue;
        }

        /// <summary>
        /// Creates the <see cref="FlexiSectionBlockOptions"/> for the current block.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="blockLevel"></param>
        internal virtual FlexiSectionBlockOptions CreateFlexiSectionBlockOptions(BlockProcessor processor, int blockLevel)
        {
            FlexiSectionBlockOptions result = _flexiSectionBlocksExtensionOptions.DefaultFlexiSectionBlockOptions.Clone();

            // Apply FlexiOptionsBlock options if they exist
            _flexiOptionsBlockService.TryPopulateOptions(processor, result, processor.LineIndex);

            if (result.WrapperElement == SectioningContentElement.Undefined)
            {
                result.WrapperElement = blockLevel == 1 ? _flexiSectionBlocksExtensionOptions.Level1WrapperElement : _flexiSectionBlocksExtensionOptions.Level2PlusWrapperElement;
            }

            return result;
        }

        /// <summary>
        /// Called when a <see cref="FlexiSectionBlock"/> is closed. Sets up callbacks that handle identifier generation and auto linking.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="block"/> does not contain a <see cref="HeadingBlock"/>.</exception>
        internal virtual void FlexiSectionBlockOnClosed(BlockProcessor processor, Block block)
        {
            var flexiSectionBlock = (FlexiSectionBlock)block;
            FlexiSectionBlockOptions flexiSectionBlockOptions = flexiSectionBlock.FlexiSectionBlockOptions;

            // Setup identifier generation and auto links
            if (flexiSectionBlockOptions.GenerateIdentifier)
            {
                var headingBlock = (HeadingBlock)flexiSectionBlock.FirstOrDefault(child => child is HeadingBlock);

                if (headingBlock == null)
                {
                    throw new InvalidOperationException("A FlexiSectionBlock must contain a heading block.");
                }

                _identifierService.SetupIdentifierGeneration(headingBlock);

                if (flexiSectionBlockOptions.AutoLinkable)
                {
                    _autoLinkService.SetupAutoLink(processor, flexiSectionBlock, headingBlock);
                }
            }
        }
    }
}
