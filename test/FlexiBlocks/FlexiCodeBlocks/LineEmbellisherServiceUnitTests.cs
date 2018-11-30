using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class LineEmbellisherServiceUnitTests
    {
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

        [Theory]
        [MemberData(nameof(EmbellishLines_EmbellishesLines_Data))]
        public void EmbellishLines_EmbellishesLines(SerializableWrapper<List<NumberedLineRange>> dummyLineNumberLineRanges,
            SerializableWrapper<List<LineRange>> dummyHighlightLineRanges,
            string dummyPrefixForClasses,
            string dummyHiddenLinesIconMarkup,
            string expectedResult)
        {
            // Arrange
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
            var testSubject = new LineEmbellisherService();

            // Act
            string result = testSubject.EmbellishLines(dummyText, dummyLineNumberLineRanges?.Value, dummyHighlightLineRanges?.Value, dummyPrefixForClasses, dummyHiddenLinesIconMarkup);

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> EmbellishLines_EmbellishesLines_Data()
        {
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
                    null,
                    null,
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
                    null,
                    null,
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
                    null,
                    null,
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
                    null,
                    null,
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
                    null,
                    "dummyHiddenLinesIconMarkup",
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
                    null,
                    null,
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
                    null,
                    null,
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
                    "dummy-prefix-",
                    null,
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
            };
        }
    }
}
