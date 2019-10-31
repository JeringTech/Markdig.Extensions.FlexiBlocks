using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class LeadingWhitespaceEditorServiceUnitTests
    {
        [Fact]
        public void Indent_ThrowsArgumentOutOfRangeExceptionIfIndentLengthIsNegative()
        {
            // Arrange
            var testSubject = new LeadingWhitespaceEditorService();
            var dummyStringSlice = new StringSlice();
            const int dummyIndentLength = -1;

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => testSubject.Indent(dummyStringSlice, dummyIndentLength));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_Shared_ValueCannotBeNegative, dummyIndentLength) + "\nParameter name: indentLength",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(Indent_IndentsStringSlice_Data))]
        public void Indent_IndentsStringSlice(string dummyLine, int dummyIndentLength, string expectedResult)
        {
            // Arrange
            var testSubject = new LeadingWhitespaceEditorService();
            var dummyStringSlice = new StringSlice(dummyLine);

            // Act
            StringSlice result = testSubject.Indent(dummyStringSlice, dummyIndentLength);

            // Assert
            Assert.Equal(expectedResult, result.ToString());
        }

        public static IEnumerable<object[]> Indent_IndentsStringSlice_Data()
        {
            return new object[][]
            {
                new object[]{"    dummyLine", 2, "      dummyLine"}, // Indent
                new object[]{"", 3, "   " }, // Indent empty string
                new object[]{"  dummyLine", 0, "  dummyLine" }, // Indent length 0
                new object[]{"    ", 2, "      " } // White space only string
            };
        }

        [Fact]
        public void Dedent_ThrowsArgumentOutOfRangeExceptionIfDedentLengthIsNegative()
        {
            // Arrange
            var testSubject = new LeadingWhitespaceEditorService();
            var dummyStringSlice = new StringSlice();
            const int dummyDedentLength = -1;

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => testSubject.Dedent(ref dummyStringSlice, dummyDedentLength));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_Shared_ValueCannotBeNegative, dummyDedentLength) + "\nParameter name: dedentLength",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(Dedent_DedentsLeadingWhitespace_Data))]
        public void Dedent_DedentsLeadingWhitespace(string dummyLine, int dummyDedentLength, string expectedResult)
        {
            // Arrange
            var testSubject = new LeadingWhitespaceEditorService();
            var dummyStringSlice = new StringSlice(dummyLine);

            // Act
            testSubject.Dedent(ref dummyStringSlice, dummyDedentLength);

            // Assert
            Assert.Equal(expectedResult, dummyStringSlice.ToString());
        }

        public static IEnumerable<object[]> Dedent_DedentsLeadingWhitespace_Data()
        {
            return new object[][]
            {
                new object[]{"    dummyLine", 2, "  dummyLine"}, // Dedent
                new object[]{"  dummyLine", 4, "dummyLine" }, // Dedent till there is no leading white space
                new object[]{"  ", 3, "" }, // Dedent till string is empty
                new object[]{"dummyLine", 2, "dummyLine" }, // Do nothing to line with no leading white space
                new object[]{"", 0, "" }, // Empty string
                new object[]{"    ", 2, "  " }, // White space only string
                new object[]{"    dummyLine", 0, "    dummyLine"}, // Do nothing
            };
        }

        [Theory]
        [MemberData(nameof(Collapse_ThrowsArgumentOutOfRangeExceptionIfCollapseRatioIsNotWithinTheAcceptedRange_Data))]
        public void Collapse_ThrowsArgumentOutOfRangeExceptionIfCollapseRatioIsNotWithinTheAcceptedRange(float dummyCollapseRatio)
        {
            // Arrange
            var testSubject = new LeadingWhitespaceEditorService();
            var dummyStringSlice = new StringSlice();

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => testSubject.Collapse(ref dummyStringSlice, dummyCollapseRatio));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_Shared_ValueMustBeWithinRange, "[0, 1]", dummyCollapseRatio) + "\nParameter name: collapseRatio",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Collapse_ThrowsArgumentOutOfRangeExceptionIfCollapseRatioIsNotWithinTheAcceptedRange_Data()
        {
            return new object[][]
            {
                new object[]{-0.1},
                new object[]{1.1}
            };
        }

        [Theory]
        [MemberData(nameof(Collapse_CollapsesLeadingWhitespace_Data))]
        public void Collapse_CollapsesLeadingWhitespace(string dummyLine, float dummyCollapseLength, string expectedResult)
        {
            // Arrange
            var testSubject = new LeadingWhitespaceEditorService();
            var dummyStringSlice = new StringSlice(dummyLine);

            // Act
            testSubject.Collapse(ref dummyStringSlice, dummyCollapseLength);

            // Assert
            Assert.Equal(expectedResult, dummyStringSlice.ToString());
        }

        public static IEnumerable<object[]> Collapse_CollapsesLeadingWhitespace_Data()
        {
            return new object[][]
            {
                new object[]{"    dummyLine", 0.5, "  dummyLine"}, // Collapse
                new object[]{"     dummyLine", 0.5, "  dummyLine"}, // Collapse when ratio*num leading white spaces != whole number
                new object[]{" dummyLine", 0, "dummyLine"}, // Collapse till there is no leading white space
                new object[]{" dummyLine", 0.3, "dummyLine"}, // Collapse till there is no leading white space (final number of leading white spaces is rounded down)
                new object[]{"dummyLine", 0.5, "dummyLine" }, // Do nothing to line with no leading white space
                new object[]{"", 0, "" }, // Empty string
                new object[]{"    ", 0.5, "  " }, // White space only string
                new object[]{"    dummyLine", 1, "    dummyLine"}, // Do nothing
            };
        }
    }
}
