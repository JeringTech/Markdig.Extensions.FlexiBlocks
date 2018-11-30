using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersOutermostElementsClassIfClassIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void WriteFlexiBlock_RendersOutermostElementsClassIfClassIsNotNullWhitespaceOrAnEmptyString(string @class, string expectedResult)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: @class, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersOutermostElementsClassIfClassIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyClass = "dummyClass";
            const string noClassExpectedResult = @"<div>
<header>
<button>
</button>
</header>
<pre><code></code></pre>
</div>
";

            return new object[][]
            {
                new object[]{
                    dummyClass,
                    $@"<div class=""{dummyClass}"">
<header>
<button>
</button>
</header>
<pre><code></code></pre>
</div>
"
                },
                // Null
                new object[]{
                    null, noClassExpectedResult
                },
                // Whitespace
                new object[]{
                    " ", noClassExpectedResult
                },
                // Empty
                new object[]{
                    string.Empty, noClassExpectedResult
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersAttributesIfAnyAreSpecified_Data))]
        public void WriteFlexiBlock_RendersAttributesIfAnyAreSpecified(SerializableWrapper<Dictionary<string, string>> dummyAttributesWrapper,
            string expectedDivStartTag)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(attributes: dummyAttributesWrapper.Value, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"{expectedDivStartTag}
<header>
<button>
</button>
</header>
<pre><code></code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersAttributesIfAnyAreSpecified_Data()
        {
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";
            const string dummyClass = "dummyClass";

            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        new Dictionary<string, string>() {{ dummyAttribute, dummyAttributeValue }}
                    ),
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\" class=\"flexi-code-block\">"
                },
                // Class specified
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        new Dictionary<string, string>() {{ "class", dummyClass}}
                    ),
                    $"<div class=\"{dummyClass} flexi-code-block\">"
                },
                // Empty
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        new Dictionary<string, string>()
                    ),
                    "<div class=\"flexi-code-block\">"
                },
                // Null
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        null
                    ),
                    "<div class=\"flexi-code-block\">"
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void WriteFlexiBlock_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString(string dummyTitle, string expectedHeaderElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, title: dummyTitle, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
{expectedHeaderElement}
<pre><code></code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyTitle = "dummyTitle";

            return new object[][]
            {
                new object[]{
                    dummyTitle,
                    $"<header>\n<span>{dummyTitle}</span>\n<button>\n</button>\n</header>"
                },
                // Empty
                new object[]{
                    string.Empty,
                    "<header>\n<button>\n</button>\n</header>"
                },
                // Null
                new object[]{
                    null,
                    "<header>\n<button>\n</button>\n</header>"
                },
                // Whitespace
                new object[]{
                    " ",
                    "<header>\n<button>\n</button>\n</header>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void WriteFlexiBlock_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString(string dummyCopyIconMarkup, string expectedHeaderElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, copyIconMarkup: dummyCopyIconMarkup);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
{expectedHeaderElement}
<pre><code></code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString_Data()
        {
            const string dummyCopyIconMarkup = "dummyCopyIconMarkup";

            return new object[][]
            {
                new object[]{
                    dummyCopyIconMarkup,
                    $"<header>\n<button>\n{dummyCopyIconMarkup}\n</button>\n</header>"
                },
                // Empty
                new object[]{
                    string.Empty,
                    "<header>\n<button>\n</button>\n</header>"
                },
                // Null
                new object[]{
                    null,
                    "<header>\n<button>\n</button>\n</header>"
                },
                // Whitespace
                new object[]{
                    " ",
                    "<header>\n<button>\n</button>\n</header>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersCodeLanguageClassIfItIsNotNull_Data))]
        public void WriteFlexiBlock_RendersCodeLanguageClassIfItIsNotNull(string dummyLanguage,
            string dummyCodeLanguageClassFormat,
            string expectedCodeElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, language: dummyLanguage, codeClassFormat: dummyCodeLanguageClassFormat,
                syntaxHighlighter: SyntaxHighlighter.None, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal($@"<div>
<header>
<button>
</button>
</header>
<pre>{expectedCodeElement}</pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_RendersCodeLanguageClassIfItIsNotNull_Data()
        {
            const string dummyLanguage = "dummyLanguage";
            const string dummyCodeLanguageClassLanguageFormat = "dummyCodeLanguageClassFormat-{0}";

            return new object[][]
            {
                new object[]{
                    dummyLanguage,
                    dummyCodeLanguageClassLanguageFormat,
                    $"<code class=\"{string.Format(dummyCodeLanguageClassLanguageFormat, dummyLanguage)}\"></code>"
                },
                // CodeLanguageClass == null when Language == null || CodeLanguageClassFormat == null
                new object[]{
                    null,
                    null,
                    "<code></code>"
                }
            };
        }

        [Fact]
        public void WriteFlexiBlock_HighlightsSyntaxUsingHighlightJSIfLanguageIsNotNullWhitespaceOrAnEmptyStringAndSyntaxHighlighterIsHighlightJS()
        {
            // Arrange
            const string dummyCode = "dummyCode\"&<>"; // Should not be escaped by markdig since syntax highlighters do escaping
            const string dummyLanguage = "dummyLanguage";
            const string dummyHighlightJSClassPrefix = "dummyHighlightJSClassPrefix";
            const string dummyHighlightedCode = "dummyHighlightedCode";
            const string dummyEmbellishedCode = "dummyEmbellishedCode";
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, syntaxHighlighter: SyntaxHighlighter.HighlightJS,
                language: dummyLanguage, highlightJSClassPrefix: dummyHighlightJSClassPrefix, copyIconMarkup: null, codeClassFormat: null, hiddenLinesIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<IHighlightJSService> mockHighlightJSService = _mockRepository.Create<IHighlightJSService>();
            mockHighlightJSService.Setup(h => h.HighlightAsync(dummyCode, dummyLanguage, dummyHighlightJSClassPrefix, default)).ReturnsAsync(dummyHighlightedCode);
            Mock<ILineEmbellisherService> mockLineEmbellisherService = _mockRepository.Create<ILineEmbellisherService>();
            mockLineEmbellisherService.Setup(l => l.EmbellishLines(dummyHighlightedCode, null, null, null, null)).Returns(dummyEmbellishedCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(highlightJSService: mockHighlightJSService.Object, lineEmbellisherService: mockLineEmbellisherService.Object);

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal($@"<div>
<header>
<button>
</button>
</header>
<pre><code>{dummyEmbellishedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteFlexiBlock_HighlightsSyntaxUsingPrismIfLanguageIsNotNullWhitespaceOrAnEmptyStringAndSyntaxHighlighterIsPrism()
        {
            // Arrange
            const string dummyCode = "dummyCode\"&<>";
            const string dummyLanguage = "dummyLanguage";
            const string dummyHighlightedCode = "dummyHighlightedCode";
            const string dummyEmbellishedCode = "dummyEmbellishedCode";
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, syntaxHighlighter: SyntaxHighlighter.Prism,
                language: dummyLanguage, copyIconMarkup: null, codeClassFormat: null, hiddenLinesIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<IPrismService> mockPrismService = _mockRepository.Create<IPrismService>();
            mockPrismService.Setup(h => h.HighlightAsync(dummyCode, dummyLanguage, default)).ReturnsAsync(dummyHighlightedCode);
            Mock<ILineEmbellisherService> mockLineEmbellisherService = _mockRepository.Create<ILineEmbellisherService>();
            mockLineEmbellisherService.Setup(l => l.EmbellishLines(dummyHighlightedCode, null, null, null, null)).Returns(dummyEmbellishedCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(prismService: mockPrismService.Object, lineEmbellisherService: mockLineEmbellisherService.Object);

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal($@"<div>
<header>
<button>
</button>
</header>
<pre><code>{dummyEmbellishedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_DoesNotHighlightSyntaxifLanguageIsNullWhitespaceOrAnEmptyStringOrSyntaxHighlighterIsNone_Data))]
        public void WriteFlexiBlock_DoesNotHighlightSyntaxifLanguageIsNullWhitespaceOrAnEmptyStringOrSyntaxHighlighterIsNone(string dummyLanguage,
            SyntaxHighlighter dummySyntaxHighlighter)
        {
            // Arrange
            const string dummyCode = "dummyCode";
            const string dummyEmbellishedCode = "dummyEmbellishedCode";
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, syntaxHighlighter: dummySyntaxHighlighter,
                language: dummyLanguage, copyIconMarkup: null, codeClassFormat: null, hiddenLinesIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<ILineEmbellisherService> mockLineEmbellisherService = _mockRepository.Create<ILineEmbellisherService>();
            mockLineEmbellisherService.Setup(l => l.EmbellishLines(dummyCode, null, null, null, null)).Returns(dummyEmbellishedCode);
            Mock<IHighlightJSService> mockHighlightJSService = _mockRepository.Create<IHighlightJSService>();
            Mock<IPrismService> mockPrismService = _mockRepository.Create<IPrismService>();
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(mockPrismService.Object, mockHighlightJSService.Object, mockLineEmbellisherService.Object);

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            _mockRepository.VerifyAll();
            mockHighlightJSService.Verify(h => h.HighlightAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
            mockPrismService.Verify(h => h.HighlightAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
            Assert.Equal($@"<div>
<header>
<button>
</button>
</header>
<pre><code>{dummyEmbellishedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_DoesNotHighlightSyntaxifLanguageIsNullWhitespaceOrAnEmptyStringOrSyntaxHighlighterIsNone_Data()
        {
            return new object[][]
            {
                new object[]{null, SyntaxHighlighter.HighlightJS},
                new object[]{" ", SyntaxHighlighter.Prism},
                new object[]{string.Empty, SyntaxHighlighter.HighlightJS},
                new object[]{"dummyLanguage", SyntaxHighlighter.None},
            };
        }

        [Fact]
        public void WriteFlexiBlock_EmbellishesLines()
        {
            // Arrange
            const string dummyLineEmbellishmentClassesPrefix = "dummyLineEmbellishmentClassesPrefix";
            const string dummyHiddenLinesIconMarkup = "dummyHiddenLinesIconMarkup";
            const string dummyEmbellishedCode = "dummyEmbellishedCode";
            var dummyHighlightLineRanges = new List<LineRange>();
            var dummyLineNumberLineRanges = new List<NumberedLineRange>();
            var dummyLines = new StringLineGroup("dummyCode\"&<>");
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null,
                lineNumberLineRanges: dummyLineNumberLineRanges,
                highlightLineRanges: dummyHighlightLineRanges,
                lineEmbellishmentClassesPrefix: dummyLineEmbellishmentClassesPrefix,
                hiddenLinesIconMarkup: dummyHiddenLinesIconMarkup,
                copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<ILineEmbellisherService> mockLineEmbellisherService = _mockRepository.Create<ILineEmbellisherService>();
            mockLineEmbellisherService.Setup(l => l.EmbellishLines("dummyCode&quot;&amp;&lt;&gt;", // Code gets escaped before embellishing
                    dummyLineNumberLineRanges,
                    dummyHighlightLineRanges,
                    dummyLineEmbellishmentClassesPrefix,
                    dummyHiddenLinesIconMarkup)).
                Returns(dummyEmbellishedCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(lineEmbellisherService: mockLineEmbellisherService.Object);

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal($@"<div>
<header>
<button>
</button>
</header>
<pre><code>{dummyEmbellishedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteFlexiBlock_EscapesCodeWrittenDirectlyToTheRenderer()
        {
            // Arrange
            var dummyLines = new StringLineGroup("dummyCode\"&<>");
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(@class: null, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                testSubject.Write(dummyHtmlRenderer, dummyCodeBlock);
                result = dummyStringWriter.ToString();
            }

            // Assert
            Assert.Equal(@"<div>
<header>
<button>
</button>
</header>
<pre><code>dummyCode&quot;&amp;&lt;&gt;</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public FlexiCodeBlockRenderer CreateFlexiCodeBlockRenderer(IPrismService prismService = null,
            IHighlightJSService highlightJSService = null,
            ILineEmbellisherService lineEmbellisherService = null)
        {
            return new FlexiCodeBlockRenderer(prismService ?? _mockRepository.Create<IPrismService>().Object,
                highlightJSService ?? _mockRepository.Create<IHighlightJSService>().Object,
                lineEmbellisherService ?? _mockRepository.Create<ILineEmbellisherService>().Object);
        }
    }
}
