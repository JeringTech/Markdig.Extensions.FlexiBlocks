using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlocksExtensionUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiIncludeBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiIncludeBlocksExtension(null));
        }
    }
}
