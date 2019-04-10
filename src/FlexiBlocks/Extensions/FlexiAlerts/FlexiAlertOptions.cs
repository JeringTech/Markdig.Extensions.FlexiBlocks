using Jering.Markdig.Extensions.FlexiBlocks.Options;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jering.Markdig.Extensions.FlexiBlocks.Alerts
{
    /// <summary>
    /// <para>The default implementation of <see cref="IFlexiAlertOptions"/>.</para>
    /// 
    /// <para>Initialization-wise, this class is primarily populated from JSON in <see cref="BlockOptions"/>s. Hence the Newtonsoft.JSON attributes. 
    /// Developers can also manually instantiate this class, typically for use as extension-wide default options.</para>
    /// 
    /// <para>This class is immutable.</para>
    /// </summary>
    public class FlexiAlertOptions : FlexiBlockOptions<IFlexiAlertOptions>, IFlexiAlertOptions
    {
        /// <summary>
        /// Creates a <see cref="FlexiAlertOptions"/>.
        /// </summary>
        /// <param name="blockName">
        /// <para>The <see cref="FlexiAlert" />'s <a href="https://en.bem.info/methodology/naming-convention/#block-name">BEM block name</a>.</para>
        /// <para>In compliance with <a href="https://en.bem.info">BEM methodology</a>, this value is the root element's class as well as the prefix for all other classes.</para>
        /// <para>This value should contain only valid <a href="https://www.w3.org/TR/CSS21/syndata.html#characters">CSS class characters</a>.</para>
        /// <para>If this value is <c>null</c>, the <see cref="FlexiAlert"/> has no classes.</para>
        /// <para>Defaults to "flexi-alert".</para>
        /// </param>
        /// <param name="type">
        /// <para>The <see cref="FlexiAlert"/>'s type.</para>
        /// <para>This value is used in the root element's default <a href="https://en.bem.info/methodology/quick-start/#modifier">modifier class</a>, 
        /// "&lt;<paramref name="blockName"/>&gt;_&lt;<paramref name="type"/>&gt;".</para>
        /// <para>This value is also used to retrieve an icon HTML fragment if <paramref name="iconHtmlFragment"/> is null.</para>
        /// <para>Icon HTML fragments for custom types can be defined in <see cref="FlexiAlertsExtensionOptions.IconHtmlFragments"/>, which contains fragments for types "info", 
        /// "warning" and "critical-warning" by default.</para>
        /// <para>This value should contain only valid <a href="https://www.w3.org/TR/CSS21/syndata.html#characters">CSS class characters</a>.</para>
        /// <para>If this value is <c>null</c>, the root element will have no modifier class and no attempt will be made to retrieve an icon HTML fragment.</para>
        /// <para>Defaults to "info".</para>
        /// </param>
        /// <param name="iconHtmlFragment">
        /// <para>The <see cref="FlexiAlert" />'s icon as a HTML fragment.</para>
        /// <para>A class attribute with value "&lt;<paramref name="blockName"/>&gt;__icon" is added to this fragment's first start tag.</para>
        /// <para>If this value is <c>null</c>, an attempt is made to retrieve a HTML fragment for the <see cref="FlexiAlert"/>'s type from 
        /// <see cref="FlexiAlertsExtensionOptions.IconHtmlFragments"/>, failing which, no icon is rendered.</para>
        /// <para>Defaults to <c>null</c>.</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the <see cref="FlexiAlert"/>'s root element.</para>
        /// <para>If the class attribute is specified, its value is appended to default classes. This facilitates <a href="https://en.bem.info/methodology/quick-start/#mix">BEM mixes</a>.</para>
        /// <para>If this value is <c>null</c>, default classes are still assigned to the root element.</para>
        /// <para>Defaults to <c>null</c>.</para>
        /// </param>
        public FlexiAlertOptions(
            string blockName = "flexi-alert",
            string type = "info",
            string iconHtmlFragment = default,
            IDictionary<string, string> attributes = default) : base(blockName, attributes)
        {
            Type = type;
            IconHtmlFragment = iconHtmlFragment;
        }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual string Type { get; private set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual string IconHtmlFragment { get; private set; }
    }
}
