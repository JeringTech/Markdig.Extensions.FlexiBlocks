using FlexiBlocks.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace FlexiBlocks.Alerts
{
    public class FlexiAlertBlockParser : BlockParser
    {
        private readonly FlexiAlertBlocksExtensionOptions _flexiAlertsExtensionOptions;
        private readonly FlexiOptionBlocksService _flexiOptionsService;

        /// <summary>
        /// Initializes an instance of type <see cref="FlexiAlertBlockParser"/>.
        /// </summary>
        /// <param name="flexiAlertsExtensionOptions"></param>
        /// <param name="flexiOptionsService"></param>
        public FlexiAlertBlockParser(FlexiAlertBlocksExtensionOptions flexiAlertsExtensionOptions,
            FlexiOptionBlocksService flexiOptionsService)
        {
            OpeningCharacters = new[] { '!' };

            _flexiAlertsExtensionOptions = flexiAlertsExtensionOptions;
            _flexiOptionsService = flexiOptionsService;
        }

        /// <summary>
        /// Attempts to open a <see cref="FlexiAlertBlock"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if current line has code indent or does not contain a valid FlexiAlert type (any sequence of characters from the regex set [A-Za-z0-9_-]).
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

            // A FlexiAlert block begins with 0-3 spaces, followed by !, followed by 0 or 1 spaces.
            char c = processor.NextChar();
            if (c.IsSpaceOrTab())
            {
                processor.NextColumn(); // Skip whitespace
            }

            // Attempt to retrieve the FlexiAlert's type
            string flexiAlertType = TryGetFlexiAlertType(processor.Line);
            if (flexiAlertType == null)
            {
                processor.Line.Start = initialStart;
                return BlockState.None;
            }

            // Create options
            FlexiAlertBlockOptions flexiAlertBlockOptions = CreateFlexiAlertBlockOptions(processor, flexiAlertType);

            processor.NewBlocks.Push(new FlexiAlertBlock(this)
            {
                Column = initialColumn,
                Span = new SourceSpan(initialStart, processor.Line.End),
                FlexiAlertBlockOptions = flexiAlertBlockOptions
            });

            return BlockState.ContinueDiscard; // Line already consumed and used as alert type name
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
                return BlockState.None; // Close FlexiAlert block and its children
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
        /// Creates <see cref="FlexiAlertBlockOptions"/> for the current block.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="alertType"></param>
        internal virtual FlexiAlertBlockOptions CreateFlexiAlertBlockOptions(BlockProcessor processor, string alertType)
        {
            FlexiAlertBlockOptions result = _flexiAlertsExtensionOptions.DefaultFlexiAlertBlockOptions.Clone();

            _flexiOptionsService.TryPopulateOptions(processor, result, processor.LineIndex);

            // Set icon markup (precedence - FlexiOptions > default FlexiAlertBlockOptions > FlexiAlertsExtensionOptions.IconMarkups)
            if (result.IconMarkup == null && _flexiAlertsExtensionOptions.IconMarkups.TryGetValue(alertType, out string iconMarkup))
            {
                result.IconMarkup = iconMarkup;
            }

            // Add FlexiAlert class
            if (!string.IsNullOrWhiteSpace(result.ClassNameFormat))
            {
                string alertClass = string.Format(result.ClassNameFormat, alertType.ToLowerInvariant());
                result.Attributes.Add("class", alertClass);
            }

            return result;
        }

        /// <summary>
        /// Retrieves FlexiAlert type from <paramref name="line"/>. A FlexiAlert type is simply a sequence of characters from the regex set [A-Za-z0-9_-].
        /// </summary>
        /// <param name="line"></param>
        /// <returns>
        /// The FlexiAlert type if successful, null otherwise.
        /// </returns>
        internal virtual string TryGetFlexiAlertType(StringSlice line)
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
