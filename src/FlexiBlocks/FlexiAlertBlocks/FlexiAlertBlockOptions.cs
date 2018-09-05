namespace Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks
{
    /// <summary>
    /// Represents options for a <see cref="FlexiAlertBlock"/>.
    /// </summary>
    public class FlexiAlertBlockOptions : IMarkdownObjectOptions<FlexiAlertBlockOptions>
    {
        /// <summary>
        /// <para>Gets or sets the markup for the <see cref="FlexiAlertBlock" />'s icon.</para>
        /// If not null, white space or an empty string,
        /// the markup is rendered before the <see cref="FlexiAlertBlock" />'s content.
        /// </summary>
        public string IconMarkup { get; set; }

        /// <summary>
        /// <para>Gets or sets the format for the <see cref="FlexiAlertBlock" />'s outermost element's class.</para>
        /// <para>The <see cref="FlexiAlertBlock" />'s type (the string provided in the first line of the markdown for a <see cref="FlexiAlertBlock" />) 
        /// will replace "{0}" in the format.</para> 
        /// If the format is null, whitespace or an empty string, no class is assigned.
        /// </summary>
        public string ClassNameFormat { get; set; } = "fab-{0}";

        /// <summary>
        /// <para>Gets or sets the class of the <see cref="FlexiAlertBlock" />'s content wrapper.</para>  
        /// If the value is null, whitespace or an empty string, no class is assigned. 
        /// </summary>
        public string ContentClassName { get; set; } = "fab-content";

        /// <summary>
        /// Gets or sets HTML attributes for the outermost element of the <see cref="FlexiAlertBlock" />.
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
