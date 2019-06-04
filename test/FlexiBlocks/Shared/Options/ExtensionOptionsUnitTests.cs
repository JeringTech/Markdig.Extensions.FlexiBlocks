using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class ExtensionOptionsUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfDefaultBlockOptionsIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExposedExtensionOptions<DummyOptions>(null));
        }

        private class ExposedExtensionOptions<T> : ExtensionOptions<T> where T : class, IBlockOptions<T>
        {
            public ExposedExtensionOptions(T defaultBlockOptions) : base(defaultBlockOptions)
            {
            }
        }

        private class DummyOptions : BlockOptions<DummyOptions>
        {
        }
    }
}
