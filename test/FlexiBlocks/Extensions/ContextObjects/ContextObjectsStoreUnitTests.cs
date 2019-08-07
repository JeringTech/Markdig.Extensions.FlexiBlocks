using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig.Parsers;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.ContextObjects
{
    public class ContextObjectsStoreUnitTests
    {
        [Fact]
        public void TryOpen_ReturnsBlockStateNone()
        {
            // Arrange
            ContextObjectsStore testSubject = CreateContextObjectsStore();

            // Act
            BlockState result = testSubject.TryOpen(null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        private ContextObjectsStore CreateContextObjectsStore()
        {
            return new ContextObjectsStore();
        }
    }
}