using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    public class FlexiVideoBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new FlexiVideoBlockParser(_mockRepository.Create<IJsonBlockFactory<FlexiVideoBlock, ProxyJsonBlock>>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('v', testSubject.OpeningCharacters[0]);
        }
    }
}
