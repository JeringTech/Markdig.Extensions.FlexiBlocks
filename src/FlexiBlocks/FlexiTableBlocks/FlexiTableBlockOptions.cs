using System.Collections.Generic;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Newtonsoft.Json;

namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// <para>An implementation of <see cref="FlexiBlockOptions{T}"/> representing options for a FlexiTableBlock.</para>
    /// 
    /// <para>This class is primarily used through the <see cref="FlexiOptionsBlocksExtension"/>. To that end, this class is designed to be populated from JSON.
    /// This class may occasionally be created manually for use as the default FlexiTableBlock options, so it accomodates manual creation as well.</para>
    /// 
    /// <para>Markdig is designed to be extensible, as a result, any third party extension can access a FlexiTableBlock's options. To prevent inconsistent state, 
    /// this class is immutable.</para>
    /// </summary>
    public class FlexiTableBlockOptions : FlexiBlockOptions<FlexiTableBlockOptions>
    {
        private const string _defaultWrapperElement = "span";
        private const string _defaultLabelAttribute = "data-label";

        /// <summary>
        /// Creates a <see cref="FlexiTableBlockOptions"/> instance.
        /// </summary>
        /// <param name="wrapperElement">
        /// <para>The element that will wrap td contents.</para>
        /// <para>If this value is null, whitespace or an empty string, no wrapper element is rendered.</para>
        /// <para>Defaults to "span" for ARIA compatibility - https://www.w3.org/TR/2017/NOTE-wai-aria-practices-1.1-20171214/examples/table/table.html.</para>
        /// </param>
        /// <param name="labelAttribute">
        /// <para>The td attribute used to store its corresponding th's contents.</para>
        /// <para>If this value is null, whitespace or an empty string, no attribute is rendered.</para>
        /// <para>Defaults to "data-label".</para>
        /// </param>
        /// <param name="attributes">
        /// <para>The HTML attributes for the FlexiTableBlock's outermost element.</para>
        /// <para>If this value is null, no attributes will be assigned to the outermost element.</para>
        /// <para>Defaults to null.</para>
        /// </param>
        public FlexiTableBlockOptions(
            string wrapperElement = _defaultWrapperElement,
            string labelAttribute = _defaultLabelAttribute,
            IDictionary<string, string> attributes = default) : base(attributes)
        {
            WrapperElement = wrapperElement;
            LabelAttribute = labelAttribute;
        }

        /// <summary>
        /// Gets or set the element that will wrap td contents.
        /// </summary>
        [JsonProperty]
        public string WrapperElement { get; private set; }

        /// <summary>
        /// Gets or sets the td attribute used to store its corresponding th's contents.
        /// </summary>
        [JsonProperty]
        public string LabelAttribute { get; private set; }
    }
}
