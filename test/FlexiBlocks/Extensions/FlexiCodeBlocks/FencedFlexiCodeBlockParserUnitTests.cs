using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class FencedFlexiCodeBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new FencedFlexiCodeBlockParser(_mockRepository.Create<IFlexiCodeBlockFactory>().Object);

            // Assert
            Assert.Equal(2, testSubject.OpeningCharacters.Length);
            Assert.Equal('`', testSubject.OpeningCharacters[0]);
            Assert.Equal('~', testSubject.OpeningCharacters[1]);
        }
    }
}
