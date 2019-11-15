using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class LineRangeComparerUnitTests
    {
        [Theory]
        [MemberData(nameof(Compare_ComparesLineRanges_Data))]
        public void Compare_ComparesLineRanges(int dummyXStart, int dummyXEnd,
            int dummyYStart, int dummyYEnd,
            int dummyNumLines,
            int expectedResult)
        {
            // Arrange
            var dummyX = new LineRange(dummyXStart, dummyXEnd);
            var dummyY = new LineRange(dummyYStart, dummyYEnd);
            var testSubject = new LineRangeComparer(dummyNumLines);

            // Act
            int result = testSubject.Compare(dummyX, dummyY);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Compare_ComparesLineRanges_Data()
        {
            return new object[][]
            {
                // X starts before y
                new object[]{ 3, 15, 4, 16, 16, -1},
                // X and y start at the same line, x ends after y
                new object[]{ 5, 58, 5, 27, 60, -1},
                // Y starts before x
                new object[]{ 4, 16, 3, 15, 16, 1},
                // Y and x start at the same line, y ends after x
                new object[]{ 5, 27, 5, 58, 60, 1},
                // X and y are identical
                new object[]{ 4, 16, 4, 16, 16, 0}
            };
        }
    }
}
