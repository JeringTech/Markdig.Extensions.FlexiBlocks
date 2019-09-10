using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class BacktickFencedFlexiCodeBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new BacktickFencedFlexiCodeBlockParser(_mockRepository.Create<IFlexiCodeBlockFactory>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('`', testSubject.OpeningCharacters[0]);
        }
    }
}
