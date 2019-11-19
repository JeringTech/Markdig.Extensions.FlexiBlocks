using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig;
using Markdig.Parsers;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.ContextObjects
{
    public class ContextObjectsServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryGetContextObject_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act and assert
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => testSubject.TryGetContextObject(new object(), null, out object value));
        }

        [Fact]
        public void TryGetContextObject_ThrowsArgumentNullExceptionIfKeyIsNull()
        {
            // Arrange
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act and assert
            ArgumentNullException result = Assert.
                Throws<ArgumentNullException>(() => testSubject.TryGetContextObject(null, MarkdigTypesFactory.CreateBlockProcessor(), out object value));
        }

        [Fact]
        public void TryGetContextObject_ReturnsTrueAndContextObjectIfItExistsInBlockProcessorContext()
        {
            // Arrange
            var dummyKey = new object();
            var dummyValue = new object();
            var dummyMarkdownParserContext = new MarkdownParserContext();
            dummyMarkdownParserContext.Properties.Add(dummyKey, dummyValue);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor(markdownParserContext: dummyMarkdownParserContext);
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act
            bool result = testSubject.TryGetContextObject(dummyKey, dummyBlockProcessor, out object resultValue);

            // Assert
            Assert.True(result);
            Assert.Same(dummyValue, resultValue);
        }

        [Fact]
        public void TryGetContextObject_AddsContextObjectToBlockProcessorContextAndReturnsTrueAndContextObjectIfItExistsInContextObjectsStore()
        {
            // Arrange
            var dummyKey = new object();
            var dummyValue = new object();
            var dummyMarkdownParserContext = new MarkdownParserContext(); // Context with no context objects
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor(markdownParserContext: dummyMarkdownParserContext);
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetFromContextObjectsStore(dummyKey, dummyBlockProcessor, out dummyValue)).Returns(true);

            // Act
            bool result = mockTestSubject.Object.TryGetContextObject(dummyKey, dummyBlockProcessor, out object resultValue);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
            Assert.Same(dummyValue, resultValue);
            Assert.Single(dummyMarkdownParserContext.Properties);
            Assert.Equal(dummyValue, dummyMarkdownParserContext.Properties[dummyKey]);
        }

        [Fact]
        public void TryGetContextObject_ReturnsFalseAndNullIfContextObjectCannotBeRetrieved()
        {
            // Arrange
            var dummyKey = new object();
            object dummyValue = null;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetFromContextObjectsStore(dummyKey, dummyBlockProcessor, out dummyValue)).Returns(false);

            // Act
            bool result = mockTestSubject.Object.TryGetContextObject(dummyKey, dummyBlockProcessor, out object resultValue);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
            Assert.Null(resultValue);
        }

        [Fact]
        public void TryAddContextObject_ThrowsArgumentNullExceptionIfKeyIsNull()
        {
            // Arrange
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act and assert
            ArgumentNullException result = Assert.
                Throws<ArgumentNullException>(() => testSubject.TryAddContextObject(null, null, MarkdigTypesFactory.CreateBlockProcessor()));
        }

        [Fact]
        public void TryAddContextObject_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act and assert
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => testSubject.TryAddContextObject(new object(), null, null));
        }

        [Fact]
        public void TryAddContextObject_ReturnsTrueAndAddsContextObjectToMarkdownParserContextIfItsNotNull()
        {
            // Arrange
            var dummyKey = new object();
            var dummyValue = new object();
            var dummyMarkdownParserContext = new MarkdownParserContext();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor(markdownParserContext: dummyMarkdownParserContext);
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act
            bool result = testSubject.TryAddContextObject(dummyKey, dummyValue, dummyBlockProcessor);

            // Assert
            Assert.True(result);
            Assert.Single(dummyMarkdownParserContext.Properties);
            Assert.Same(dummyValue, dummyMarkdownParserContext.Properties[dummyKey]);
        }

        [Fact]
        public void TryAddContextObject_ReturnsTrueAndAddsContextObjectToContextObjectsStoreIfItsNotNullAndMarkdownParserContextIsNull()
        {
            // Arrange
            var dummyKey = new object();
            var dummyValue = new object();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyContextObjectsStore = new ContextObjectsStore();
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContextObjectsStore(dummyBlockProcessor)).Returns(dummyContextObjectsStore);

            // Act
            bool result = mockTestSubject.Object.TryAddContextObject(dummyKey, dummyValue, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
            Assert.Single(dummyContextObjectsStore);
            Assert.Same(dummyValue, dummyContextObjectsStore[dummyKey]);
        }

        [Fact]
        public void TryAddContextObject_ReturnsFalseIfUnableToSetContextObject()
        {
            // Arrange
            var dummyKey = new object();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContextObjectsStore(dummyBlockProcessor)).Returns((ContextObjectsStore)null);

            // Act
            bool result = mockTestSubject.Object.TryAddContextObject(dummyKey, null, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Fact]
        public void TryGetContextObjectsStore_ReturnsNullIfThereAreNoGlobalParsers()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor(blockParsers: new BlockParserList(new BlockParser[0]));
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act
            ContextObjectsStore result = testSubject.TryGetContextObjectsStore(dummyBlockProcessor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetContextObjectsStore_ReturnsNullIfNoGlobalParserIsAContextObjectsStore()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor(); // Several global parsers registered by default
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act
            ContextObjectsStore result = testSubject.TryGetContextObjectsStore(dummyBlockProcessor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetContextObjectsStore_ReturnsContextObjectsStoreIfItExists()
        {
            // Arrange
            var dummyContextObjectsStore = new ContextObjectsStore();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor(blockParsers: new BlockParserList(new BlockParser[] { dummyContextObjectsStore }));
            ContextObjectsService testSubject = CreateContextObjectsService();

            // Act
            ContextObjectsStore result = testSubject.TryGetContextObjectsStore(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyContextObjectsStore, result);
        }

        [Fact]
        public void TryGetFromContextObjectsStore_ReturnsFalseAndNullIfThereIsNoContextObjectsStore()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContextObjectsStore(dummyBlockProcessor)).Returns((ContextObjectsStore)null);

            // Act
            bool result = mockTestSubject.Object.TryGetFromContextObjectsStore(null, dummyBlockProcessor, out object resultValue);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
            Assert.Null(resultValue);
        }

        [Fact]
        public void TryGetFromContextObjectsStore_ReturnsFalseAndNullIfTheContextObjectsStoreDoesNotContainAContextObjectWithTheSpecifiedKey()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyContextObjectsStore = new ContextObjectsStore();
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContextObjectsStore(dummyBlockProcessor)).Returns(dummyContextObjectsStore);

            // Act
            bool result = mockTestSubject.Object.TryGetFromContextObjectsStore("dummyKey", dummyBlockProcessor, out object resultValue);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
            Assert.Null(resultValue);
        }

        [Fact]
        public void TryGetFromContextObjectsStore_ReturnsTrueAndTheContextObjectIfItExists()
        {
            // Arrange
            var dummyKey = new object();
            var dummyValue = new object();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyContextObjectsStore = new ContextObjectsStore();
            dummyContextObjectsStore.Add(dummyKey, dummyValue);
            Mock<ContextObjectsService> mockTestSubject = CreateMockContextObjectsService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetContextObjectsStore(dummyBlockProcessor)).Returns(dummyContextObjectsStore);

            // Act
            bool result = mockTestSubject.Object.TryGetFromContextObjectsStore(dummyKey, dummyBlockProcessor, out object resultValue);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result);
            Assert.Same(dummyValue, resultValue);
        }

        private ContextObjectsService CreateContextObjectsService()
        {
            return new ContextObjectsService();
        }

        private Mock<ContextObjectsService> CreateMockContextObjectsService()
        {
            return _mockRepository.Create<ContextObjectsService>();
        }
    }
}
