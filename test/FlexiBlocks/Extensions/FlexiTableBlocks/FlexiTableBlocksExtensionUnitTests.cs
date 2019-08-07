using Jering.Markdig.Extensions.FlexiBlocks.FlexiTableBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTableBlocks
{
    public class FlexiTableBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTableBlockParsersIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiTableBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfFlexiTableBlockParsersIsEmpty()
        {
            // Act and assert
            Assert.Throws<ArgumentException>(() => new FlexiTableBlocksExtension(new ProxyBlockParser<FlexiTableBlock, ProxyTableBlock>[0],
                _mockRepository.Create<BlockRenderer<FlexiTableBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTableBlockRendererIsNull()
        {
            // Arrange
            var dummyFlexiTableBlockParsers =
                new ProxyBlockParser<FlexiTableBlock, ProxyTableBlock>[] { _mockRepository.Create<ProxyBlockParser<FlexiTableBlock, ProxyTableBlock>>().Object };

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTableBlocksExtension(dummyFlexiTableBlockParsers, null));
        }
    }
}