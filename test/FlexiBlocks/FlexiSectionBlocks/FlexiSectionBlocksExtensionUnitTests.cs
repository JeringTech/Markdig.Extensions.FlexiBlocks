using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlocksExtensionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiSectionBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlocksExtension(null, new FlexiSectionBlockRenderer()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiSectionBlockRendererIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() =>
                new FlexiSectionBlocksExtension(new FlexiSectionBlockParser(_mockRepository.Create<IFlexiOptionsBlockService>().Object,
                    new FlexiSectionBlocksExtensionOptions()), null));
        }
    }
}
