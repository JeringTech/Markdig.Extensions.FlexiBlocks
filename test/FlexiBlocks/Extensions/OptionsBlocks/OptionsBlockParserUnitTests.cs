using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.OptionsBlocks
{
    public class OptionsBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new OptionsBlockParser(_mockRepository.Create<IJsonBlockFactory<OptionsBlock, ProxyJsonBlock>>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('@', testSubject.OpeningCharacters[0]);
        }
    }
}
