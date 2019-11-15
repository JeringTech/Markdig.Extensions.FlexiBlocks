using Jering.Markdig.Extensions.FlexiBlocks.FlexiQuoteBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiQuoteBlocks
{
    public class FlexiQuoteBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiQuoteBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiQuoteBlocksExtension(null, _mockRepository.Create<BlockRenderer<FlexiQuoteBlock>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiQuoteBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiQuoteBlocksExtension(_mockRepository.Create<BlockParser<FlexiQuoteBlock>>().Object, null));
        }
    }
}
