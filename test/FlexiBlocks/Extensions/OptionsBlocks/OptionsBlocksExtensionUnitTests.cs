using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.OptionsBlocks
{
    public class OptionsBlocksExtensionUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsBlockParserIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new OptionsBlocksExtension(null));
        }
    }
}