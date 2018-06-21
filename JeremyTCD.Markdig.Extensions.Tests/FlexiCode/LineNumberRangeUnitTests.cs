using JeremyTCD.Markdig.Extensions.FlexiCode;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.FlexiCode
{
    public class LineNumberRangeUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsExceptionIfStartLineNumberIsInvalid_Data))]
        public void Constructor_ThrowsExceptionIfEndLineIsInvalid(int startLineNumber)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new LineNumberRange(1, 1, startLineNumber));
            Assert.Equal(string.Format(Strings.ArgumentException_InvalidStartLineNumber, startLineNumber), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsExceptionIfStartLineNumberIsInvalid_Data()
        {
            return new object[][]
            {
                new object[]{ 0 }, // Cannot be less than 1
                new object[]{ -1 } // Cannot be less than 1
            };
        }

        [Fact]
        public void Constructor_CorrectlyAssignsValuesIfSuccessful()
        {
            // Arrange
            const int dummyStartLine = 1;
            const int dummyEndLine = 2;
            const int dummyStartLineNumber = 3;

            // Act
            var result = new LineNumberRange(dummyStartLine, dummyEndLine, dummyStartLineNumber);

            // Assert
            Assert.Equal(dummyStartLine, result.LineRange.StartLine);
            Assert.Equal(dummyEndLine, result.LineRange.EndLine);
            Assert.Equal(dummyStartLineNumber, result.StartLineNumber);
        }
    }
}
