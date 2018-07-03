namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// Options for a FlexiTableBlock.
    /// </summary>
    public class FlexiTableBlockOptions : IMarkdownObjectOptions<FlexiTableBlockOptions>
    {
        /// <summary>
        /// Gets or set the value used as the name of the element that will wrap td element contents. 
        /// If the value is null, whitespace or an empty string, no wrapper is rendered.
        /// Defaults to "span" for ARIA compatibility - https://www.w3.org/TR/2017/NOTE-wai-aria-practices-1.1-20171214/examples/table/table.html.
        /// </summary>
        public string WrapperElementName { get; set; } = "span";

        /// <summary>
        /// Gets or sets the value used as the attribute name of the td element's attribute that stores its corresponding th's contents.
        /// If the value is null, whitespace or an empty string, no attribute is rendered.
        /// </summary>
        public string LabelAttributeName { get; set; } = "data-label";

        /// <summary>
        /// HTML attributes for the outermost element of the FlexiTableBlock.
        /// </summary>
        public HtmlAttributeDictionary Attributes { get; set; } = new HtmlAttributeDictionary();

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public FlexiTableBlockOptions Clone()
        {
            var result = (FlexiTableBlockOptions)MemberwiseClone();
            result.Attributes = new HtmlAttributeDictionary(Attributes);

            return result;
        }
    }
}
