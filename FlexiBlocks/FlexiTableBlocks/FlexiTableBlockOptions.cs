namespace FlexiBlocks.FlexiTableBlocks
{
    /// <summary>
    /// Options for a responsive table.
    /// </summary>
    public class FlexiTableBlockOptions : IMarkdownObjectOptions<FlexiTableBlockOptions>
    {
        /// <summary>
        /// Name of element used to wrap contents of td elements. Defaults to "span" for ARIA compatibility - https://www.w3.org/TR/2017/NOTE-wai-aria-practices-1.1-20171214/examples/table/table.html.
        /// </summary>
        public string WrapperElementName { get; set; } = "span";

        /// <summary>
        /// Name of attribute used to store a td element's corresponding th's content.
        /// </summary>
        public string LabelAttributeName { get; set; } = "data-label";

        /// <summary>
        /// HTML attributes for the outermost element of the responsive table block.
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
