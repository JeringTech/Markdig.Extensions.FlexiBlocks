using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks.LineEmbellisherService;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class LineEmbellisherServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Theory]
        [MemberData(nameof(EmbellishLines_ReturnsTextIfTextIsNullOrEmpty_Data))]
        public void EmbellishLines_ReturnsTextIfTextIsNullOrEmpty(string dummyText)
        {
            // Arrange
            var testSubject = new LineEmbellisherService();

            // Act  
            string result = testSubject.EmbellishLines(dummyText,
                new List<NumberedLineRange> { new NumberedLineRange() },
                new List<LineRange> { new LineRange() });

            // Assert
            Assert.Equal(dummyText, result);
        }

        public static IEnumerable<object[]> EmbellishLines_ReturnsTextIfTextIsNullOrEmpty_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{string.Empty}
            };
        }

        [Fact]
        public void EmbellishLines_ThrowsInvalidOperationExceptionIfExtractStartTagInfosThrowsAnException()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyException = new Exception();
            Mock<LineEmbellisherService> mockTestSubject = _mockRepository.Create<LineEmbellisherService>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ExtractOpenStartTagInfos(dummyText, It.IsAny<Stack<StartTagInfo>>())).Throws(dummyException);

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockTestSubject.Object.EmbellishLines(dummyText, null, null));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_LineEmbellisherService_InvalidMarkupFragmentWithInnerException, dummyText), result.Message);
            Assert.Equal(dummyException, result.InnerException);
        }

        [Fact]
        public void EmbellishLines_ThrowsInvalidOperationExceptionIfOpenStartTagInfosIsNotEmptyAfterAllLinesHaveBeenEmbellished()
        {
            // Arrange
            const string dummyText = "dummyText";
            var dummyException = new Exception();
            Mock<LineEmbellisherService> mockTestSubject = _mockRepository.Create<LineEmbellisherService>();
            mockTestSubject.CallBase = true;
            mockTestSubject.
                Setup(t => t.ExtractOpenStartTagInfos(dummyText, It.IsAny<Stack<StartTagInfo>>())).
                Callback<string, Stack<StartTagInfo>>((_, openStartTagInfos) => openStartTagInfos.Push(new StartTagInfo(" ", 0, 0, 0)));

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockTestSubject.Object.EmbellishLines(dummyText, null, null));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_LineEmbellisherService_InvalidMarkupFragment, dummyText), result.Message);
        }

        [Theory]
        [MemberData(nameof(EmbellishLines_EmbellishesLines_Data))]
        public void EmbellishLines_EmbellishesLines(SerializableWrapper<List<NumberedLineRange>> dummyLineNumberLineRanges,
            SerializableWrapper<List<LineRange>> dummyHighlightLineRanges,
            string dummyText,
            string dummyPrefixForClasses,
            string dummyHiddenLinesIconMarkup,
            bool dummySplitMultilineElements,
            string expectedResult)
        {
            // Arrange
            var testSubject = new LineEmbellisherService();

            // Act
            string result = testSubject.EmbellishLines(dummyText, dummyLineNumberLineRanges?.Value, dummyHighlightLineRanges?.Value, dummyPrefixForClasses, dummyHiddenLinesIconMarkup, dummySplitMultilineElements);

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> EmbellishLines_EmbellishesLines_Data()
        {
            const string dummyText = @"line 1
line 2
line 3
line 4
line 5
line 6
line 7
line 8
line 9
line 10";
            return new object[][]
            {
                // Both line numbers and highlighting
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange> { new NumberedLineRange(1, 4, 1), new NumberedLineRange(7, 10, 7) }
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange> { new LineRange(2, 2), new LineRange(8, 9) }
                    ),
                    dummyText,
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">line 1</span></span>
<span class=""line highlight""><span class=""line-number"">2</span><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">line 3</span></span>
<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">line 4</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-number"">7</span><span class=""line-text"">line 7</span></span>
<span class=""line highlight""><span class=""line-number"">8</span><span class=""line-text"">line 8</span></span>
<span class=""line highlight""><span class=""line-number"">9</span><span class=""line-text"">line 9</span></span>
<span class=""line""><span class=""line-number"">10</span><span class=""line-text"">line 10</span></span>"
                },
                // Both line numbers and highlighting using -1 to specify end lines
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange> { new NumberedLineRange(1, 4, 1), new NumberedLineRange(7, -1, 7) }
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange> { new LineRange(2, 2), new LineRange(9, -1) }
                    ),
                    dummyText,
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">line 1</span></span>
<span class=""line highlight""><span class=""line-number"">2</span><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">line 3</span></span>
<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">line 4</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-number"">7</span><span class=""line-text"">line 7</span></span>
<span class=""line""><span class=""line-number"">8</span><span class=""line-text"">line 8</span></span>
<span class=""line highlight""><span class=""line-number"">9</span><span class=""line-text"">line 9</span></span>
<span class=""line highlight""><span class=""line-number"">10</span><span class=""line-text"">line 10</span></span>"
                },
                // Only line numbers (empty list of highlight line ranges) 
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange> { new NumberedLineRange(1, 4, 2), new NumberedLineRange(7, -1, 7) }
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange>()
                    ),
                    dummyText,
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-number"">2</span><span class=""line-text"">line 1</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">line 3</span></span>
<span class=""line""><span class=""line-number"">5</span><span class=""line-text"">line 4</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-number"">7</span><span class=""line-text"">line 7</span></span>
<span class=""line""><span class=""line-number"">8</span><span class=""line-text"">line 8</span></span>
<span class=""line""><span class=""line-number"">9</span><span class=""line-text"">line 9</span></span>
<span class=""line""><span class=""line-number"">10</span><span class=""line-text"">line 10</span></span>"
                },
                // Only line numbers (null list of highlight line ranges)
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange> { new NumberedLineRange(1, 4, 1), new NumberedLineRange(7, 8, 7) }
                    ),
                    null,
                    dummyText,
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">line 1</span></span>
<span class=""line""><span class=""line-number"">2</span><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">line 3</span></span>
<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">line 4</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-number"">7</span><span class=""line-text"">line 7</span></span>
<span class=""line""><span class=""line-number"">8</span><span class=""line-text"">line 8</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 9</span></span>
<span class=""line""><span class=""line-number""></span><span class=""line-text"">line 10</span></span>"
                },
                // Hidden lines icon markup specified
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange> { new NumberedLineRange(1, 4, 1), new NumberedLineRange(7, 8, 7) }
                    ),
                    null,
                    dummyText,
                    null,
                    "dummyHiddenLinesIconMarkup",
                    true,
                    @"<span class=""line""><span class=""line-number"">1</span><span class=""line-text"">line 1</span></span>
<span class=""line""><span class=""line-number"">2</span><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-number"">3</span><span class=""line-text"">line 3</span></span>
<span class=""line""><span class=""line-number"">4</span><span class=""line-text"">line 4</span></span>
<span class=""line""><span class=""line-number"">dummyHiddenLinesIconMarkup</span><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-number"">dummyHiddenLinesIconMarkup</span><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-number"">7</span><span class=""line-text"">line 7</span></span>
<span class=""line""><span class=""line-number"">8</span><span class=""line-text"">line 8</span></span>
<span class=""line""><span class=""line-number"">dummyHiddenLinesIconMarkup</span><span class=""line-text"">line 9</span></span>
<span class=""line""><span class=""line-number"">dummyHiddenLinesIconMarkup</span><span class=""line-text"">line 10</span></span>"
                },
                // Only highlighting (null list of line number line ranges)
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange>()
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange> { new LineRange(2, 2), new LineRange(9, -1) }
                    ),
                    dummyText,
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-text"">line 1</span></span>
<span class=""line highlight""><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-text"">line 3</span></span>
<span class=""line""><span class=""line-text"">line 4</span></span>
<span class=""line""><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-text"">line 7</span></span>
<span class=""line""><span class=""line-text"">line 8</span></span>
<span class=""line highlight""><span class=""line-text"">line 9</span></span>
<span class=""line highlight""><span class=""line-text"">line 10</span></span>"
                },
                // Only highlighting (null list of line number line ranges)
                new object[]{
                    null,
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange> { new LineRange(2, 2), new LineRange(4, 5) }
                    ),
                    dummyText,
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-text"">line 1</span></span>
<span class=""line highlight""><span class=""line-text"">line 2</span></span>
<span class=""line""><span class=""line-text"">line 3</span></span>
<span class=""line highlight""><span class=""line-text"">line 4</span></span>
<span class=""line highlight""><span class=""line-text"">line 5</span></span>
<span class=""line""><span class=""line-text"">line 6</span></span>
<span class=""line""><span class=""line-text"">line 7</span></span>
<span class=""line""><span class=""line-text"">line 8</span></span>
<span class=""line""><span class=""line-text"">line 9</span></span>
<span class=""line""><span class=""line-text"">line 10</span></span>"
                },
                // Prefix specified
                new object[]{
                    new SerializableWrapper<List<NumberedLineRange>>(
                        new List<NumberedLineRange> { new NumberedLineRange(1, 4, 1), new NumberedLineRange(7, 10, 7) }
                    ),
                    new SerializableWrapper<List<LineRange>>(
                        new List<LineRange> { new LineRange(2, 2), new LineRange(8, 9) }
                    ),
                    dummyText,
                    "dummy-prefix-",
                    null,
                    true,
                    @"<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number"">1</span><span class=""dummy-prefix-line-text"">line 1</span></span>
<span class=""dummy-prefix-line dummy-prefix-highlight""><span class=""dummy-prefix-line-number"">2</span><span class=""dummy-prefix-line-text"">line 2</span></span>
<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number"">3</span><span class=""dummy-prefix-line-text"">line 3</span></span>
<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number"">4</span><span class=""dummy-prefix-line-text"">line 4</span></span>
<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number""></span><span class=""dummy-prefix-line-text"">line 5</span></span>
<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number""></span><span class=""dummy-prefix-line-text"">line 6</span></span>
<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number"">7</span><span class=""dummy-prefix-line-text"">line 7</span></span>
<span class=""dummy-prefix-line dummy-prefix-highlight""><span class=""dummy-prefix-line-number"">8</span><span class=""dummy-prefix-line-text"">line 8</span></span>
<span class=""dummy-prefix-line dummy-prefix-highlight""><span class=""dummy-prefix-line-number"">9</span><span class=""dummy-prefix-line-text"">line 9</span></span>
<span class=""dummy-prefix-line""><span class=""dummy-prefix-line-number"">10</span><span class=""dummy-prefix-line-text"">line 10</span></span>"
                },
                // Multi-line elements get split up if splitMultilineElements is true
                new object[]{
                    null,
                    null,
                    @"<span class=""token comment"">/*
    This
    <strong>Is
<em>A

    Multi-line</em>
Comment</strong>
*/</span>",
                    null,
                    null,
                    true,
                    @"<span class=""line""><span class=""line-text""><span class=""token comment"">/*</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">    This</span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">    <strong>Is</strong></span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment""><strong><em>A</em></strong></span></span></span>
<span class=""line""><span class=""line-text""></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment""><strong><em>    Multi-line</em></strong></span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment""><strong>Comment</strong></span></span></span>
<span class=""line""><span class=""line-text""><span class=""token comment"">*/</span></span></span>"
                },
                // Multi-line elements do not get split up if splitMultilineElements is false (the result isn't valid, this test exists to confirm that setting splitMultilineElements to false works as expected)
                new object[]{
                    null,
                    null,
                    @"<span class=""token comment"">/*
    This
    <strong>Is
<em>A

    Multi-line</em>
Comment</strong>
*/</span>",
                    null,
                    null,
                    false,
                    @"<span class=""line""><span class=""line-text""><span class=""token comment"">/*</span></span>
<span class=""line""><span class=""line-text"">    This</span></span>
<span class=""line""><span class=""line-text"">    <strong>Is</span></span>
<span class=""line""><span class=""line-text""><em>A</span></span>
<span class=""line""><span class=""line-text""></span></span>
<span class=""line""><span class=""line-text"">    Multi-line</em></span></span>
<span class=""line""><span class=""line-text"">Comment</strong></span></span>
<span class=""line""><span class=""line-text"">*/</span></span></span>"
                },
            };
        }

        [Theory]
        [MemberData(nameof(ExtractOpenStartTagInfos_ExtractsStartTagInfos_Data))]
        public void ExtractOpenStartTagInfos_ExtractsStartTagInfos(string dummyLine,
            object[] dummyOpenStartTagInfosAsObjects, // StartTagInfo is internal, so we have use a stack of objects
            object[] expectedOpenStartTagInfosAsObjects) // StartTagInfo is internal, so we have to use a stack of objects
        {
            // Arrange
            var dummyOpenStartTagInfos = new Stack<StartTagInfo>(dummyOpenStartTagInfosAsObjects.Cast<StartTagInfo>());
            var expectedOpenStartTagInfos = new Stack<StartTagInfo>(expectedOpenStartTagInfosAsObjects.Cast<StartTagInfo>());
            var testSubject = new LineEmbellisherService();

            // Act
            testSubject.ExtractOpenStartTagInfos(dummyLine, dummyOpenStartTagInfos);

            // Assert
            Assert.Equal(expectedOpenStartTagInfos, dummyOpenStartTagInfos);
        }

        public static IEnumerable<object[]> ExtractOpenStartTagInfos_ExtractsStartTagInfos_Data()
        {
            const string dummyTagFormat = "span{0}class=\"dummy-class\"";
            string dummyLineWithOpenStartTagFormat = $"<{dummyTagFormat}><span>text</span>";

            return new object[][]
            {
                // Balanced line
                new object[]{
                    "<span>text <span> text</span>text text</span>",
                    new object[0],
                    new object[0]
                },
                // Line balanced with an existing open start tag info
                new object[]{
                    "text <span> text</span>text text</span>",
                    new object[]{ new StartTagInfo() },
                    new object[0]
                },
                // Line balanced with multiple existing open start tag infos
                new object[]{
                    "text <span> text</span>text</span> text</span>",
                    new object[]{ new StartTagInfo(), new StartTagInfo() },
                    new object[0]
                },
                // Line with an open start tag
                new object[]{
                    "<span><span>text</span>",
                    new object[0],
                    new object[]{ new StartTagInfo("<span><span>text</span>", 1, 4, 4) }
                },
                // Line with multiple open start tags
                new object[]{
                    "<span><span><span>text</span>text",
                    new object[0],
                    new object[]{ new StartTagInfo("<span><span><span>text</span>text", 1, 4, 4), new StartTagInfo("<span><span><span>text</span>text", 7, 10, 10) }
                },
                // Line with open start tag that contains attributes
                new object[]{
                    string.Format(dummyLineWithOpenStartTagFormat, ' '),
                    new object[0],
                    new object[]{ new StartTagInfo(string.Format(dummyLineWithOpenStartTagFormat, ' '), 1, dummyTagFormat.Length - 2, 4) }
                },
                new object[]{
                    string.Format(dummyLineWithOpenStartTagFormat, '\n'),
                    new object[0],
                    new object[]{ new StartTagInfo(string.Format(dummyLineWithOpenStartTagFormat, '\n'), 1, dummyTagFormat.Length - 2, 4) }
                },
                new object[]{
                    string.Format(dummyLineWithOpenStartTagFormat, '\t'),
                    new object[0],
                    new object[]{ new StartTagInfo(string.Format(dummyLineWithOpenStartTagFormat, '\t'), 1, dummyTagFormat.Length - 2, 4) }
                },
                new object[]{
                    string.Format(dummyLineWithOpenStartTagFormat, '\r'),
                    new object[0],
                    new object[]{ new StartTagInfo(string.Format(dummyLineWithOpenStartTagFormat, '\r'), 1, dummyTagFormat.Length - 2, 4) }
                },
                // Line with no tags
                new object[]{
                    "text text text",
                    new object[0],
                    new object[0]
                }
            };
        }

        [Theory]
        [MemberData(nameof(StartTagInfo_Length_ReturnsTagLength_Data))]
        public void StartTagInfo_Length_ReturnsTagLength(int dummyStartIndex, int dummyEndIndex, int expectedLength)
        {
            // Arrange
            var dummyTestSubject = new StartTagInfo(null, dummyStartIndex, dummyEndIndex, 0);

            // Act
            int result = dummyTestSubject.Length;

            // Assert
            Assert.Equal(expectedLength, result);
        }

        public static IEnumerable<object[]> StartTagInfo_Length_ReturnsTagLength_Data()
        {
            return new object[][]
            {
                new object[]{1, 4, 4},
                // Minimum length is 1
                new object[]{2, 2, 1}
            };
        }

        [Theory]
        [MemberData(nameof(StartTagInfo_NameLength_ReturnsTagNameLength_Data))]
        public void StartTagInfo_NameLength_ReturnsTagNameLength(int dummyStartIndex, int dummyNameEndIndex, int expectedNameLength)
        {
            // Arrange
            var dummyTestSubject = new StartTagInfo(null, dummyStartIndex, 0, dummyNameEndIndex);

            // Act
            int result = dummyTestSubject.NameLength;

            // Assert
            Assert.Equal(expectedNameLength, result);
        }

        public static IEnumerable<object[]> StartTagInfo_NameLength_ReturnsTagNameLength_Data()
        {
            return new object[][]
            {
                new object[]{1, 4, 4},
                // Minimum NameLength is 1
                new object[]{2, 2, 1}
            };
        }
    }
}
