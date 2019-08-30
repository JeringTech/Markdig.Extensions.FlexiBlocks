using Jering.Markdig.Extensions.FlexiBlocks.FlexiFigureBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiFigureBlocks
{
    public class FlexiFigureBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiFigureBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiFigureBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiFigureBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiFigureBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiFigureBlocksExtension(_mockRepository.Create<BlockParser<FlexiFigureBlock>>().Object, null));
        }
    }
}
