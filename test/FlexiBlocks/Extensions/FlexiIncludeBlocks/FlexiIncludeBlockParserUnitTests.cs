using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new FlexiIncludeBlockParser(_mockRepository.Create<IJsonBlockFactory<FlexiIncludeBlock, ProxyJsonBlock>>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('i', testSubject.OpeningCharacters[0]);
        }
    }
}
