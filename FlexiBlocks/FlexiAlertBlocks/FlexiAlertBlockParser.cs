using FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace FlexiBlocks.FlexiAlertBlocks
{
    public class FlexiAlertBlockParser : BlockParser
    {
        private readonly FlexiAlertBlocksExtensionOptions _flexiAlertBlocksExtensionOptions;
        private readonly FlexiOptionsBlockService _flexiOptionsBlockService;

        /// <summary>
        /// Initializes an instance of type <see cref="FlexiAlertBlockParser"/>.
        /// </summary>
        /// <param name="flexiAlertBlocksExtensionOptions"></param>
        /// <param name="flexiOptionsBlockService"></param>
        public FlexiAlertBlockParser(FlexiAlertBlocksExtensionOptions flexiAlertBlocksExtensionOptions,
            FlexiOptionsBlockService flexiOptionsBlockService)
        {
            OpeningCharacters = new[] { '!' };

            _flexiAlertBlocksExtensionOptions = flexiAlertBlocksExtensionOptions;
            _flexiOptionsBlockService = flexiOptionsBlockService;
        }

        /// <summary>
        /// Attempts to open a <see cref="FlexiAlertBlock"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if current line has code indent or does not contain a valid FlexiAlertBlock type (any sequence of characters from the regex set [A-Za-z0-9_-]).
        /// <see cref="BlockState.ContinueDiscard"/> if a <see cref="FlexiAlertBlock"/> is opened.
        /// </returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            int initialColumn = processor.Column;
            int initialStart = processor.Line.Start;

            // A FlexiAlertBlock begins with 0-3 spaces, followed by !, followed by 0 or 1 spaces.
            char c = processor.NextChar();
            if (c.IsSpaceOrTab())
            {
                processor.NextColumn(); // Skip whitespace
            }

            // Attempt to retrieve the FlexiAlertBlock's type
            string flexiAlertBlockType = TryGetFlexiAlertBlockType(processor.Line);
            if (flexiAlertBlockType == null)
            {
                processor.Line.Start = initialStart;
                return BlockState.None;
            }

            // Create options
            FlexiAlertBlockOptions flexiAlertBlockOptions = CreateFlexiAlertBlockOptions(processor, flexiAlertBlockType);

            processor.NewBlocks.Push(new FlexiAlertBlock(this)
            {
                Column = initialColumn,
                Span = new SourceSpan(initialStart, processor.Line.End),
                FlexiAlertBlockOptions = flexiAlertBlockOptions
            });

            return BlockState.ContinueDiscard; // Discard since line already consumed and used as alert type name
        }

        /// <summary>
        /// Attempts to continue a <see cref="FlexiAlertBlock"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not being with '!'.
        /// <see cref="BlockState.BreakDiscard"/> if the current line is blank.
        /// <see cref="BlockState.Continue"/> if <paramref name="block"/> can be continued.
        /// </returns>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None; // Close FlexiAlertBlock and its children
            }

            if (processor.CurrentChar != OpeningCharacters[0])
            {
                return processor.IsBlankLine ? BlockState.BreakDiscard : BlockState.None;
            }

            char c = processor.NextChar(); // Skip opening char
            if (c.IsSpace())
            {
                processor.NextChar(); // Skip whitespace
            }

            block.UpdateSpanEnd(processor.Line.End);

            return BlockState.Continue;
        }

        /// <summary>
        /// Creates the <see cref="FlexiAlertBlockOptions"/> for the current block.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="alertType"></param>
        internal virtual FlexiAlertBlockOptions CreateFlexiAlertBlockOptions(BlockProcessor processor, string alertType)
        {
            FlexiAlertBlockOptions result = _flexiAlertBlocksExtensionOptions.DefaultFlexiAlertBlockOptions.Clone();

            _flexiOptionsBlockService.TryPopulateOptions(processor, result, processor.LineIndex);

            // Set icon markup (precedence - FlexiOptionsBlock > default FlexiAlertBlockOptions > FlexiAlertBlocksExtensionOptions.IconMarkups)
            if (result.IconMarkup == null && _flexiAlertBlocksExtensionOptions.IconMarkups.TryGetValue(alertType, out string iconMarkup))
            {
                result.IconMarkup = iconMarkup;
            }

            // Add FlexiAlertBlock class
            if (!string.IsNullOrWhiteSpace(result.ClassNameFormat))
            {
                string alertClass = string.Format(result.ClassNameFormat, alertType.ToLowerInvariant());
                result.Attributes.Add("class", alertClass);
            }

            return result;
        }

        /// <summary>
        /// Retrieves FlexiAlertBlock type from <paramref name="line"/>. A FlexiAlertBlock type is simply a sequence of characters from the regex set [A-Za-z0-9_-].
        /// </summary>
        /// <param name="line"></param>
        /// <returns>
        /// The FlexiAlertBlock type if successful, null otherwise.
        /// </returns>
        internal virtual string TryGetFlexiAlertBlockType(StringSlice line)
        {
            StringSlice duplicateLine = line;
            char c = duplicateLine.CurrentChar;

            while (c != '\0')
            {
                if (c >= 65 && c <= 90 || // A-Z
                    c >= 97 && c <= 122 || // a-z
                    c == '_' ||
                    c == '-')
                {
                    c = duplicateLine.NextChar();
                }
                else
                {
                    return null;
                }
            }

            // At least one character
            if (duplicateLine.Start == line.Start)
            {
                return null;
            }

            return line.ToString().ToLower();
        }
    }
}
