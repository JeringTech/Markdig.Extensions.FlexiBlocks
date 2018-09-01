using Jering.IocServices.Newtonsoft.Json;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryExtractOptions_ReturnsNullIfNoFlexiOptionsBlockExists()
        {
            // Arrange
            const int dummyStartLine = 1;
            Mock<FlexiOptionsBlockService> mockTestSubject = CreateMockFlexiOptionsBlockService();
            mockTestSubject.CallBase = true;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            mockTestSubject.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns((FlexiOptionsBlock)null);

            // Act
            DummyOptions result = mockTestSubject.Object.TryExtractOptions<DummyOptions>(dummyBlockProcessor, dummyStartLine);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Null(result);
        }

        [Fact]
        public void TryExtractOptions_ThrowsFlexiBlocksExceptionIfJsonCannotBeDeserialized()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            const int dummyLineIndex = 1;
            const int dummyStartLineNumber = dummyLineIndex + 1;
            const int dummyColumn = 2;
            var dummyJsonException = new JsonException();
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Deserialize<DummyOptions>(It.IsAny<JsonTextReader>())).Throws(dummyJsonException);
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLineIndex,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockTestSubject = CreateMockFlexiOptionsBlockService(mockJsonSerializerService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLineNumber)).Returns(dummyFlexiOptionsBlock);

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryExtractOptions<DummyOptions>(dummyBlockProcessor, dummyStartLineNumber));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    typeof(FlexiOptionsBlock).Name,
                    dummyStartLineNumber,
                    dummyColumn,
                    string.Format(Strings.FlexiBlocksException_UnableToParseJson, dummyJson)), 
                result.Message);
            Assert.Same(dummyJsonException, result.InnerException);
        }

        [Fact]
        public void TryPopulateOptions_ThrowsArgumentNullExceptionIfTargetIsNull()
        {
            // Arrange
            FlexiOptionsBlockService testSubject = CreateFlexiOptionsBlockService();

            // Act and assert
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => testSubject.TryPopulateOptions<DummyOptions>(null, null, 0));
            Assert.Equal("Value cannot be null.\nParameter name: target", result.Message, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void TryPopulateOptions_ReturnsFalseIfNoFlexiOptionsBlockExists()
        {
            // Arrange
            const int dummyStartLineNumber = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = CreateMockFlexiOptionsBlockService();
            mockFlexiOptionsBlockService.CallBase = true;
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLineNumber)).Returns((FlexiOptionsBlock)null);
            var dummyTestOptions = new DummyOptions();

            // Act
            bool result = mockFlexiOptionsBlockService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyStartLineNumber);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Fact]
        public void TryPopulateOptions_ThrowsFlexiBlocksExceptionIfJsonCannotBeDeserialized()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            const int dummyLineIndex = 1;
            const int dummyStartLineNumber = dummyLineIndex + 1;
            const int dummyColumn = 2;
            var dummyJsonException = new JsonException();
            var dummyOptions = new DummyOptions();
            Mock<IJsonSerializerService> mockJsonSerializerService = _mockRepository.Create<IJsonSerializerService>();
            mockJsonSerializerService.Setup(j => j.Populate(It.IsAny<JsonTextReader>(), dummyOptions)).Throws(dummyJsonException);
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLineIndex,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockTestSubject = CreateMockFlexiOptionsBlockService(mockJsonSerializerService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLineNumber)).Returns(dummyFlexiOptionsBlock);

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => mockTestSubject.Object.TryPopulateOptions(dummyBlockProcessor, dummyOptions, dummyStartLineNumber));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    typeof(FlexiOptionsBlock).Name,
                    dummyStartLineNumber,
                    dummyColumn,
                    string.Format(Strings.FlexiBlocksException_UnableToParseJson, dummyJson)),
                result.Message);
            Assert.Same(dummyJsonException, result.InnerException);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_ThrowsArgumentNullExceptionIfProcessorIsNull()
        {
            // Arrange
            FlexiOptionsBlockService testSubject = CreateFlexiOptionsBlockService();

            // Act and assert
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => testSubject.TryGetFlexiOptionsBlock(null, 0));
            Assert.Equal("Value cannot be null.\nParameter name: processor", result.Message, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_ReturnsNullIfAFlexiOptionsBlockDoesNotExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiOptionsBlockService testSubject = CreateFlexiOptionsBlockService();

            // Act
            FlexiOptionsBlock result = testSubject.TryGetFlexiOptionsBlock(dummyBlockProcessor, 0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_ThrowsFlexiBlocksExceptionIfFlexiOptionsBlockDoesNotImmediatelyPrecedeCurrentLine()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            var dummyStringLineGroup = new StringLineGroup(dummyJson);
            const int dummyLineIndex = 0;
            const int dummyColumn = 2;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLineIndex,
                Column = dummyColumn,
                Lines = dummyStringLineGroup
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK, dummyFlexiOptionsBlock);
            FlexiOptionsBlockService testSubject = CreateFlexiOptionsBlockService();

            // Act and Assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => testSubject.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyLineIndex + dummyStringLineGroup.Count + 1));  // 1 line gap between options block and current line
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFlexiBlock,
                    typeof(FlexiOptionsBlock).Name,
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.FlexiBlocksException_MispositionedFlexiOptionsBlock),
                result.Message);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_IfSuccessfulReturnsFlexiOptionsBlockAndRemovesItFromDocumentData()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            var dummyStringLineGroup = new StringLineGroup(dummyJson);
            const int dummyLineIndex = 1;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLineIndex,
                Lines = dummyStringLineGroup
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK, dummyFlexiOptionsBlock);
            FlexiOptionsBlockService testSubject = CreateFlexiOptionsBlockService();

            // Act
            FlexiOptionsBlock result = testSubject.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyLineIndex + dummyStringLineGroup.Count); // no gap between options block and current line

            // Assert
            Assert.Same(dummyFlexiOptionsBlock, result);
            Assert.Null(dummyBlockProcessor.Document.GetData(FlexiOptionsBlockParser.PENDING_FLEXI_OPTIONS_BLOCK));
        }

        public FlexiOptionsBlockService CreateFlexiOptionsBlockService(IJsonSerializerService jsonSerializerService = null)
        {
            return new FlexiOptionsBlockService(jsonSerializerService ?? new JsonSerializerService());
        }

        public Mock<FlexiOptionsBlockService> CreateMockFlexiOptionsBlockService(IJsonSerializerService jsonSerializerService = null)
        {
            return _mockRepository.Create<FlexiOptionsBlockService>(jsonSerializerService ?? new JsonSerializerService());
        }

        private class DummyOptions
        {
        }
    }
}
