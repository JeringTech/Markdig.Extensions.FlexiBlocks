using Markdig.Parsers;
using Markdig.Syntax;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlockExceptionUnitTests
    {
        [Theory]
        [MemberData(nameof(Message_ReturnsInvalidFlexiBlockMessageIfBlockTypeNameIsNotNull_Data))]
        public void Message_ReturnsInvalidFlexiBlockMessageIfBlockTypeNameIsNotNull(string dummyDescription, string dummyExpectedDescription)
        {
            // Arrange
            const int dummyColumn = 2; // Arbitrary
            const int dummyLineIndex = 5; // Arbitrary
            var dummyBlock = new DummyBlock(null)
            {
                Column = dummyColumn,
                Line = dummyLineIndex
            };
            var testSubject = new FlexiBlocksException(dummyBlock, dummyDescription);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal($@"The DummyBlock starting at line ""6"", column ""2"", is invalid:
{dummyExpectedDescription}", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Message_ReturnsInvalidFlexiBlockMessageIfBlockTypeNameIsNotNull_Data()
        {
            return new object[][]
            {
                new object[]{"dummyDescription", "dummyDescription"},
                new object[]{null, Strings.FlexiBlocksException_UnexpectedException}
            };
        }

        [Theory]
        [MemberData(nameof(Message_ReturnsInvalidMarkdownMessageIfBlockTypeNameIsNullButDescriptionIsNotNull_Data))]
        public void Message_ReturnsInvalidMarkdownMessageIfBlockTypeNameIsNullButDescriptionIsNotNull(string dummyDescription, string dummyExpectedDescription)
        {
            // Arrange
            const int dummyColumn = 2; // Arbitrary
            const int dummyLineIndex = 5; // Arbitrary
            var testSubject = new FlexiBlocksException(dummyLineIndex, dummyColumn, dummyDescription);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal($@"The markdown at line ""6"", column ""2"" is invalid:
{dummyExpectedDescription}", result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Message_ReturnsInvalidMarkdownMessageIfBlockTypeNameIsNullButDescriptionIsNotNull_Data()
        {
            return new object[][]
            {
                new object[]{"dummyDescription", "dummyDescription"},
                new object[]{null, Strings.FlexiBlocksException_UnexpectedException}
            };
        }

        [Fact]
        public void Message_ReturnsCustomMessageIfMessageIsNotNull()
        {
            // Arrange
            const string dummyMessage = "dummyMessage";
            var testSubject = new FlexiBlocksException(dummyMessage);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal(dummyMessage, result);
        }

        [Fact]
        public void Message_ReturnsDefaultMessageIfNoOtherMessageExistsOrCanBeCreated()
        {
            // Arrange
            var testSubject = new FlexiBlocksException();

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal($"Exception of type '{typeof(FlexiBlocksException).FullName}' was thrown.", result);
        }

        private class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
