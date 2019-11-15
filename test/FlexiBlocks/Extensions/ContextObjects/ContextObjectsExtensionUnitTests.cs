using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.ContextObjects
{
    public class ContextObjectsExtensionUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfContextObjectsStoreIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ContextObjectsExtension(null));
        }

        [Fact]
        public void Setup_InsertsContextObjectsStoreIntoBlockParsersIfItDoesNotContainAContextObjectsStore()
        {
            // Arrange
            var dummyContextObjectsStore = new ContextObjectsStore();
            ContextObjectsExtension testSubject = CreateContextObjectsExtension(dummyContextObjectsStore);
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();

            // Act
            testSubject.Setup(dummyMarkdownPipelineBuilder);

            // Assert
            Assert.Contains(dummyContextObjectsStore, dummyMarkdownPipelineBuilder.BlockParsers);
        }

        [Fact]
        public void Setup_DoesNothingIfBlockParsersContainsAContextObjectsStore()
        {
            // Arrange
            var dummyContextObjectsStore = new ContextObjectsStore();
            ContextObjectsExtension testSubject = CreateContextObjectsExtension(dummyContextObjectsStore);
            var dummyMarkdownPipelineBuilder = new MarkdownPipelineBuilder();
            dummyMarkdownPipelineBuilder.BlockParsers.Add(new ContextObjectsStore());

            // Act
            testSubject.Setup(dummyMarkdownPipelineBuilder);

            // Assert
            // Can't use Assert.DoesNotContain here:
            // Because ContextObjectsStore implements IDictionary<object, object>, Xunit compares the ContextObjectsStore instances as dictionaries:
            // - https://github.com/xunit/assert.xunit/blob/92395f59e3ff47d6d5a1edeced9e99ccfef9fd37/Sdk/AssertEqualityComparer.cs#L84
            // They're found to be "equal" because they have the same contents. What we want to check is whether the same reference exists in the collection.
            Assert.NotSame(dummyContextObjectsStore, dummyMarkdownPipelineBuilder.BlockParsers.Find<ContextObjectsStore>());
        }

        private ContextObjectsExtension CreateContextObjectsExtension(ContextObjectsStore contextObjectsStore = null)
        {
            return new ContextObjectsExtension(contextObjectsStore);
        }
    }
}
