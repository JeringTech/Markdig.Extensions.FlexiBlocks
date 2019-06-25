using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.IncludeBlocks
{
    public class IncludeBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new IncludeBlockParser(_mockRepository.Create<IJsonBlockFactory<IncludeBlock, ProxyJsonBlock>>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('+', testSubject.OpeningCharacters[0]);
        }
    }
}
