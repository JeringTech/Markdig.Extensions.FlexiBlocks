using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCardsBlocks
{
    public class FlexiCardsBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCardsBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCardsBlocksExtension(
                null,
                _mockRepository.Create<BlockParser<FlexiCardBlock>>().Object,
                _mockRepository.Create<BlockRenderer<FlexiCardsBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCardBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCardsBlocksExtension(
                _mockRepository.Create<ProxyBlockParser<FlexiCardsBlock, ProxyFlexiCardsBlock>>().Object,
                null,
                _mockRepository.Create<BlockRenderer<FlexiCardsBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCardsBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCardsBlocksExtension(
                _mockRepository.Create<ProxyBlockParser<FlexiCardsBlock, ProxyFlexiCardsBlock>>().Object,
                _mockRepository.Create<BlockParser<FlexiCardBlock>>().Object,
                null));
        }
    }
}
