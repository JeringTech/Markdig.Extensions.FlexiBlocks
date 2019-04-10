using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig;
using Markdig.Parsers;
using System;
using System.IO;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// <para>The default implementation of <see cref="IFlexiAlertsExtensionOptionsFactory"/>.</para>
    /// <para>This factory allows extension options to be specified when:
    /// <para>- Creating a <see cref="MarkdownPipelineBuilder"/> using <see cref="MarkdownPipelineBuilderExtensions"/>.</para>
    /// <para>- Using a <see cref="MarkdownPipeline"/> - extension options can be added to <see cref="MarkdownParserContext.Properties"/> 
    /// when calling methods like <see cref="Markdown.ToHtml(string, TextWriter, MarkdownPipeline, MarkdownParserContext)"/>.</para>
    /// <para>If extension options aren't specified using these means, default options are returned.</para>
    /// </para>
    /// </summary>
    public class FlexiAlertsExtensionOptionsFactory : IFlexiAlertsExtensionOptionsFactory
    {
        private readonly IContextObjectsService _contextObjectsService;
        private FlexiAlertsExtensionOptions _defaultFlexiAlertsExtensionOptions; // Immutable, so thread-safe

        /// <summary>
        /// Creates a <see cref="FlexiAlertsExtensionOptionsFactory"/>.
        /// </summary>
        /// <param name="contextObjectService">The service used to try retrieve a <see cref="IFlexiAlertsExtensionOptions"/> from a <see cref="BlockProcessor"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contextObjectService"/> is null.</exception>
        public FlexiAlertsExtensionOptionsFactory(IContextObjectsService contextObjectService)
        {
            _contextObjectsService = contextObjectService ?? throw new ArgumentNullException(nameof(contextObjectService));
        }

        /// <inheritdoc />
        public IFlexiAlertsExtensionOptions Create(BlockProcessor blockProcessor)
        {
            if (_contextObjectsService.TryGetContextObject(typeof(IFlexiAlertsExtensionOptions), blockProcessor, out object value) &&
                value is IFlexiAlertsExtensionOptions valueAsIFlexiAlertsExtensionOptions)
            {
                return valueAsIFlexiAlertsExtensionOptions;
            }

            _defaultFlexiAlertsExtensionOptions = _defaultFlexiAlertsExtensionOptions ?? new FlexiAlertsExtensionOptions(); // Might have been instantiated during an earlier Markdown.ToHtml run
            _contextObjectsService.TrySetContextObject(typeof(IFlexiAlertsExtensionOptions), _defaultFlexiAlertsExtensionOptions, blockProcessor);

            return _defaultFlexiAlertsExtensionOptions;
        }
    }
}