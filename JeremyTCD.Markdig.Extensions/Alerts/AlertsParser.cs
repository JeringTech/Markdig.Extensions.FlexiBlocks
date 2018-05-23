using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace JeremyTCD.Markdig.Extensions.Alerts
{
    public class AlertsParser : BlockParser
    {
        private readonly AlertsOptions _alertsOptions;
        private readonly JsonOptionsService _jsonOptionsService;

        /// <summary>
        /// Initializes an instance of type <see cref="AlertsParser"/>.
        /// </summary>
        /// <param name="alertsOptions"></param>
        /// <param name="jsonOptionsService"></param>
        public AlertsParser(AlertsOptions alertsOptions,
            JsonOptionsService jsonOptionsService)
        {
            OpeningCharacters = new[] { '!' };

            _alertsOptions = alertsOptions;
            _jsonOptionsService = jsonOptionsService;
        }

        /// <summary>
        /// Attempts to open an <see cref="AlertBlock"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if current line has code indent or current line does not contain a valid alert type name (any sequence of characters from the regex set [A-Za-z0-9_-]).
        /// <see cref="BlockState.ContinueDiscard"/> if an <see cref="AlertBlock"/> is opened.
        /// </returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            int initialColumn = processor.Column;
            int initialStart = processor.Line.Start;

            // An alert block begins with 0-3 spaces, followed by !, followed by 0 or 1 spaces.
            char c = processor.NextChar();
            if (c.IsSpaceOrTab())
            {
                processor.NextColumn(); // Skip whitespace
            }

            // Attempt to retrieve alert type name
            string alertTypeName = TryGetAlertTypeName(processor.Line);
            if (alertTypeName == null)
            {
                processor.Line.Start = initialStart;
                return BlockState.None;
            }

            // Create options
            AlertBlockOptions alertBlockOptions = CreateAlertOptions(processor, alertTypeName);

            processor.NewBlocks.Push(new AlertBlock(this)
            {
                Column = initialColumn,
                Span = new SourceSpan(initialStart, processor.Line.End),
                AlertBlockOptions = alertBlockOptions
            });

            return BlockState.ContinueDiscard; // Line already consumed and used as alert type name
        }

        /// <summary>
        /// Attempts to continue an <see cref="AlertBlock"/>.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="block"></param>
        /// <returns>
        /// <see cref="BlockState.None"/> if the current line has code indent or if the current line does not being with '!'.
        /// <see cref="BlockState.BreakDiscard"/> if the current line is blank.
        /// <see cref="BlockState.Continue"/> if successful.
        /// </returns>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None; // Close alert block and its children
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
        /// Creates <see cref="AlertBlockOptions"/> for the current block.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="alertTypeName"></param>
        internal virtual AlertBlockOptions CreateAlertOptions(BlockProcessor processor, string alertTypeName)
        {
            AlertBlockOptions result = _alertsOptions.DefaultAlertBlockOptions.Clone();

            _jsonOptionsService.TryPopulateOptions(processor, result);

            // Set icon element (precedence - JSON options > default AlertBlockOptions > AlertOptions.IconMarkups)
            if (result.IconMarkup == null && _alertsOptions.IconMarkups.TryGetValue(alertTypeName, out string iconMarkup))
            {
                result.IconMarkup = iconMarkup;
            }

            // Add alert class
            string alertClass = $"alert-{alertTypeName}";

            if (result.Attributes.TryGetValue("class", out string existingClasses) && !string.IsNullOrWhiteSpace(existingClasses))
            {
                result.Attributes["class"] = $"{existingClasses} {alertClass}";
            }
            else
            {
                result.Attributes["class"] = alertClass;
            }

            return result;
        }

        /// <summary>
        /// Retrieves alert type name from <paramref name="line"/>. An alert type name is simply a sequence of characters from the regex set [A-Za-z0-9_-].
        /// </summary>
        /// <param name="line"></param>
        /// <returns>
        /// The alert type name if successful, null otherwise.
        /// </returns>
        internal virtual string TryGetAlertTypeName(StringSlice line)
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
