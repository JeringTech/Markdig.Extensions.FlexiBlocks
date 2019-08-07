using Jering.IocServices.Newtonsoft.Json;
using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class BlockOptionsFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfJsonSerializerServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new BlockOptionsFactory<IDummyOptions>(null));
        }

        [Fact]
        public void Create_FromOptionsBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(_mockRepository.Create<IDummyOptions>().Object, (BlockProcessor)null));
        }

        [Fact]
        public void Create_FromOptionsBlock_ReturnsDefaultBlockOptionsIfThereIsNoOptionsBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IDummyOptions> dummyOptions = _mockRepository.Create<IDummyOptions>();
            Mock<BlockOptionsFactory<IDummyOptions>> mockTestSubject = CreateMockBlockOptionsFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetOptionsBlock(dummyBlockProcessor)).Returns((OptionsBlock)null);

            // Act
            IDummyOptions result = mockTestSubject.Object.Create(dummyOptions.Object, dummyBlockProcessor);

            // Assert
            Assert.Same(dummyOptions.Object, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_FromOptionsBlock_CreatesOptions()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IDummyOptions> dummyClonedDummyOptions = _mockRepository.Create<IDummyOptions>();
            Mock<IDummyOptions> dummyOptions = _mockRepository.Create<IDummyOptions>();
            var dummyOptionsBlock = new OptionsBlock(null);
            Mock<BlockOptionsFactory<IDummyOptions>> mockTestSubject = CreateMockBlockOptionsFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.TryGetOptionsBlock(dummyBlockProcessor)).Returns(dummyOptionsBlock);
            mockTestSubject.Setup(t => t.Create(dummyOptions.Object, dummyOptionsBlock)).Returns(dummyClonedDummyOptions.Object);

            // Act
            IDummyOptions result = mockTestSubject.Object.Create(dummyOptions.Object, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyClonedDummyOptions.Object, result);
        }

        [Fact]
        public void Create_FromLeafBlock_ThrowsArgumentNullExceptionIfDefaultBlockOptionsIsNull()
        {
            // Arrange
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(null, _mockRepository.Create<LeafBlock>(null).Object));
        }

        [Fact]
        public void Create_FromLeafBlock_ThrowsArgumentNullExceptionIfLeafBlockIsNull()
        {
            // Arrange
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(_mockRepository.Create<IDummyOptions>().Object, (LeafBlock)null));
        }

        [Fact]
        public void Create_FromLeafBlock_ThrowsBlockExceptionIfJsonCannotBeDeserialized()
        {
            // Arrange
            const string dummyJson = "@dummyJson";
            const int dummyColumn = 2;
            const int dummyLineIndex = 4;
            Mock<LeafBlock> dummyLeafBlock = _mockRepository.Create<LeafBlock>(null);
            dummyLeafBlock.Object.Lines = new StringLineGroup(dummyJson);
            dummyLeafBlock.Object.Line = dummyLineIndex;
            dummyLeafBlock.Object.Column = dummyColumn;
            var dummyJsonException = new JsonException();
            Mock<IDummyOptions> dummyClonedBlockOptions = _mockRepository.Create<IDummyOptions>();
            Mock<IDummyOptions> mockDefaultBlockOptions = _mockRepository.Create<IDummyOptions>();
            mockDefaultBlockOptions.Setup(d => d.Clone()).Returns(dummyClonedBlockOptions.Object);
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Populate(It.IsAny<JsonTextReader>(), dummyClonedBlockOptions.Object)).Throws(dummyJsonException);
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory(mockJsonSerializerService.Object);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => testSubject.Create(mockDefaultBlockOptions.Object, dummyLeafBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    dummyLeafBlock.Object.GetType().Name,
                    dummyLineIndex + 1,
                    dummyColumn,
                    string.Format(Strings.OptionsException_BlockOptionsFactory_InvalidJson, dummyJson)),
                result.Message);
            Assert.Same(dummyJsonException, result.InnerException);
        }

        [Fact]
        public void Create_FromLeafBlock_CreatesOptions()
        {
            // Arrange
            Mock<IDummyOptions> dummyClonedBlockOptions = _mockRepository.Create<IDummyOptions>();
            Mock<IDummyOptions> mockDummyOptions = _mockRepository.Create<IDummyOptions>();
            mockDummyOptions.Setup(d => d.Clone()).Returns(dummyClonedBlockOptions.Object);
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Populate(It.IsAny<JsonTextReader>(), dummyClonedBlockOptions.Object));
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory(mockJsonSerializerService.Object);

            // Act
            IDummyOptions result = testSubject.Create(mockDummyOptions.Object, _mockRepository.Create<LeafBlock>(null).Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyClonedBlockOptions.Object, result);
        }

        [Fact]
        public void TryGetOptionsBlock_ReturnsNullIfAnOptionsBlockDoesNotExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory();

            // Act
            OptionsBlock result = testSubject.TryGetOptionsBlock(dummyBlockProcessor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetOptionsBlock_IfSuccessfulReturnsOptionsBlockAndRemovesItFromDocumentData()
        {
            // Arrange
            var dummyOptionsBlock = new OptionsBlock(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(OptionsBlockFactory.PENDING_OPTIONS_BLOCK, dummyOptionsBlock);
            BlockOptionsFactory<IDummyOptions> testSubject = CreateBlockOptionsFactory();

            // Act
            OptionsBlock result = testSubject.TryGetOptionsBlock(dummyBlockProcessor);

            // Assert
            Assert.Same(dummyOptionsBlock, result);
            Assert.Null(dummyBlockProcessor.Document.GetData(OptionsBlockFactory.PENDING_OPTIONS_BLOCK));
        }

        public BlockOptionsFactory<IDummyOptions> CreateBlockOptionsFactory(IJsonSerializerService jsonSerializerService = null)
        {
            return new BlockOptionsFactory<IDummyOptions>(jsonSerializerService ?? _mockRepository.Create<IJsonSerializerService>().Object);
        }

        public Mock<BlockOptionsFactory<IDummyOptions>> CreateMockBlockOptionsFactory(IJsonSerializerService jsonSerializerService = null)
        {
            return _mockRepository.Create<BlockOptionsFactory<IDummyOptions>>(jsonSerializerService ?? _mockRepository.Create<IJsonSerializerService>().Object);
        }

        public interface IDummyOptions : IBlockOptions<IDummyOptions>
        {
        }
    }
}
