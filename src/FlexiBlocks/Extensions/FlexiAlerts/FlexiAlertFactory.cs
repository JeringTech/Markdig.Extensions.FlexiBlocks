using Jering.Markdig.Extensions.FlexiBlocks.Options;
using Markdig.Parsers;
using Markdig.Syntax;
using System;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// The default implementation of <see cref="BlockFactory{FlexiAlert}"/>.
    /// </summary>
    public class FlexiAlertFactory : BlockFactory<FlexiAlert>
    {
        private readonly IFlexiBlockOptionsFactory _flexiBlockOptionsFactory;
        private readonly IFlexiAlertsExtensionOptionsFactory _flexiAlertsExtensionOptionsFactory;

        /// <summary>
        /// Creates a <see cref="FlexiAlertFactory"/>.
        /// </summary>
        /// <param name="flexiBlockOptionsFactory">The factory for building <see cref="IFlexiAlertOptions"/>.</param>
        /// <param name="flexiAlertsExtensionOptionsFactory">The factory for building <see cref="IFlexiAlertsExtensionOptions"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="flexiBlockOptionsFactory"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="flexiAlertsExtensionOptionsFactory"/> is null.</exception>
        public FlexiAlertFactory(IFlexiBlockOptionsFactory flexiBlockOptionsFactory, IFlexiAlertsExtensionOptionsFactory flexiAlertsExtensionOptionsFactory)
        {
            _flexiBlockOptionsFactory = flexiBlockOptionsFactory ?? throw new ArgumentNullException(nameof(flexiBlockOptionsFactory));
            _flexiAlertsExtensionOptionsFactory = flexiAlertsExtensionOptionsFactory ?? throw new ArgumentNullException(nameof(flexiAlertsExtensionOptionsFactory));
        }

        /// <inheritdoc />
        protected override FlexiAlert CreateBlock(BlockProcessor blockProcessor, BlockParser<FlexiAlert> blockParser = null)
        {
            IFlexiAlertsExtensionOptions flexiAlertsExtensionOptions = _flexiAlertsExtensionOptionsFactory.Create(blockProcessor);
            IFlexiAlertOptions flexiAlertOptions = _flexiBlockOptionsFactory.Create(flexiAlertsExtensionOptions.DefaultBlockOptions, blockProcessor);

            string iconHtmlFragment = GetIconHtmlFragment(flexiAlertOptions, flexiAlertsExtensionOptions);

            return new FlexiAlert(iconHtmlFragment, flexiAlertOptions, blockParser)
            {
                Column = blockProcessor.Column,
                Span = new SourceSpan(blockProcessor.Start, blockProcessor.Line.End) // Might be the only line, FlexiAlertBlockParser.TryContinueFlexiBlock will update End if there are more lines
            };
        }

        internal virtual string GetIconHtmlFragment(IFlexiAlertOptions flexiAlertOptions, IFlexiAlertsExtensionOptions flexiAlertsExtensionOptions)
        {
            string result = flexiAlertOptions.IconHtmlFragment;
            if (result == null && flexiAlertOptions.Type != null)
            {
                flexiAlertsExtensionOptions.IconHtmlFragments.TryGetValue(flexiAlertOptions.Type, out result);
            }

            return result;
        }
    }
}
