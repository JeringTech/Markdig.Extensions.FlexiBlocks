using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiOptionsBlocks
{
    public class FlexiOptionsBlocksExtensionUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiOptionsBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiOptionsBlocksExtension(null));
        }
    }
}
