using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    public class FlexiPictureBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new FlexiPictureBlockParser(_mockRepository.Create<IJsonBlockFactory<FlexiPictureBlock, ProxyJsonBlock>>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('p', testSubject.OpeningCharacters[0]);
        }
    }
}
