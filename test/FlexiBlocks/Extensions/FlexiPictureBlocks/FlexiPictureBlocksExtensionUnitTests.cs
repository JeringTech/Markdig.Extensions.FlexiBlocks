using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    public class FlexiPictureBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiPictureBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiPictureBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiPictureBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiPictureBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiPictureBlocksExtension(_mockRepository.Create<ProxyBlockParser<FlexiPictureBlock, ProxyJsonBlock>>().Object, null));
        }
    }
}
