using Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiBannerBlocks
{
    public class FlexiBannerBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiBannerBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiBannerBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiBannerBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiBannerBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiBannerBlocksExtension(_mockRepository.Create<BlockParser<FlexiBannerBlock>>().Object, null));
        }
    }
}
