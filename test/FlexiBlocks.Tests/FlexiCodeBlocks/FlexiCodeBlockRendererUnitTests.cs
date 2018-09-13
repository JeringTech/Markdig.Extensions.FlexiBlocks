using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Jering.Web.SyntaxHighlighters.HighlightJS;
using Jering.Web.SyntaxHighlighters.Prism;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FlexiCodeBlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersAttributesIfAnyAreSpecified_Data))]
        public void WriteFlexiBlock_RendersAttributesIfAnyAreSpecified(SerializableWrapper<Dictionary<string, string>> dummyAttributesWrapper,
            string expectedDivStartTag)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(attributes: dummyAttributesWrapper.Value, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeBlockRenderer = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeBlockRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
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

        public static IEnumerable<object[]> WriteFlexiBlock_RendersAttributesIfAnyAreSpecified_Data()
        {
            const string dummyAttribute = "dummyAttribute";
            const string dummyAttributeValue = "dummyAttributeValue";

            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        new Dictionary<string, string>() {{ dummyAttribute, dummyAttributeValue }}
                    ),
                    $"<div {dummyAttribute}=\"{dummyAttributeValue}\">"
                },
                // Empty
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        new Dictionary<string, string>()
                    ),
                    "<div>"
                },
                // Null
                new object[]{
                    new SerializableWrapper<Dictionary<string, string>>(
                        null
                    ),
                    "<div>"
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void WriteFlexiBlock_RendersTitleIfItIsNotNullWhitespaceOrAnEmptyString(string dummyTitle, string expectedHeaderElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(title: dummyTitle, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeBlockRenderer = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeBlockRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
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
        [MemberData(nameof(WriteFlexiBlock_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString_Data))]
        public void WriteFlexiBlock_RendersCopyIconMarkupIfItIsNotNullWhitespaceOrAnEmptyString(string dummyCopyIconMarkup, string expectedHeaderElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(copyIconMarkup: dummyCopyIconMarkup);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeBlockRenderer = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeBlockRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
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
        [MemberData(nameof(WriteFlexiBlock_RendersCodeLanguageClassIfItIsNotNull_Data))]
        public void WriteFlexiBlock_RendersCodeLanguageClassIfItIsNotNull(string dummyLanguage,
            string dummyCodeLanguageClassFormat,
            string expectedCodeElement)
        {
            // Arrange
            var dummyCodeBlock = new CodeBlock(null);
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(language: dummyLanguage, codeClassFormat: dummyCodeLanguageClassFormat, 
                syntaxHighlighter: SyntaxHighlighter.None, copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            FlexiCodeBlockRenderer dummyFlexiCodeBlockRenderer = CreateFlexiCodeBlockRenderer();

            // Act
            string result = null;
            using (var dummyStringWriter = new StringWriter())
            {
                var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
                dummyFlexiCodeBlockRenderer.Write(dummyHtmlRenderer, dummyCodeBlock);
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

        // TODO ensure that code is not escaped when using syntax highlighters
        [Fact]
        public void WriteFlexiBlock_HighlightsSyntaxUsingHighlightJSIfLanguageIsNotNullWhitespaceOrAnEmptyStringAndSyntaxHighlighterIsHighlightJS()
        {
            // Arrange
            const string dummyCode = "dummyCode\"&<>"; // Should not be escaped by markdig since syntax highlighters do escaping
            const string dummyLanguage = "dummyLanguage";
            const string dummyHighlightJSClassPrefix = "dummyHighlightJSClassPrefix";
            const string dummyHighlightedCode = "dummyHighlightedCode";
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(syntaxHighlighter: SyntaxHighlighter.HighlightJS,
                language: dummyLanguage, highlightJSClassPrefix: dummyHighlightJSClassPrefix, copyIconMarkup: null, codeClassFormat: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<IHighlightJSService> mockHighlightJSService = _mockRepository.Create<IHighlightJSService>();
            mockHighlightJSService.Setup(h => h.HighlightAsync(dummyCode, dummyLanguage, dummyHighlightJSClassPrefix)).ReturnsAsync(dummyHighlightedCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(highlightJSService: mockHighlightJSService.Object);

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
</header>
<pre><code>{dummyHighlightedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteFlexiBlock_HighlightsSyntaxUsingPrismIfLanguageIsNotNullWhitespaceOrAnEmptyStringAndSyntaxHighlighterIsHighlightJS()
        {
            // Arrange
            const string dummyCode = "dummyCode\"&<>";
            const string dummyLanguage = "dummyLanguage";
            const string dummyHighlightedCode = "dummyHighlightedCode";
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(syntaxHighlighter: SyntaxHighlighter.Prism,
                language: dummyLanguage, copyIconMarkup: null, codeClassFormat: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<IPrismService> mockPrismService = _mockRepository.Create<IPrismService>();
            mockPrismService.Setup(h => h.HighlightAsync(dummyCode, dummyLanguage)).ReturnsAsync(dummyHighlightedCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(prismService: mockPrismService.Object);

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
</header>
<pre><code>{dummyHighlightedCode}</code></pre>
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
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(syntaxHighlighter: dummySyntaxHighlighter,
                language: dummyLanguage, copyIconMarkup: null, codeClassFormat: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            // Don't provide any syntax highlighting services to FlexiCodeBlockRenderer. As long as Renderer.Write does not throw, the test has passed.
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
</header>
<pre><code>{dummyCode}</code></pre>
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

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_EmbellishesLinesIfLineNumberRangesOrHighlightLineRangesIsNotNullOrEmpty_Data))]
        public void WriteFlexiBlock_EmbellishesLinesIfLineNumberRangesOrHighlightLineRangesIsNotNullOrEmpty(SerializableWrapper<List<LineNumberRange>> dummyLineNumberRangesWrapper,
            SerializableWrapper<List<LineRange>> dummyHighlightLineRangesWrapper)
        {
            // Arrange
            const string dummyLineEmbellishmentClassesPrefix = "dummyLineEmbellishmentClassesPrefix";
            const string dummyEmbellishedCode = "dummyEmbellishedCode";
            var dummyLines = new StringLineGroup("dummyCode\"&<>");
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(lineNumberRanges: dummyLineNumberRangesWrapper.Value,
                highlightLineRanges: dummyHighlightLineRangesWrapper.Value,
                lineEmbellishmentClassesPrefix: dummyLineEmbellishmentClassesPrefix,
                copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            Mock<ILineEmbellishmentsService> mockLineEmbellishmentsService = _mockRepository.Create<ILineEmbellishmentsService>();
            mockLineEmbellishmentsService.Setup(l => l.EmbellishLines("dummyCode&quot;&amp;&lt;&gt;", // Code gets escaped before embellishing
                    dummyLineNumberRangesWrapper.Value,
                    dummyHighlightLineRangesWrapper.Value,
                    dummyLineEmbellishmentClassesPrefix)).
                Returns(dummyEmbellishedCode);
            FlexiCodeBlockRenderer testSubject = CreateFlexiCodeBlockRenderer(lineEmbellishmentsService: mockLineEmbellishmentsService.Object);

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
</header>
<pre><code>{dummyEmbellishedCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_EmbellishesLinesIfLineNumberRangesOrHighlightLineRangesIsNotNullOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(
                        null
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange>{ new LineRange() }
                    )
                },
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(
                        new List<LineNumberRange>{new LineNumberRange()}
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        null
                    )
                },
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(
                        new List<LineNumberRange>{new LineNumberRange()}
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange>{ new LineRange() }
                    )
                }
            };
        }

        [Theory]
        [MemberData(nameof(WriteFlexiBlock_DoesNotEmbellishLinesIfLineNumberRangesAndHighlightLineRangesAreNullOrEmpty_Data))]
        public void WriteFlexiBlock_DoesNotEmbellishLinesIfLineNumberRangesAndHighlightLineRangesAreNullOrEmpty(SerializableWrapper<List<LineNumberRange>> dummyLineNumberRangesWrapper,
            SerializableWrapper<List<LineRange>> dummyHighlightLineRangesWrapper)
        {
            // Arrange
            const string dummyCode = "dummyCode";
            var dummyLines = new StringLineGroup(dummyCode);
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(lineNumberRanges: dummyLineNumberRangesWrapper.Value,
                highlightLineRanges: dummyHighlightLineRangesWrapper.Value,
                copyIconMarkup: null);
            dummyCodeBlock.SetData(FlexiCodeBlocksExtension.FLEXI_CODE_BLOCK_OPTIONS_KEY, dummyFlexiCodeBlockOptions);
            // Don't provide LineEmbellishmentsService. As long as Renderer.Write does not throw, the test has passed.
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
</header>
<pre><code>{dummyCode}</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteFlexiBlock_DoesNotEmbellishLinesIfLineNumberRangesAndHighlightLineRangesAreNullOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(
                        null
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        null
                    )
                },
                new object[]{
                    new SerializableWrapper<List<LineNumberRange>>(
                        new List<LineNumberRange>()
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange>()
                    )
                }
            };
        }

        [Fact]
        public void WriteFlexiBlock_EscapesCodeWrittenDirectlyToTheRenderer()
        {
            // Arrange
            var dummyLines = new StringLineGroup("dummyCode\"&<>");
            var dummyCodeBlock = new CodeBlock(null) { Lines = dummyLines };
            var dummyFlexiCodeBlockOptions = new FlexiCodeBlockOptions(copyIconMarkup: null);
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
</header>
<pre><code>dummyCode&quot;&amp;&lt;&gt;</code></pre>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        public FlexiCodeBlockRenderer CreateFlexiCodeBlockRenderer(IPrismService prismService = null, IHighlightJSService highlightJSService = null,
            ILineEmbellishmentsService lineEmbellishmentsService = null)
        {
            return new FlexiCodeBlockRenderer(prismService, highlightJSService, lineEmbellishmentsService);
        }
    }
}
