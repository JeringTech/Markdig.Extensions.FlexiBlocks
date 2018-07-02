namespace FlexiBlocks.Alerts
{
    public class FlexiAlertBlockOptions : IMarkdownObjectOptions<FlexiAlertBlockOptions>
    {
        /// <summary>
        /// Markup for the FlexiAlert's icon. If specified, rendered before the FlexiAlert's content.
        /// </summary>
        public string IconMarkup { get; set; }

        /// <summary>
        /// Gets or sets the value used as the format for the FlexiAlert's outer div's class. 
        /// The inserted string is the FlexiAlert's type (the string provided in the first line of the markdown for a FlexiAlert). 
        /// 
        /// If the format is null, whitespace or an empty string, no class is assigned.
        /// </summary>
        public string ClassNameFormat { get; set; } = "flexi-alert-{0}";

        /// <summary>
        /// Gets or sets the value used as the class of the FlexiAlert's content wrapper. 
        /// If the value is null, whitespace or an empty string, no class is assigned. 
        /// </summary>
        public string ContentClassName { get; set; } = "flexi-alert-content";

        /// <summary>
        /// HTML attributes for the outermost element of the FlexiAlert block.
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
