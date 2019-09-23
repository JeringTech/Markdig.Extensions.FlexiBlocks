using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class OptionsServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfBlockOptionsFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new OptionsService<IDummyBlockOptions, IDummyBlocksExtensionOptions>(
                null,
                _mockRepository.Create<IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfBlocksExtensionOptionsFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new OptionsService<IDummyBlockOptions, IDummyBlocksExtensionOptions>(
                _mockRepository.Create<IBlockOptionsFactory<IDummyBlockOptions>>().Object,
                null));
        }

        [Fact]
        public void CreateOptions_FromLeafBlock_CreatesOptions()
        {
            // Arrange
            Mock<LeafBlock> dummyLeafBlock = _mockRepository.Create<LeafBlock>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IDummyBlockOptions> dummyDefaultBlockOptions = _mockRepository.Create<IDummyBlockOptions>();
            Mock<IDummyBlockOptions> dummyNewBlockOptions = _mockRepository.Create<IDummyBlockOptions>();
            Mock<IDummyBlocksExtensionOptions> mockBlocksExtensionOptions = _mockRepository.Create<IDummyBlocksExtensionOptions>();
            mockBlocksExtensionOptions.Setup(f => f.DefaultBlockOptions).Returns(dummyDefaultBlockOptions.Object);
            Mock<IBlockOptionsFactory<IDummyBlockOptions>> mockBlockOptionsFactory = _mockRepository.Create<IBlockOptionsFactory<IDummyBlockOptions>>();
            mockBlockOptionsFactory.Setup(f => f.Create(dummyDefaultBlockOptions.Object, dummyLeafBlock.Object)).Returns(dummyNewBlockOptions.Object);
            Mock<IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions>> mockBlocksExtensionOptionsFactory =
                _mockRepository.Create<IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions>>();
            mockBlocksExtensionOptionsFactory.Setup(f => f.Create(dummyBlockProcessor)).Returns(mockBlocksExtensionOptions.Object);
            OptionsService<IDummyBlockOptions, IDummyBlocksExtensionOptions> testSubject = CreateOptionsService(mockBlockOptionsFactory.Object, mockBlocksExtensionOptionsFactory.Object);

            // Act
            (IDummyBlockOptions resultBlockOptions, IDummyBlocksExtensionOptions resultExtensionOptions) = testSubject.CreateOptions(dummyBlockProcessor, dummyLeafBlock.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyNewBlockOptions.Object, resultBlockOptions);
            Assert.Same(mockBlocksExtensionOptions.Object, resultExtensionOptions);
        }

        [Fact]
        public void CreateOptions_FromFlexiOptionsBlock_CreatesOptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IDummyBlockOptions> dummyDefaultBlockOptions = _mockRepository.Create<IDummyBlockOptions>();
            Mock<IDummyBlockOptions> dummyNewBlockOptions = _mockRepository.Create<IDummyBlockOptions>();
            Mock<IDummyBlocksExtensionOptions> mockBlocksExtensionOptions = _mockRepository.Create<IDummyBlocksExtensionOptions>();
            mockBlocksExtensionOptions.Setup(f => f.DefaultBlockOptions).Returns(dummyDefaultBlockOptions.Object);
            Mock<IBlockOptionsFactory<IDummyBlockOptions>> mockBlockOptionsFactory = _mockRepository.Create<IBlockOptionsFactory<IDummyBlockOptions>>();
            mockBlockOptionsFactory.Setup(f => f.Create(dummyDefaultBlockOptions.Object, dummyBlockProcessor)).Returns(dummyNewBlockOptions.Object);
            Mock<IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions>> mockBlocksExtensionOptionsFactory =
                _mockRepository.Create<IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions>>();
            mockBlocksExtensionOptionsFactory.Setup(f => f.Create(dummyBlockProcessor)).Returns(mockBlocksExtensionOptions.Object);
            OptionsService<IDummyBlockOptions, IDummyBlocksExtensionOptions> testSubject = CreateOptionsService(mockBlockOptionsFactory.Object, mockBlocksExtensionOptionsFactory.Object);

            // Act
            (IDummyBlockOptions resultBlockOptions, IDummyBlocksExtensionOptions resultExtensionOptions) = testSubject.CreateOptions(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyNewBlockOptions.Object, resultBlockOptions);
            Assert.Same(mockBlocksExtensionOptions.Object, resultExtensionOptions);
        }

        private OptionsService<IDummyBlockOptions, IDummyBlocksExtensionOptions> CreateOptionsService(
            IBlockOptionsFactory<IDummyBlockOptions> BlockOptionsFactory = null,
            IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions> BlocksExtensionOptionsFactory = null)
        {
            return new OptionsService<IDummyBlockOptions, IDummyBlocksExtensionOptions>(BlockOptionsFactory ?? _mockRepository.Create<IBlockOptionsFactory<IDummyBlockOptions>>().Object,
                    BlocksExtensionOptionsFactory ?? _mockRepository.Create<IExtensionOptionsFactory<IDummyBlocksExtensionOptions, IDummyBlockOptions>>().Object);
        }

        public interface IDummyBlocksExtensionOptions : IExtensionOptions<IDummyBlockOptions>
        {
        }

        public interface IDummyBlockOptions : IBlockOptions<IDummyBlockOptions>
        {
        }
    }
}
