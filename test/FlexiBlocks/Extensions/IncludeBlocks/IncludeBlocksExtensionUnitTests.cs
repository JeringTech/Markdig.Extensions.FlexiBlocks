using Jering.Markdig.Extensions.FlexiBlocks.IncludeBlocks;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.IncludeBlocks
{
    public class IncludeBlocksExtensionUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfIncludeBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new IncludeBlocksExtension(null));
        }
    }
}
