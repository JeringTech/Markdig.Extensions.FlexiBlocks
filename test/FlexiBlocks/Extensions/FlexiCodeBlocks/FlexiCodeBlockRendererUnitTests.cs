using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Markdig.Parsers;
using Markdig.Renderers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfPrismServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCodeBlockRenderer(null, _mockRepository.Create<IHighlightJSService>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfHighlightJSServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCodeBlockRenderer(_mockRepository.Create<IPrismService>().Object, null));
        }

        [Theory]
        [MemberData(nameof(WriteBlock_OnlyRendersCodeIfEnableHtmlForBlockIsFalse_Data))]
        public void WriteBlock_OnlyRendersCodeIfEnableHtmlForBlockIsFalse(string dummyCode, string expectedResult)
        {
            // Arrange
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock(code: dummyCode);
            ExposedFlexiCodeBlockRenderer testSubject = CreateExposedFlexiCodeBlockRenderer();

            // Act
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter) { EnableHtmlForBlock = false };
            testSubject.Write(dummyHtmlRenderer, dummyFlexiCodeBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_OnlyRendersCodeIfEnableHtmlForBlockIsFalse_Data()
        {
            return new object[][]
            {
                // Special HTML entities are escaped
                new object[]{"<&", "&lt;&amp;\n"},
                // An end of line character is written after code
                new object[]{"dummyCode", "dummyCode\n"}
            };
        }

        [Fact]
        public void WriteBlock_RendersClassicCodeBlockIfRenderingModeIsClassic()
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock(renderingMode: FlexiCodeBlockRenderingMode.Classic);
            Mock<ExposedFlexiCodeBlockRenderer> mockTestSubject = CreateMockExposedFlexiCodeBlockRenderer();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.WriteClassic(dummyHtmlRenderer, dummyFlexiCodeBlock));

            // Act
            mockTestSubject.Object.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiCodeBlock);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void WriteBlock_RendersStandardCodeBlockIfRenderingModeIsStandard()
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock(renderingMode: FlexiCodeBlockRenderingMode.Standard);
            Mock<ExposedFlexiCodeBlockRenderer> mockTestSubject = CreateMockExposedFlexiCodeBlockRenderer();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.WriteStandard(dummyHtmlRenderer, dummyFlexiCodeBlock));

            // Act
            mockTestSubject.Object.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiCodeBlock);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Theory]
        [MemberData(nameof(WriteClassic_RendersClassicCodeBlock_Data))]
        public void WriteClassic_RendersClassicCodeBlock(int dummyCodeNumLines, string dummyCode, string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock(codeNumLines: dummyCodeNumLines, code: dummyCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            testSubject.WriteClassic(dummyHtmlRenderer, dummyFlexiCodeBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> WriteClassic_RendersClassicCodeBlock_Data()
        {
            const string dummyCode = "dummyCode";

            return new object[][]
            {
                // If code num lines is 0, no code is written
                new object[]{0, dummyCode, "<pre><code></code></pre>\n"},
                // If code num lines is larger than 0, code is written
                new object[]{1, dummyCode, $"<pre><code>{dummyCode}\n</code></pre>\n"},
                // Special HTML entities are escaped
                new object[]{1, "<&", "<pre><code>&lt;&amp;\n</code></pre>\n"},
                // \n is always prepended to code, even if it ends with an end of line character (consistent with CommonMark)
                new object[]{1, dummyCode + "\n", $"<pre><code>{dummyCode}\n\n</code></pre>\n"}
            };
        }

        // Interactions between syntax highlighting and code embellishing are tested in FlexiCodeBlocksSpecs
        [Theory]
        [MemberData(nameof(WriteStandard_RendersStandardCodeBlock_Data))]
        public void WriteStandard_RendersStandardCodeBlock(FlexiCodeBlock dummyFlexiCodeBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            testSubject.WriteStandard(dummyHtmlRenderer, dummyFlexiCodeBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteStandard_RendersStandardCodeBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummyTitle = "dummyTitle";
            const string dummyCopyIcon = "<dummyCopyIcon></dummyCopyIcon>";
            const string dummyCopyIconWithClass = "<dummyCopyIcon class=\"__copy-icon\"></dummyCopyIcon>";
            const string dummyLanguage = "dummyLanguage";
            const string dummyCodeWithSpecialChars = "<&>\"";
            const string dummyOmittedLinesIcon = "<dummyOmittedLinesIcon></dummyOmittedLinesIcon>";
            const string dummyOmittedLinesIconWithClass = "<dummyOmittedLinesIcon class=\"__omitted-lines-icon\"></dummyOmittedLinesIcon>";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiCodeBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_no_title {dummyBlockName}_no_copy-icon {dummyBlockName}_no_syntax-highlights {dummyBlockName}_no_line-numbers {dummyBlockName}_no_omitted-lines-icon {dummyBlockName}_no_highlighted-lines {dummyBlockName}_no_highlighted-phrases"">
<header class=""{dummyBlockName}__header"">
<span class=""{dummyBlockName}__title""></span>
<button class=""{dummyBlockName}__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""{dummyBlockName}__pre""><code class=""{dummyBlockName}__code""></code></pre>
</div>
"
                },
                // If Title is specified, it is rendered and a _has_title class is rendered
                new object[]{
                    CreateFlexiCodeBlock(title: dummyTitle),
                    $@"<div class="" _has_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title"">{dummyTitle}</span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // If Title is null, whitespace or an empty string, no title is rendered and a _no_title class is rendered (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(title: " "),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                new object[]{
                    CreateFlexiCodeBlock(title: string.Empty),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // If CopyIcon is valid HTML, it is rendered with a default class and a _has_copy-icon class is rendered
                new object[]{
                    CreateFlexiCodeBlock(copyIcon: dummyCopyIcon),
                    $@"<div class="" _no_title _has_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
{dummyCopyIconWithClass}
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // If CopyIcon is null, whitespace or an empty string, no copy icon is rendered and a _no_copy-icon class is rendered (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(copyIcon: " "),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                new object[]{
                    CreateFlexiCodeBlock(copyIcon: string.Empty),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // If Language is specified, a language modifier class is rendered
                new object[]{
                    CreateFlexiCodeBlock(language: dummyLanguage),
                    $@"<div class="" _no_title _no_copy-icon _language-{dummyLanguage} _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // Code is rendered with <, >, & and "escaped if there are no line embellishements or syntax highlights
                new object[]{
                    CreateFlexiCodeBlock(code: dummyCodeWithSpecialChars, codeNumLines: 1),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">&lt;&amp;&gt;&quot;
</code></pre>
</div>
"
                },
                // Code is rendered with <, >, & and " escaped if there are line embellishments and no syntax highlights
                new object[]{
                    CreateFlexiCodeBlock(code: dummyCodeWithSpecialChars, codeNumLines: 1, highlightedLines: new ReadOnlyCollection<LineRange>(new List<LineRange>(){ new LineRange() })),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _has_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line __line_highlighted"">&lt;&amp;&gt;&quot;</span>
</code></pre>
</div>
"
                },
                // If Code is null or an empty string, no code is rendered (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(code: string.Empty),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // Line numbers are rendered if LineNumbers is specified and not empty - Omitted lines notices are rendered if line with no line numbers is empty
                new object[]{
                    CreateFlexiCodeBlock(code: @"
line

line
",
                        codeNumLines: 5, lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(2, 2, 21), new NumberedLineRange(4, 4, 1234) })),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 1 to 20 omitted for brevity</span>
<span class=""__line-prefix"">21</span><span class=""__line"">line</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 22 to 1233 omitted for brevity</span>
<span class=""__line-prefix"">1234</span><span class=""__line"">line</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 1235 to the end omitted for brevity</span>
</code></pre>
</div>
"
                },
                // Line numbers are rendered if LineNumbers is specified and not empty - Omitted lines notice is not rendered if line with no line number is not empty
                new object[]{
                    CreateFlexiCodeBlock(code: @"line
...
line",
                        codeNumLines: 3, lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(endLine:1), new NumberedLineRange(3, 3, 12) })),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix"">1</span><span class=""__line"">line</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">...</span>
<span class=""__line-prefix"">12</span><span class=""__line"">line</span>
</code></pre>
</div>
"
                },
                // Line numbers are rendered if LineNumbers is specified and not empty - Range starts and ends can be negative
                new object[]{
                    CreateFlexiCodeBlock(code: @"line
line
line
line
line
line",
                        codeNumLines: 6, lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(1, -5, 1),
                        new NumberedLineRange(-4, 4, 3),
                        new NumberedLineRange(-2, -1, 5)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix"">1</span><span class=""__line"">line</span>
<span class=""__line-prefix"">2</span><span class=""__line"">line</span>
<span class=""__line-prefix"">3</span><span class=""__line"">line</span>
<span class=""__line-prefix"">4</span><span class=""__line"">line</span>
<span class=""__line-prefix"">5</span><span class=""__line"">line</span>
<span class=""__line-prefix"">6</span><span class=""__line"">line</span>
</code></pre>
</div>
"
                },
                // Line numbers are rendered if LineNumbers is specified and not empty - If there are sequential empty lines with no line number, the omitted lines message is repeated
                new object[]{
                    CreateFlexiCodeBlock(code: @"line

",
                        codeNumLines: 3, lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(endLine: 1)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix"">1</span><span class=""__line"">line</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 2 to the end omitted for brevity</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 2 to the end omitted for brevity</span>
</code></pre>
</div>
"
                },
                // No Line numbers are rendered if LineNumbers is null or empty (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(code: @"line

line",
                        codeNumLines: 3, lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>())),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">line

line
</code></pre>
</div>
"
                },
                // If omitted lines icon is valid HTML, it is rendered with a default class and a _has_omitted-lines-icon class is rendered
                new object[]{
                    CreateFlexiCodeBlock(code: @"line

line",
                        codeNumLines: 3, lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(endLine: 1), new NumberedLineRange(3, startNumber: 5)}),
                        omittedLinesIcon: dummyOmittedLinesIcon),
                    $@"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _has_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix"">1</span><span class=""__line"">line</span>
<span class=""__line-prefix"">{dummyOmittedLinesIconWithClass}</span><span class=""__line __line_omitted-lines"">Lines 2 to 4 omitted for brevity</span>
<span class=""__line-prefix"">5</span><span class=""__line"">line</span>
</code></pre>
</div>
"
                },
                // If omitted lines icon is null, whitespace or an empty string, no omitted lines icon is rendered and a _no_omitted-lines-icon class is rendered (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(code: @"line

line",
                        codeNumLines: 3,
                        lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(endLine: 1), new NumberedLineRange(3, startNumber: 5)}),
                        omittedLinesIcon: " "),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix"">1</span><span class=""__line"">line</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 2 to 4 omitted for brevity</span>
<span class=""__line-prefix"">5</span><span class=""__line"">line</span>
</code></pre>
</div>
"
                },
                new object[]{
                    CreateFlexiCodeBlock(code: @"line

line",
                        codeNumLines: 3,
                        lineNumbers: new ReadOnlyCollection<NumberedLineRange>(new List<NumberedLineRange>(){new NumberedLineRange(endLine: 1), new NumberedLineRange(3, startNumber: 5)}),
                        omittedLinesIcon: string.Empty),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _has_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line-prefix"">1</span><span class=""__line"">line</span>
<span class=""__line-prefix""></span><span class=""__line __line_omitted-lines"">Lines 2 to 4 omitted for brevity</span>
<span class=""__line-prefix"">5</span><span class=""__line"">line</span>
</code></pre>
</div>
"
                },
                // Lines are highlighted if HighlightedLines is specified and not empty - Range starts and ends can be negative
                new object[]{
                    CreateFlexiCodeBlock(code: @"line
line
line
line
line
line
line
line
line
line",
                        codeNumLines: 10,
                        highlightedLines: new ReadOnlyCollection<LineRange>(new List<LineRange>(){ new LineRange(endLine: 2), new LineRange(4, -6), new LineRange(-4, 7), new LineRange(-2, -1)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _has_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__line __line_highlighted"">line</span>
<span class=""__line __line_highlighted"">line</span>
line
<span class=""__line __line_highlighted"">line</span>
<span class=""__line __line_highlighted"">line</span>
line
<span class=""__line __line_highlighted"">line</span>
line
<span class=""__line __line_highlighted"">line</span>
<span class=""__line __line_highlighted"">line</span>
</code></pre>
</div>
"
                },
                // No lines are highlighted if HighlightedLines is null or empty (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(code: @"line

line",
                        codeNumLines: 3,
                        highlightedLines: new ReadOnlyCollection<LineRange>(new List<LineRange>())),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">line

line
</code></pre>
</div>
"
                },
                // Phrases are highlighted if HighlightedPhrases is specified and not empty - Phrase at start of code
                new object[]{
                    CreateFlexiCodeBlock(code: "start end", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>(){ new Phrase(0, 4)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _has_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__highlighted-phrase"">start</span> end
</code></pre>
</div>
"
                },
                // Phrases are highlighted if HighlightedPhrases is specified and not empty - Phrase at end of code
                new object[]{
                    CreateFlexiCodeBlock(code: "start end", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>(){ new Phrase(6, 8)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _has_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">start <span class=""__highlighted-phrase"">end</span>
</code></pre>
</div>
"
                },
                // Phrases are highlighted if HighlightedPhrases is specified and not empty - Intersecting phrases are combined
                new object[]{
                    CreateFlexiCodeBlock(code: "12345", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>(){ new Phrase(0, 2), new Phrase(2, 4), new Phrase(3, 4),})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _has_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__highlighted-phrase"">12345</span>
</code></pre>
</div>
"
                },
                // Phrases are highlighted if HighlightedPhrases is specified and not empty - Adjacent phrases are combined
                new object[]{
                    CreateFlexiCodeBlock(code: "abcde", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>(){ new Phrase(0, 2), new Phrase(3, 4)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _has_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__highlighted-phrase"">abcde</span>
</code></pre>
</div>
"
                },
                // Phrases are highlighted if HighlightedPhrases is specified and not empty - Unaffected by HTML escaping
                new object[]{
                    CreateFlexiCodeBlock(code: "< &", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>(){ new Phrase(0, 0), new Phrase(2, 2)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _has_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__highlighted-phrase"">&lt;</span> <span class=""__highlighted-phrase"">&amp;</span>
</code></pre>
</div>
"
                },
                // Phrases are highlighted if HighlightedPhrases is specified and not empty - Can be multiline
                new object[]{
                    CreateFlexiCodeBlock(code: "This\nis a multiline\nphrase", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>(){ new Phrase(0, 25)})),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _has_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""><span class=""__highlighted-phrase"">This
is a multiline
phrase</span>
</code></pre>
</div>
"
                },
                // No phrases are highlighted if HighlightedPhrases is null or empty (null case verified by other tests)
                new object[]{
                    CreateFlexiCodeBlock(code: "12345", codeNumLines: 1, highlightedPhrases: new ReadOnlyCollection<Phrase>(new List<Phrase>())),
                    @"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">12345
</code></pre>
</div>
"
                },
                // If attributes specified, they're written
                new object[]{
                    CreateFlexiCodeBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiCodeBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _no_title _no_copy-icon _no_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases {dummyClass}"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code""></code></pre>
</div>
"
                }
            };
        }

        [Fact]
        public void WriteStandard_SyntaxHighlightsCodeUsingPrismIfSyntaxHighlighterIsPrism()
        {
            // Arrange
            const string dummyCode = "dummyCode";
            const string dummyHighlightedCode = "dummyHighlightedCode";
            const string dummyLanguage = "dummyLanguage";
            Mock<IPrismService> mockPrismService = _mockRepository.Create<IPrismService>();
            mockPrismService.Setup(p => p.HighlightAsync(dummyCode, dummyLanguage, default)).ReturnsAsync(dummyHighlightedCode);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock(language: dummyLanguage, code: dummyCode, syntaxHighlighter: SyntaxHighlighter.Prism);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(prismService: mockPrismService.Object);

            // Act
            testSubject.WriteStandard(dummyHtmlRenderer, dummyFlexiCodeBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal($@"<div class="" _no_title _no_copy-icon _language-dummyLanguage _has_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">{dummyHighlightedCode}
</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteStandard_SyntaxHighlightsCodeUsingHighlightJSIfSyntaxHighlighterIsHighlightJS()
        {
            // Arrange
            const string dummyCode = "dummyCode";
            const string dummyHighlightedCode = "dummyHighlightedCode";
            const string dummyLanguage = "dummyLanguage";
            Mock<IHighlightJSService> mockHighlightJSService = _mockRepository.Create<IHighlightJSService>();
            mockHighlightJSService.Setup(p => p.HighlightAsync(dummyCode, dummyLanguage, "hljs-", default)).ReturnsAsync(dummyHighlightedCode);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            FlexiCodeBlock dummyFlexiCodeBlock = CreateFlexiCodeBlock(language: dummyLanguage, code: dummyCode, syntaxHighlighter: SyntaxHighlighter.HighlightJS);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(highlightJSService: mockHighlightJSService.Object);

            // Act
            testSubject.WriteStandard(dummyHtmlRenderer, dummyFlexiCodeBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal($@"<div class="" _no_title _no_copy-icon _language-dummyLanguage _has_syntax-highlights _no_line-numbers _no_omitted-lines-icon _no_highlighted-lines _no_highlighted-phrases"">
<header class=""__header"">
<span class=""__title""></span>
<button class=""__copy-button"" title=""Copy code"" aria-label=""Copy code"">
</button>
</header>
<pre class=""__pre""><code class=""__code"">{dummyHighlightedCode}
</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public class ExposedFlexiCodeBlockRenderer : FlexiCodeBlockRenderer
        {
            public ExposedFlexiCodeBlockRenderer(IPrismService prismService, IHighlightJSService highlightJSService) : base(prismService, highlightJSService)
            {
            }

            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiCodeBlock flexiCodeBlock)
            {
                base.WriteBlock(htmlRenderer, flexiCodeBlock);
            }
        }

        public FlexiCodeBlockRenderer CreateFlexiCodeBlockRenderer(IPrismService prismService = null,
            IHighlightJSService highlightJSService = null)
        {
            return new FlexiCodeBlockRenderer(prismService ?? _mockRepository.Create<IPrismService>().Object,
                highlightJSService ?? _mockRepository.Create<IHighlightJSService>().Object);
        }

        public ExposedFlexiCodeBlockRenderer CreateExposedFlexiCodeBlockRenderer(IPrismService prismService = null,
            IHighlightJSService highlightJSService = null)
        {
            return new ExposedFlexiCodeBlockRenderer(prismService ?? _mockRepository.Create<IPrismService>().Object,
                highlightJSService ?? _mockRepository.Create<IHighlightJSService>().Object);
        }

        public Mock<ExposedFlexiCodeBlockRenderer> CreateMockExposedFlexiCodeBlockRenderer(IPrismService prismService = null,
            IHighlightJSService highlightJSService = null)
        {
            return _mockRepository.Create<ExposedFlexiCodeBlockRenderer>(prismService ?? _mockRepository.Create<IPrismService>().Object,
                highlightJSService ?? _mockRepository.Create<IHighlightJSService>().Object);
        }

        private static FlexiCodeBlock CreateFlexiCodeBlock(string blockName = default,
            string title = default,
            string copyIcon = default,
            string language = default,
            string code = default,
            int codeNumLines = default,
            SyntaxHighlighter syntaxHighlighter = default,
            ReadOnlyCollection<NumberedLineRange> lineNumbers = default,
            string omittedLinesIcon = default,
            ReadOnlyCollection<LineRange> highlightedLines = default,
            ReadOnlyCollection<Phrase> highlightedPhrases = default,
            FlexiCodeBlockRenderingMode renderingMode = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiCodeBlock(blockName, title, copyIcon, language, code, codeNumLines, syntaxHighlighter, lineNumbers, omittedLinesIcon,
                highlightedLines, highlightedPhrases, renderingMode, attributes, blockParser);
        }
    }
}
