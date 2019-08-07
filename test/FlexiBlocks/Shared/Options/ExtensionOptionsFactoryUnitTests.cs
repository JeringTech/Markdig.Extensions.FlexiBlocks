using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Markdig.Parsers;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class ExtensionOptionsFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfContextObjectServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExtensionOptionsFactory<IDummyExtensionOptions, DummyExtensionOptions, IDummyBlockOptions>(null));
        }

        [Fact]
        public void Create_ReturnsIExtensionOptionsFromContextObjectsIfItExists()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            Mock<IDummyExtensionOptions> dummyExtensionOptions = _mockRepository.Create<IDummyExtensionOptions>();
            object dummyValue = dummyExtensionOptions.Object;
            mockContextObjectsService.Setup(c => c.TryGetContextObject(typeof(IDummyExtensionOptions), dummyBlockProcessor, out dummyValue)).Returns(true);
            ExtensionOptionsFactory<IDummyExtensionOptions, DummyExtensionOptions, IDummyBlockOptions> testSubject =
                CreateExtensionOptionsFactory(mockContextObjectsService.Object);

            // Act
            IDummyExtensionOptions result = testSubject.Create(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyValue, result);
        }

        [Fact]
        public void Create_ReturnsNewExtensionOptionsIfContextObjectIsNotAnIExtensionOptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyValue = new object();
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            mockContextObjectsService.Setup(c => c.TryGetContextObject(typeof(IDummyExtensionOptions), dummyBlockProcessor, out dummyValue)).Returns(true);
            ExtensionOptionsFactory<IDummyExtensionOptions, DummyExtensionOptions, IDummyBlockOptions> testSubject =
                CreateExtensionOptionsFactory(mockContextObjectsService.Object);

            // Act
            IDummyExtensionOptions result = testSubject.Create(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.IsType<DummyExtensionOptions>(result);
        }

        [Fact]
        public void Create_ReturnsNewExtensionOptionsIfThereIsNoContextObjectForTheGivenKey()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            object dummyValue = null;
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            mockContextObjectsService.Setup(c => c.TryGetContextObject(typeof(IDummyExtensionOptions), dummyBlockProcessor, out dummyValue)).Returns(false);
            ExtensionOptionsFactory<IDummyExtensionOptions, DummyExtensionOptions, IDummyBlockOptions> testSubject =
                CreateExtensionOptionsFactory(mockContextObjectsService.Object);

            // Act
            IDummyExtensionOptions result = testSubject.Create(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.IsType<DummyExtensionOptions>(result);
        }

        public class DummyExtensionOptions : IDummyExtensionOptions
        {
            public IDummyBlockOptions DefaultBlockOptions { get; set; }
        }

        public interface IDummyExtensionOptions : IExtensionOptions<IDummyBlockOptions>
        {
        }

        public interface IDummyBlockOptions : IBlockOptions<IDummyBlockOptions>
        {
        }

        private ExtensionOptionsFactory<IDummyExtensionOptions, DummyExtensionOptions, IDummyBlockOptions> CreateExtensionOptionsFactory(IContextObjectsService contextObjectsService = null)
        {
            return new ExtensionOptionsFactory<IDummyExtensionOptions, DummyExtensionOptions, IDummyBlockOptions>(contextObjectsService ?? _mockRepository.Create<IContextObjectsService>().Object);
        }
    }
}
