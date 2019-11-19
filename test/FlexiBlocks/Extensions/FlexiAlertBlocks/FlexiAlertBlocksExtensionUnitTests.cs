using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiAlertBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiAlertBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiAlertBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertBlocksExtension(_mockRepository.Create<BlockParser<FlexiAlertBlock>>().Object, null));
        }
    }
}
