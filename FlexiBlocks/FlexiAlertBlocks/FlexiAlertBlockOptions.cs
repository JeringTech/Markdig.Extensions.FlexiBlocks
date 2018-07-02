namespace FlexiBlocks.Alerts
{
    public class FlexiAlertBlockOptions : IMarkdownObjectOptions<FlexiAlertBlockOptions>
    {
        /// <summary>
        /// Markup for the FlexiAlertBlock's icon. If specified, rendered before the FlexiAlertBlock's content.
        /// </summary>
        public string IconMarkup { get; set; }

        /// <summary>
        /// Gets or sets the value used as the format for the FlexiAlertBlock's outermost element's class. 
        /// The inserted string is the FlexiAlertBlock's type (the string provided in the first line of the markdown for a FlexiAlertBlock). 
        /// 
        /// If the format is null, whitespace or an empty string, no class is assigned.
        /// </summary>
        public string ClassNameFormat { get; set; } = "fab-{0}";

        /// <summary>
        /// Gets or sets the value used as the class of the FlexiAlertBlock's content wrapper. 
        /// If the value is null, whitespace or an empty string, no class is assigned. 
        /// </summary>
        public string ContentClassName { get; set; } = "fab-content";

        /// <summary>
        /// HTML attributes for the outermost element of the FlexiAlertBlock.
        /// </summary>
        public HtmlAttributeDictionary Attributes { get; set; } = new HtmlAttributeDictionary();

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public FlexiAlertBlockOptions Clone()
        {
            var result = (FlexiAlertBlockOptions)MemberwiseClone();
            result.Attributes = new HtmlAttributeDictionary(Attributes);

            return result;
        }
    }
}
