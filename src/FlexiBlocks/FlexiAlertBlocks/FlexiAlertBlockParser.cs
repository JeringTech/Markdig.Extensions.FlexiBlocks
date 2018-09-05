using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Microsoft.Extensions.Options;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// A parser that creates <see cref="FlexiAlertBlock"/>s from markdown.
    /// </summary>
    public class FlexiAlertBlockParser : FlexiBlockParser
    {
        private readonly FlexiAlertBlocksExtensionOptions _extensionOptions;
        private readonly IFlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Creates a <see cref="FlexiAlertBlockParser"/> instance.
        /// </summary>
        /// <param name="extensionOptionsAccessor">The accessor for <see cref="FlexiAlertBlocksExtensionOptions"/>.</param>
        /// <param name="flexiOptionsBlockService">The service that will handle populating of <see cref="FlexiAlertBlockOptions"/>.</param>
        public FlexiAlertBlockParser(IOptions<FlexiAlertBlocksExtensionOptions> extensionOptionsAccessor,
            IFlexiOptionsBlockService flexiOptionsBlockService)
        {
            OpeningCharacters = new[] { '!' };

            _extensionOptions = extensionOptionsAccessor?.Value ?? new FlexiAlertBlocksExtensionOptions();
            _flexiOptionsBlockService = flexiOptionsBlockService;
        }

        /// <summary>
        /// Opens a <see cref="FlexiAlertBlock"/> if a line begins with 0 to 3 spaces followed by "!".
        /// </summary>
        /// <param name="processor">The block processor for the document that contains a line with first non-white-space character "!".</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if current line has code indent or does not contain a valid <see cref="FlexiAlertBlock" /> type 
        /// (any sequence of characters from the regex set [A-Za-z0-9_-]).
        /// <see cref="BlockState.ContinueDiscard"/> if a <see cref="FlexiAlertBlock"/> is opened.
        /// </returns>
        public override BlockState TryOpenFlexiBlock(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            int initialColumn = processor.Column;
            int initialStart = processor.Start;

            // Skip ! and first whitespace char after !
            if (processor.NextChar().IsSpaceOrTab())
            {
                processor.NextChar();
            }

            // Attempt to retrieve the FlexiAlertBlock's type
            string flexiAlertType = TryGetFlexiAlertType(processor.Line);
            if (flexiAlertType == null)
            {
                // Reset for other parsers
                processor.Column = initialColumn;
                processor.Line.Start = initialStart;

                return BlockState.None;
            }

            // Create block
            var flexiAlertBlock = new FlexiAlertBlock(this)
            {
                Column = initialColumn,
                Span = new SourceSpan(initialStart, processor.Line.End) // Might be the only line, we'll update End if there are more lines
            };
            processor.NewBlocks.Push(flexiAlertBlock);

            // Create options
            flexiAlertBlock.FlexiAlertBlockOptions = CreateFlexiAlertBlockOptions(processor, flexiAlertType);

            return BlockState.ContinueDiscard; // Discard since line already consumed and used as alert type name
        }

        /// <summary>
        /// Continues a <see cref="FlexiAlertBlock"/> if the current line begins with 0 to 3 spaces followed by "!".
        /// </summary>
        /// <param name="processor">The block processor for the <see cref="FlexiAlertBlock"/> to try and continue.</param>
        /// <param name="block">The <see cref="FlexiAlertBlock"/> to try and continue.</param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not begin with the expected characters.
        /// <see cref="BlockState.BreakDiscard"/> if the current line is blank, indicating that the <see cref="FlexiAlertBlock"/> has ended and should be closed.
        /// <see cref="BlockState.Continue"/> if the <see cref="FlexiAlertBlock"/> is still open.
        /// </returns>
        public override BlockState TryContinueFlexiBlock(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            if (processor.CurrentChar != OpeningCharacters[0])
            {
                return processor.IsBlankLine ? BlockState.BreakDiscard : BlockState.None;
            }

            // Skip opening char and first whitespace following it
            if (processor.NextChar().IsSpaceOrTab())
            {
                processor.NextChar();
            }

            block.UpdateSpanEnd(processor.Line.End);

            return BlockState.Continue;
        }

        internal virtual FlexiAlertBlockOptions CreateFlexiAlertBlockOptions(BlockProcessor processor, string alertType)
        {
            FlexiAlertBlockOptions result = _extensionOptions.DefaultBlockOptions.Clone();

            _flexiOptionsBlockService.TryPopulateOptions(processor, result, processor.LineIndex);

            // Set icon markup (precedence - FlexiOptionsBlock > default FlexiAlertBlockOptions > FlexiAlertBlocksExtensionOptions.IconMarkups)
            if (result.IconMarkup == null && _extensionOptions.IconMarkups.TryGetValue(alertType, out string iconMarkup))
            {
                result.IconMarkup = iconMarkup;
            }

            // Add FlexiAlertBlock class
            if (!string.IsNullOrWhiteSpace(result.ClassNameFormat))
            {
                try
                {
                    string alertClass = string.Format(result.ClassNameFormat, alertType.ToLowerInvariant());
                    result.Attributes.Add("class", alertClass);
                }
                catch (FormatException formatException)
                {
                    throw new FlexiBlocksException(processor.NewBlocks.Peek(),
                        string.Format(Strings.FlexiBlocksException_InvalidFormat, nameof(result.ClassNameFormat), result.ClassNameFormat, 1),
                        formatException);
                }
            }

            return result;
        }

        // A FlexiAlertBlock type is simply a sequence of characters from the regex set [A-Za-z0-9_-].
        internal virtual string TryGetFlexiAlertType(StringSlice line)
        {
            if (line.IsEmpty)
            {
                return null;
            }

            int offset = 0;
            while (true)
            {
                char currentChar = line.PeekChar(offset++);

                if (currentChar == '\0')
                {
                    break;
                }

                if (!(currentChar >= 65 && currentChar <= 90 || // A-Z
                    currentChar >= 97 && currentChar <= 122 || // a-z
                    currentChar == '_' ||
                    currentChar == '-'))
                {
                    return null;
                }
            }

            return line.ToString().ToLower();
        }
    }
}
