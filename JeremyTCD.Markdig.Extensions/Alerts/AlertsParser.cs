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
        /// Initializes a new instance of the <see cref="QuoteBlockParser"/> class.
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

        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            int column = processor.Column;
            int initialStart = processor.Line.Start;

            // An alert block begins with 0-3 spaces, followed by !, followed by 0 or 1 spaces.
            char c = processor.NextChar();
            if (c.IsSpaceOrTab())
            {
                processor.NextColumn(); // Skip whitespace
            }

            // Attempt to retrieve alert type
            string alertType = TryGetAlertType(processor.Line);
            if (alertType == null)
            {
                processor.Line.Start = initialStart;
                return BlockState.None;
            }

            // Create options
            AlertBlockOptions alertBlockOptions = CreateAlertOptions(processor, alertType);

            processor.NewBlocks.Push(new AlertBlock(this)
            {
                Column = column,
                Span = new SourceSpan(initialStart, processor.Line.End), // TODO must end be specified here?
                AlertBlockOptions = alertBlockOptions
            });

            return BlockState.ContinueDiscard; // Line already consumed and used as alert type
        }

        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None; // Close alert block and its children
            }

            var alertBlock = (AlertBlock)block;

            if (processor.CurrentChar != OpeningCharacters[0])
            {
                // TODO what about lazy continuation?
                return processor.IsBlankLine ? BlockState.BreakDiscard : BlockState.None;
            }

            char c = processor.NextChar(); // Skip opening char
            if (c.IsSpace())
            {
                processor.NextChar(); // Skip whitespace
            }

            // TODO can't we just do this when the block is closed?
            block.UpdateSpanEnd(processor.Line.End);

            return BlockState.Continue;
        }

        /// <summary>
        /// Creates <see cref="AlertBlockOptions"/> for the current block.
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="alertType"></param>
        internal virtual AlertBlockOptions CreateAlertOptions(BlockProcessor processor, string alertType)
        {
            AlertBlockOptions result = _alertsOptions.DefaultAlertBlockOptions.Clone();

            _jsonOptionsService.TryPopulateOptions(processor, result);

            // Set icon element
            if (result.IconElementMarkup == null && _alertsOptions.IconElementMarkups.TryGetValue(alertType, out string iconElementMarkup))
            {
                result.IconElementMarkup = iconElementMarkup;
            }

            // Add alert class
            string alertClass = $"alert-{alertType}";

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

            // First line must be of the form !<optional space><alert type>, where <alert type> contains at least one character and only characters from the regex set [A-Za-z0-9_-]
        internal virtual string TryGetAlertType(StringSlice line)
        {
            StringSlice duplicateLine = line;
            char c = duplicateLine.CurrentChar;

            while (c != '\0')
            {
                if (c >= 65 && c <= 90 || // A-Z
                    c >= 61 && c <= 122 || // a-z
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
