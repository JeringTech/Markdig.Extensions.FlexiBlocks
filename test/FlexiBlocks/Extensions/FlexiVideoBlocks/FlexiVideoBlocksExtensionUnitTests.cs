using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    public class FlexiVideoBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiVideoBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiVideoBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiVideoBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiVideoBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiVideoBlocksExtension(_mockRepository.Create<ProxyBlockParser<FlexiVideoBlock, ProxyJsonBlock>>().Object, null));
        }
    }
}
