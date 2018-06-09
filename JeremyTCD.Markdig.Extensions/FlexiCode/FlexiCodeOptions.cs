namespace JeremyTCD.Markdig.Extensions.FlexiCode
{
    public class FlexiCodeOptions : IMarkdownObjectOptions<FlexiCodeOptions>
    {
        /// <summary>
        /// Gets or sets the value used as the flexi code blocks' outer div's class.
        /// If the value is null, whitespace or an empty string, no class is assigned to the flexi code block's outer div.
        /// </summary>
        public string FlexiCodeClassName { get; set; } = "flexi-code";

        /// <summary>
        /// Gets or sets the value used as the markup for the flexi code block's copy icon. 
        /// If the value is null, whitespace or an empty string, no copy icon is rendered.
        /// 
        /// The default SVG is part of the excellent material design icon set - https://material.io/tools/icons/?style=baseline
        /// It is licensed under an Apache License Version 2 license - https://www.apache.org/licenses/LICENSE-2.0.html
        /// </summary>
        public string IconMarkup { get; set; } = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0,0h24v24H0V0z\"/><path d=\"M14,3H6C4.9,3,4,3.9,4,5v11h2V5h8V3z M17,7h-7C8.9,7,8,7.9,8,9v10c0,1.1,0.9,2,2,2h7c1.1,0,2-0.9,2-2V9C19,7.9,18.1,7,17,7zM17,19h-7V9h7V19z\"/></svg>";

        /// <summary>
        /// Gets or sets the value used as the flexi code block's title.
        /// If the value is null, whitespace or an empty string, no title is rendered.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the boolean value indicating whether or not line numbers should be rendered.
        /// </summary>
        public bool RenderLineNumbers { get; set; }

        /// <summary>
        /// Gets or sets the value used as the language for syntax highlighting of the flexi code block's code.
        /// If <see cref="Highlight"/> is true, <see cref="FlexiCodeRenderer"/> highlights the code using PrismJs. 
        /// Otherwise, a class with format <see cref="CodeLanguageClassNameFormat"/> is added to the code element to facilitate 
        /// client side syntax highlighting.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the value used as the format for the flexi code block's code element's language class.
        /// </summary>
        public string CodeLanguageClassNameFormat { get; set; } = "language-{0}";

        /// <summary>
        /// Gets or sets the value indicating whether <see cref="FlexiCodeRenderer"/> should highlight the flexi code block's code.
        /// </summary>
        public bool Highlight { get; set; } = true;

        /// <summary>
        /// HTML attributes.
        /// </summary>
        public HtmlAttributeDictionary Attributes { get; set; } = new HtmlAttributeDictionary();

        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        public FlexiCodeOptions Clone()
        {
            var result = (FlexiCodeOptions)MemberwiseClone();
            result.Attributes = new HtmlAttributeDictionary(Attributes);

            return result;
        }
    }
}
