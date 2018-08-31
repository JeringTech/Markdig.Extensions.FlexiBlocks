using Markdig.Parsers;
using Markdig.Syntax;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlockExceptionUnitTests
    {
        [Fact]
        public void Message_ReturnsExpectedMessage()
        {
            // Arrange
            const int dummyColumn = 2; // Arbitrary
            const int dummyLineIndex = 5; // Arbitrary
            const string dummyDescription = "dummyDescription";
            var dummyBlock = new DummyBlock(null)
            {
                Column = dummyColumn,
                Line = dummyLineIndex
            };
            var testSubject = new FlexiBlockException(dummyBlock, dummyDescription);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal(@"The DummyBlock starting at line ""6"", column ""2"", is invalid:
dummyDescription", result, ignoreLineEndingDifferences: true);
        }

        private class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
