using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockServiceIntegrationTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryExtractOptions_ReturnsNullIfNoFlexiOptionsBlockExists()
        {
            // Arrange
            const int dummyStartLine = 1;
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.CallBase = true;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns((FlexiOptionsBlock)null);

            // Act
            TestOptions result = mockFlexiOptionsBlockService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor, dummyStartLine);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Null(result);
        }

        [Fact]
        public void TryExtractOptions_ThrowsExceptionIfJsonParsingFails()
        {
            // Arrange
            const string dummyJson = "{invalid json}";
            const int dummyLine = 1;
            const int dummyStartLine = 1;
            const int dummyColumn = 2;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLine,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.CallBase = true;
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyFlexiOptionsBlock);

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() =>
                mockFlexiOptionsBlockService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor, dummyStartLine));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnableToParseJson, dummyJson, dummyLine, dummyColumn), result.Message);
            Assert.True(result.InnerException is JsonException);
        }

        [Fact]
        public void TryExtractOptions_ExtractsObjectFromFlexiOptionsBlock()
        {
            // Arrange
            const string dummyValue1 = "value1";
            const int dummyStartLine = 1;
            const int dummyValue2 = 2;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup($"{{\"Option1\": \"{dummyValue1}\", \"Option2\": {dummyValue2}}}")
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.CallBase = true;
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyFlexiOptionsBlock);

            // Act
            TestOptions result = mockFlexiOptionsBlockService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor, dummyStartLine);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyValue1, result.Option1);
            Assert.Equal(dummyValue2, result.Option2);
        }

        [Fact]
        public void TryPopulateOptions_ReturnsFalseIfNoFlexiOptionsBlockExists()
        {
            // Arrange
            int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.CallBase = true;
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyLineIndex)).Returns((FlexiOptionsBlock)null);
            var dummyTestOptions = new TestOptions();

            // Act
            bool result = mockFlexiOptionsBlockService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyLineIndex);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Fact]
        public void TryPopulateOptions_ThrowsExceptionIfJsonParsingFails()
        {
            // Arrange
            const string dummyJson = "{invalid json}";
            const int dummyLine = 1;
            const int dummyStartLine = 1;
            const int dummyColumn = 2;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLine,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.CallBase = true;
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyFlexiOptionsBlock);
            var dummyTestOptions = new TestOptions();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockFlexiOptionsBlockService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyStartLine));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnableToParseJson, dummyJson, dummyLine, dummyColumn), result.Message);
            Assert.True(result.InnerException is JsonException);
        }

        [Fact]
        public void TryPopulateOptions_PopulatesObjectFromFlexiOptionsBlock()
        {
            // Arrange
            const string dummyValue1 = "value1";
            const int dummyValue2 = 2;
            const int dummyStartLine = 1;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup($"{{\"Option1\": \"{dummyValue1}\"}}")
            };
            var dummyTestOptions = new TestOptions()
            {
                Option2 = dummyValue2
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<FlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.CallBase = true;
            mockFlexiOptionsBlockService.Setup(j => j.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyFlexiOptionsBlock);

            // Act
            mockFlexiOptionsBlockService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyStartLine);

            // Assert
            // Properties specified in JSON get overwritten, other properties are left as is
            Assert.Equal(dummyValue1, dummyTestOptions.Option1);
            Assert.Equal(dummyValue2, dummyTestOptions.Option2);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_ReturnsNullIfAFlexiOptionsBlockDoesNotExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var flexiOptionsBlockService = new FlexiOptionsBlockService();

            // Act
            FlexiOptionsBlock result = flexiOptionsBlockService.TryGetFlexiOptionsBlock(dummyBlockProcessor, 0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_ThrowsExceptionIfFlexiOptionsBlockDoesNotImmediatelyPrecedeCurrentLine()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            var dummyStringLineGroup = new StringLineGroup(dummyJson);
            const int dummyLine = 0;
            const int dummyColumn = 2;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLine,
                Column = dummyColumn,
                Lines = dummyStringLineGroup
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK, dummyFlexiOptionsBlock);
            var flexiOptionsBlockService = new FlexiOptionsBlockService();

            // Act and Assert
            InvalidOperationException result = Assert.
                Throws<InvalidOperationException>(() =>
                    flexiOptionsBlockService.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyLine + dummyStringLineGroup.Count + 1));  // 1 line gap between options block and current line
            Assert.Equal(string.Format(Strings.InvalidOperationException_FlexiOptionsBlockDoesNotImmediatelyPrecedeConsumingBlock,
                dummyJson,
                dummyLine,
                dummyColumn),
                result.Message);
        }

        [Fact]
        public void TryGetFlexiOptionsBlock_IfSuccessfulReturnsFlexiOptionsBlockAndRemovesItFromDocumentData()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            var dummyStringLineGroup = new StringLineGroup(dummyJson);
            const int dummyLine = 0;
            var dummyFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLine,
                Lines = dummyStringLineGroup
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK, dummyFlexiOptionsBlock);
            var flexiOptionsBlockService = new FlexiOptionsBlockService();

            // Act
            FlexiOptionsBlock result = flexiOptionsBlockService.TryGetFlexiOptionsBlock(dummyBlockProcessor, dummyLine + dummyStringLineGroup.Count); // no gap between options block and current line

            // Assert
            Assert.Same(dummyFlexiOptionsBlock, result);
            Assert.Null(dummyBlockProcessor.Document.GetData(FlexiOptionsBlockParser.FLEXI_OPTIONS_BLOCK));
        }

        private class TestOptions
        {
            public string Option1 { get; set; }
            public int Option2 { get; set; }
        }
    }
}
