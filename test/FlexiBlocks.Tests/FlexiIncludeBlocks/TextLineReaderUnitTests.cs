using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class TextLineReaderUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfTextIsNullOrAnEmptyString_Data))]
        public void Constructor_ThrowsArgumentExceptionIfTextIsNullOrAnEmptyString(string dummyText)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new TextLineReader(dummyText, 0));
            Assert.Equal(Strings.ArgumentException_ValueCannotBeNullOrAnEmptyString + "\nParameter name: text", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfTextIsNullOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{""}
            };
        }

        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentOutOfRangeExceptionIfStartCharIndexIsOutOfRange_Data))]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartCharIndexIsOutOfRange(int dummyStartCharIndex)
        {
            // Arrange
            const string dummyText = "dummyText";

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => new TextLineReader(dummyText, dummyStartCharIndex));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_ValueMustBeWithinTheIntervalContainingBuffersIndices, "text") + "\nParameter name: startCharIndex",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentOutOfRangeExceptionIfStartCharIndexIsOutOfRange_Data()
        {
            return new object[][]
            {
                new object[]{-1}, // < 0
                new object[]{9}, // == text.length
                new object[]{10} // > text.length
            };
        }

        [Fact]
        public void Read_ThrowsArgumentNullExceptionIfBufferIsNull()
        {
            // Arrange
            var testSubject = new TextLineReader("dummyText", 0);

            // Act and assert
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => testSubject.Read(null, 0, 0));
            Assert.Equal("Value cannot be null.\nParameter name: buffer",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(Read_ThrowsArgumentOutOfRangeExceptionIfIndexIsNotInTheExpectedInterval_Data))]
        public void Read_ThrowsArgumentOutOfRangeExceptionIfIndexIsNotInTheExpectedInterval(int dummyIndex)
        {
            // Arrange
            var dummyBuffer = new char[10];
            var testSubject = new TextLineReader("dummyText", 0);

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => testSubject.Read(dummyBuffer, dummyIndex, 0));
            Assert.Equal(string.Format(Strings.ArgumentOutOfRangeException_ValueMustBeWithinTheIntervalContainingBuffersIndices, "buffer") + "\nParameter name: index", 
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Read_ThrowsArgumentOutOfRangeExceptionIfIndexIsNotInTheExpectedInterval_Data()
        {
            return new object[][]
            {
                new object[]{-1}, // < 0
                new object[]{10}, // == buffer.Length
                new object[]{11} // > buffer.Length
            };
        }

        [Theory]
        [MemberData(nameof(Read_ThrowsArgumentOutOfRangeExceptionIfCountIsNotInTheExpectedInterval_Data))]
        public void Read_ThrowsArgumentOutOfRangeExceptionIfCountIsNotInTheExpectedInterval(int dummyCount)
        {
            // Arrange
            const int dummyIndex = 5;
            var dummyBuffer = new char[10];
            var testSubject = new TextLineReader("dummyText", 0);

            // Act and assert
            ArgumentOutOfRangeException result = Assert.Throws<ArgumentOutOfRangeException>(() => testSubject.Read(dummyBuffer, dummyIndex, dummyCount));
            Assert.Equal(Strings.ArgumentOutOfRangeException_CountCannotBeNegativeOrGreaterThanTheNumberOfEmptyElementsInBuffer + "\nParameter name: count",
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Read_ThrowsArgumentOutOfRangeExceptionIfCountIsNotInTheExpectedInterval_Data()
        {
            return new object[][]
            {
                new object[]{-1}, // < 0
                new object[]{6}, // > buffer.Length - index
            };
        }

        [Theory]
        [MemberData(nameof(Read_ReadsLinesAndRecordsNumberOfLinesRead_Data))]
        public void Read_ReadsLinesAndRecordsNumberOfLinesRead(int dummyInitialBufferLength, string dummyText, int dummyStartCharIndex,
            int expectedLinesRead, string expectedResult)
        {
            // Arrange
            var dummyBuffer = new char[dummyInitialBufferLength];
            var testSubject = new TextLineReader(dummyText, dummyStartCharIndex);

            // Act
            int bufferIndex = 0;
            int bufferCount = dummyInitialBufferLength;
            int readCount = 0;
            do
            {
                readCount = testSubject.Read(dummyBuffer, bufferIndex, bufferCount);
                bufferIndex += readCount;
                bufferCount -= readCount;

                if (bufferCount == 0)
                {
                    var newDummyBuffer = new char[dummyBuffer.Length * 2];
                    Array.Copy(dummyBuffer, 0, newDummyBuffer, 0, dummyBuffer.Length);
                    bufferCount = dummyBuffer.Length;
                    dummyBuffer = newDummyBuffer;
                }
            } while (readCount > 0);

            // Assert
            string resultString = string.Join("", dummyBuffer.Where(c => c != '\0'));
            Assert.Equal(expectedResult, resultString);
            Assert.Equal(testSubject.LinesRead, expectedLinesRead);
        }

        public static IEnumerable<object[]> Read_ReadsLinesAndRecordsNumberOfLinesRead_Data()
        {
            return new object[][]
            {
                // Line feeds only
                new object[]{
                    32,
                    "line 1\nline 2\nline 3\n",
                    0,
                    3,
                    "line 1\nline 2\nline 3\n"
                },
                // Carraige returns only
                new object[]{
                    32,
                    "line 1\rline 2\rline 3\r",
                    0,
                    3,
                    "line 1\nline 2\nline 3\n"
                },
                // Carriage returns and line feeds
                new object[]{
                    32,
                    "line 1\rline 2\r\nline 3\n",
                    0,
                    3,
                    "line 1\nline 2\nline 3\n"
                },
                // Last line ends without an end of line character
                new object[]{
                    32,
                    "line 1\nline 2\nline 3",
                    0,
                    3,
                    "line 1\nline 2\nline 3"
                },
                // Non-zero start char index
                new object[]{
                    32,
                    "line 1\nline 2\nline 3",
                    7,
                    2,
                    "line 2\nline 3"
                },
                // Single line
                new object[]{
                    32,
                    "line 1",
                    0,
                    1,
                    "line 1"
                },
                // Full buffer
                new object[]{
                    16,
                    "line 1\nline 2\nline 3\n",
                    0,
                    3,
                    "line 1\nline 2\nline 3\n"
                }
            };
        }
    }
}
