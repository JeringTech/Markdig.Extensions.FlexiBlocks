using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTabsBlocks
{
    public class FlexiTabsBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTabsBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTabsBlocksExtension(
                null,
                _mockRepository.Create<BlockParser<FlexiTabBlock>>().Object,
                _mockRepository.Create<BlockRenderer<FlexiTabsBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTabBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTabsBlocksExtension(
                _mockRepository.Create<ProxyBlockParser<FlexiTabsBlock, ProxyFlexiTabsBlock>>().Object,
                null,
                _mockRepository.Create<BlockRenderer<FlexiTabsBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTabsBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTabsBlocksExtension(
                _mockRepository.Create<ProxyBlockParser<FlexiTabsBlock, ProxyFlexiTabsBlock>>().Object,
                _mockRepository.Create<BlockParser<FlexiTabBlock>>().Object,
                null));
        }
    }
}
