using FlexiBlocks.FlexiCode;
using JeremyTCD.WebUtils.SyntaxHighlighters.HighlightJS;
using JeremyTCD.WebUtils.SyntaxHighlighters.Prism;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FlexiBlocks.Tests.FlexiCode
{
    public class FlexiCodeBlockRendererIntegrationTests : IDisposable
    {
        private ServiceProvider _serviceProvider;

        [Theory]
        [MemberData(nameof(Write_RendersAttributesIfAnyAreSpecified_Data))]
        public void Write_RendersAttributesIfAnyAreSpecified(SerializableWrapper<HtmlAttributeDictionary> dummyAttributesWrapper,
            string expectedDivStartTag)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                Attributes = dummyAttributesWrapper.Value,
                CopyIconMarkup = null,
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"{expectedDivStartTag}
<header>
</header>
<pre><code></code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersAttributesIfAnyAreSpecified_Data()
        {
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";

            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<HtmlAttributeDictionary>(
                        new HtmlAttributeDictionary() {{ dummyAttribute, dummyAttributeValue }}
                    ),
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\">"
                },
                // Empty
                new object[]{
                    new SerializableWrapper<HtmlAttributeDictionary>(
                        new HtmlAttributeDictionary()
                    ),
                    "<div>"
                },
                // Null
                new object[]{
                    new SerializableWrapper<HtmlAttributeDictionary>(
                        null
                    ),
                    "<div>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void Write_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString(string title, string expectedHeaderElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                Title = title,
                CopyIconMarkup = null,
                Attributes = null
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
{expectedHeaderElement}
<pre><code></code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyTitle = "dummyTitle";

            return new object[][]
            {
                new object[]{
                    dummyTitle,
                    $"<header>\n<span>{dummyTitle}</span>\n</header>"
                },
                // Empty
                new object[]{
                    string.Empty,
                    "<header>\n</header>"
                },
                // Null
                new object[]{
                    null,
                    "<header>\n</header>"
                },
                // Whitespace
                new object[]{
                    " ",
                    "<header>\n</header>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void Write_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString(string dummyCopyIconMarkup, string expectedHeaderElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                CopyIconMarkup = dummyCopyIconMarkup,
                Attributes = null
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
{expectedHeaderElement}
<pre><code></code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyCopyIconMarkup = "dummyCopyIconMarkup";

            return new object[][]
            {
                new object[]{
                    dummyCopyIconMarkup,
                    $"<header>\n{dummyCopyIconMarkup}\n</header>"
                },
                // Empty
                new object[]{
                    string.Empty,
                    "<header>\n</header>"
                },
                // Null
                new object[]{
                    null,
                    "<header>\n</header>"
                },
                // Whitespace
                new object[]{
                    " ",
                    "<header>\n</header>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_RendersCodeLanguageClassIfLanguageAndCodeLanguageClassNameFormatAreNotNullWhitespaceOrEmptyStrings_Data))]
        public void Write_RendersCodeLanguageClassIfLanguageAndCodeLanguageClassNameFormatAreNotNullWhitespaceOrEmptyStrings(string dummyLanguage,
            string dummyCodeLanguageClassNameFormat,
            string expectedCodeElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                Language = dummyLanguage,
                CodeLanguageClassNameFormat = dummyCodeLanguageClassNameFormat,
                CopyIconMarkup = null,
                Attributes = null
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
<header>
</header>
<pre>{expectedCodeElement}</pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersCodeLanguageClassIfLanguageAndCodeLanguageClassNameFormatAreNotNullWhitespaceOrEmptyStrings_Data()
        {
            const string dummyLanguage = "dummyLanguage";
            const string dummyLanguageFormat = "dummyLanguageFormat-{0}";

            return new object[][]
            {
                new object[]{
                    dummyLanguage,
                    dummyLanguageFormat,
                    $"<code class=\"{string.Format(dummyLanguageFormat, dummyLanguage)}\"></code>"
                },
                // Language == string.Empty
                new object[]{
                    string.Empty,
                    dummyLanguageFormat,
                    "<code></code>"
                },
                // Format == string.Empty
                new object[]{
                    dummyLanguage,
                    string.Empty,
                    "<code></code>"
                },
                // Language == null
                new object[]{
                    null,
                    dummyLanguageFormat,
                    "<code></code>"
                },
                // Format == null
                new object[]{
                    dummyLanguage,
                    null,
                    "<code></code>"
                },
                // Langauge == whitespace
                new object[]{
                    " ",
                    dummyLanguageFormat,
                    "<code></code>"
                },
                // Format == whitespace
                new object[]{
                    dummyLanguage,
                    " ",
                    "<code></code>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_HighlightsSyntaxIfHighlightSyntaxIsTrueAndLanguageIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void Write_HighlightsSyntaxIfHighlightSyntaxIsTrueAndLanguageIsNotNullWhitespaceOrAnEmptyString(string dummyLanguage,
            bool dummyHighlightSyntax,
            SyntaxHighlighter dummySyntaxHighlighter,
            string dummyCode,
            string expectedCode)
        {
            // Arrange
            var lines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = lines };
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                SyntaxHighlighter = dummySyntaxHighlighter,
                Language = dummyLanguage,
                HighlightSyntax = dummyHighlightSyntax,
                CopyIconMarkup = null,
                Attributes = null,
                CodeLanguageClassNameFormat = null
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
<header>
</header>
<pre><code>{expectedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_HighlightsSyntaxIfHighlightSyntaxIsTrueAndLanguageIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyCSharpCode = @"public string ExampleFunction(string arg)
{
    // Example comment
    return arg + ""dummyString"";
}";
            const string dummyJavascriptCode = @"function exampleFunction(arg) {
    // Example comment
    return arg + 'dummyString';
}";

            return new object[][]
            {
                // Prism, csharp
                new object[]{
                    "csharp",
                    true,
                    SyntaxHighlighter.Prism,
                    dummyCSharpCode,
                    @"<span class=""token keyword"">public</span> <span class=""token keyword"">string</span> <span class=""token function"">ExampleFunction</span><span class=""token punctuation"">(</span><span class=""token keyword"">string</span> arg<span class=""token punctuation"">)</span>
<span class=""token punctuation"">{</span>
    <span class=""token comment"">// Example comment</span>
    <span class=""token keyword"">return</span> arg <span class=""token operator"">+</span> <span class=""token string"">""dummyString""</span><span class=""token punctuation"">;</span>
<span class=""token punctuation"">}</span>"
                },
                // Prism, javascript
                new object[]{
                    "javascript",
                    true,
                    SyntaxHighlighter.Prism,
                    dummyJavascriptCode,
                    @"<span class=""token keyword"">function</span> <span class=""token function"">exampleFunction</span><span class=""token punctuation"">(</span>arg<span class=""token punctuation"">)</span> <span class=""token punctuation"">{</span>
    <span class=""token comment"">// Example comment</span>
    <span class=""token keyword"">return</span> arg <span class=""token operator"">+</span> <span class=""token string"">'dummyString'</span><span class=""token punctuation"">;</span>
<span class=""token punctuation"">}</span>"
                },
                // HighlightJS, csharp
                new object[]{
                    "csharp",
                    true,
                    SyntaxHighlighter.HighlightJS,
                    dummyCSharpCode,
                    @"<span class=""hljs-function""><span class=""hljs-keyword"">public</span> <span class=""hljs-keyword"">string</span> <span class=""hljs-title"">ExampleFunction</span>(<span class=""hljs-params""><span class=""hljs-keyword"">string</span> arg</span>)
</span>{
    <span class=""hljs-comment"">// Example comment</span>
    <span class=""hljs-keyword"">return</span> arg + <span class=""hljs-string"">""dummyString""</span>;
}"
                },
                // HighlightJS, javascript
                new object[]{
                    "javascript",
                    true,
                    SyntaxHighlighter.HighlightJS,
                    dummyJavascriptCode,
                    @"<span class=""hljs-function""><span class=""hljs-keyword"">function</span> <span class=""hljs-title"">exampleFunction</span>(<span class=""hljs-params"">arg</span>) </span>{
    <span class=""hljs-comment"">// Example comment</span>
    <span class=""hljs-keyword"">return</span> arg + <span class=""hljs-string"">'dummyString'</span>;
}"
                },
                // HighlightSyntax = false
                new object[]{
                    "javascript",
                    false,
                    SyntaxHighlighter.HighlightJS,
                    dummyJavascriptCode,
                    dummyJavascriptCode
                },
                // Language = null
                new object[]{
                    null,
                    true,
                    SyntaxHighlighter.HighlightJS,
                    dummyJavascriptCode,
                    dummyJavascriptCode
                },
                // Language == string.Empty
                new object[]{
                    string.Empty,
                    true,
                    SyntaxHighlighter.HighlightJS,
                    dummyJavascriptCode,
                    dummyJavascriptCode
                },
                // Langauge == whitespace
                new object[]{
                    " ",
                    true,
                    SyntaxHighlighter.HighlightJS,
                    dummyJavascriptCode,
                    dummyJavascriptCode
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_RendersLineNumbersIfRenderLineNumbersIsTrue_Data))]
        public void Write_RendersLineNumbersIfRenderLineNumbersIsTrue(bool dummyRenderLineNumbers,
            SerializableWrapper<List<LineNumberRange>> dummyLineNumberRangesWrapper,
            string expectedCode)
        {
            // Arrange
            const string dummyCode = @"public string ExampleFunction(string arg)
{
    // Example comment
    return arg + ""dummyString"";
}";
            var lines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = lines };
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                LineNumberRanges = dummyLineNumberRangesWrapper.Value,
                RenderLineNumbers = dummyRenderLineNumbers,
                CopyIconMarkup = null,
                Attributes = null,
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
<header>
</header>
<pre><code>{expectedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_RendersLineNumbersIfRenderLineNumbersIsTrue_Data()
        {

            const string defaultLineNumbers = @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">public string ExampleFunction(string arg)</span></span>
<span class=""line""><span class=""line-number"">2</span><span class=""line-text"">{</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">    // Example comment</span></span>
<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">    return arg + &quot;dummyString&quot;;</span></span>
<span class=""line""><span class=""line-number"">5</span><span class=""line-text"">}</span></span>";

            return new object[][]
            {
                // LineNumberRanges = null (default line number ranges)
                new object[]{
                    true,
                    new SerializableWrapper<List<LineNumberRange>>(
                        null
                    ),
                    defaultLineNumbers
                },
                // LineNumberRanges.Count = 0 (default line number ranges)
                new object[]{
                    true,
                    new SerializableWrapper<List<LineNumberRange>>(
                        new List<LineNumberRange>()
                    ),
                    defaultLineNumbers
                },
                // LineNumberRanges.Count > 0
                new object[]{
                    true,
                    new SerializableWrapper<List<LineNumberRange>>(
                        new List<LineNumberRange>{new LineNumberRange(1, -1, 4)}
                    ),
                    @"<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">public string ExampleFunction(string arg)</span></span>
<span class=""line""><span class=""line-number"">5</span><span class=""line-text"">{</span></span>
<span class=""line""><span class=""line-number"">6</span><span class=""line-text"">    // Example comment</span></span>
<span class=""line""><span class=""line-number"">7</span><span class=""line-text"">    return arg + &quot;dummyString&quot;;</span></span>
<span class=""line""><span class=""line-number"">8</span><span class=""line-text"">}</span></span>"
                },
                // RenderLineNumbers = false
                new object[]{
                    false,
                    new SerializableWrapper<List<LineNumberRange>>(
                        null
                    ),
                    @"public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}"
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_HighlightsLinesIfHighlightLineRangesIsNotNullOrEmpty_Data))]
        public void Write_HighlightsLinesIfHighlightLineRangesIsNotNullOrEmpty(SerializableWrapper<List<LineRange>> dummyHighlightLineRangesWrapper,
            string expectedCode)
        {
            // Arrange
            const string dummyCode = @"public string ExampleFunction(string arg)
{
    // Example comment
    return arg + ""dummyString"";
}";
            var lines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = lines };
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                HighlightLineRanges = dummyHighlightLineRangesWrapper.Value,
                CopyIconMarkup = null,
                Attributes = null,
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
<header>
</header>
<pre><code>{expectedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_HighlightsLinesIfHighlightLineRangesIsNotNullOrEmpty_Data()
        {

            const string escapedDummyCode = @"public string ExampleFunction(string arg)
{
    // Example comment
    return arg + &quot;dummyString&quot;;
}";

            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange>{ new LineRange(4, 4) }
                    ),
                    @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">public string ExampleFunction(string arg)</span></span>
<span class=""line""><span class=""line-number"">2</span><span class=""line-text"">{</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">    // Example comment</span></span>
<span class=""line highlight""><span class=""line-number"">4</span><span class=""line-text"">    return arg + &quot;dummyString&quot;;</span></span>
<span class=""line""><span class=""line-number"">5</span><span class=""line-text"">}</span></span>"
                },
                // LineHighlightRanges == null
                new object[]{
                    new SerializableWrapper<List<LineRange>>(
                        null
                    ),
                    escapedDummyCode
                },
                // LineHighlightRanges.Count == 0
                new object[]{
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange>()
                    ),
                    escapedDummyCode
                }
            };
        }

        [Theory]
        [MemberData(nameof(Write_PrefixesEmbellishmentClassesWithLineEmbellishmentClassesPrefixIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void Write_PrefixesEmbellishmentClassesWithLineEmbellishmentClassesPrefixIfItIsNotNullWhitespaceOrAnEmptyString(string dummyLineEmbellishmentClassesPrefix,
            string expectedCode)
        {
            // Arrange
            const string dummyCode = @"public string ExampleFunction(string arg)
{
    // Example comment
    return arg + ""dummyString"";
}";
            var lines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = lines };
            var dummyFlexiCodeOptions = new FlexiCodeBlockOptions()
            {
                LineEmbellishmentClassesPrefix = dummyLineEmbellishmentClassesPrefix,
                RenderLineNumbers = true,
                HighlightLineRanges = new List<LineRange> { new LineRange(4, 4)},
                CopyIconMarkup = null,
                Attributes = null,
            };
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_OPTIONS_KEY, dummyFlexiCodeOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeRenderer = CreateFlexiCodeRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
<header>
</header>
<pre><code>{expectedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Write_PrefixesEmbellishmentClassesWithLineEmbellishmentClassesPrefixIfItIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyPrefix = "dummyPrefix-";
            const string noPrefixExpectedCode = @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">public string ExampleFunction(string arg)</span></span>
<span class=""line""><span class=""line-number"">2</span><span class=""line-text"">{</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">    // Example comment</span></span>
<span class=""line highlight""><span class=""line-number"">4</span><span class=""line-text"">    return arg + &quot;dummyString&quot;;</span></span>
<span class=""line""><span class=""line-number"">5</span><span class=""line-text"">}</span></span>";

            return new object[][]
            {
                new object[]{
                    dummyPrefix,
                    @"<span class=""dummyPrefix-line""><span class=""dummyPrefix-line-number"">1</span><span class=""dummyPrefix-line-text"">public string ExampleFunction(string arg)</span></span>
<span class=""dummyPrefix-line""><span class=""dummyPrefix-line-number"">2</span><span class=""dummyPrefix-line-text"">{</span></span>
<span class=""dummyPrefix-line""><span class=""dummyPrefix-line-number"">3</span><span class=""dummyPrefix-line-text"">    // Example comment</span></span>
<span class=""dummyPrefix-line dummyPrefix-highlight""><span class=""dummyPrefix-line-number"">4</span><span class=""dummyPrefix-line-text"">    return arg + &quot;dummyString&quot;;</span></span>
<span class=""dummyPrefix-line""><span class=""dummyPrefix-line-number"">5</span><span class=""dummyPrefix-line-text"">}</span></span>"
                },
                // LineEmbellishmentClassesPrefix == null
                new object[]{
                    null,
                    noPrefixExpectedCode
                },
                // LineEmbellishmentClassesPrefix == string.Empty
                new object[]{
                    string.Empty,
                    noPrefixExpectedCode
                },
                // LineEmbellishmentClassesPrefix == whitespace
                new object[]{
                    " ",
                    noPrefixExpectedCode
                }
            };
        }

        public FlexiCodeBlockRenderer CreateFlexiCodeRenderer()
        {
            var services = new ServiceCollection();
            services.AddPrism();
            services.AddHighlightJS();
            _serviceProvider = services.BuildServiceProvider();
            IPrismService prismService = _serviceProvider.GetRequiredService<IPrismService>();
            IHighlightJSService highlightJSService = _serviceProvider.GetRequiredService<IHighlightJSService>();

            return new FlexiCodeBlockRenderer(prismService, highlightJSService);
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}
