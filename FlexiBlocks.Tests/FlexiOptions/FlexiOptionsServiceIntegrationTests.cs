using FlexiBlocks.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace FlexiBlocks.Tests.JsonOptions
{
    public class FlexiOptionsServiceIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryExtractOptions_ReturnsNullIfNoJsonOptionsBlockExists()
        {
            // Arrange
            const int dummyStartLine = 1;
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.CallBase = true;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns((FlexiOptionsBlock)null);

            // Act
            TestOptions result = mockJsonOptionsService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor, dummyStartLine);

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
            var dummyJsonOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLine,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyJsonOptionsBlock);

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() =>
                mockJsonOptionsService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor, dummyStartLine));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnableToParseJson, dummyJson, dummyLine, dummyColumn), result.Message);
            Assert.True(result.InnerException is JsonException);
        }

        [Fact]
        public void TryExtractOptions_ExtractsObjectFromJsonOptionsBlock()
        {
            // Arrange
            const string dummyValue1 = "value1";
            const int dummyStartLine = 1;
            const int dummyValue2 = 2;
            var dummyJsonOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup($"{{\"Option1\": \"{dummyValue1}\", \"Option2\": {dummyValue2}}}")
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyJsonOptionsBlock);

            // Act
            TestOptions result = mockJsonOptionsService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor, dummyStartLine);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyValue1, result.Option1);
            Assert.Equal(dummyValue2, result.Option2);
        }

        [Fact]
        public void TryPopulateOptions_ReturnsFalseIfNoJsonOptionsBlockExists()
        {
            // Arrange
            int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyLineIndex)).Returns((FlexiOptionsBlock)null);
            var dummyTestOptions = new TestOptions();

            // Act
            bool result = mockJsonOptionsService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyLineIndex);

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
            var dummyJsonOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLine,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyJsonOptionsBlock);
            var dummyTestOptions = new TestOptions();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockJsonOptionsService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyStartLine));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnableToParseJson, dummyJson, dummyLine, dummyColumn), result.Message);
            Assert.True(result.InnerException is JsonException);
        }

        [Fact]
        public void TryPopulateOptions_PopulatesObjectFromJsonOptionsBlock()
        {
            // Arrange
            const string dummyValue1 = "value1";
            const int dummyValue2 = 2;
            const int dummyStartLine = 1;
            var dummyJsonOptionsBlock = new FlexiOptionsBlock(null)
            {
                Lines = new StringLineGroup($"{{\"Option1\": \"{dummyValue1}\"}}")
            };
            var dummyTestOptions = new TestOptions()
            {
                Option2 = dummyValue2
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyStartLine)).Returns(dummyJsonOptionsBlock);

            // Act
            mockJsonOptionsService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions, dummyStartLine);

            // Assert
            // Properties specified in JSON get overwritten, other properties are left as is
            Assert.Equal(dummyValue1, dummyTestOptions.Option1);
            Assert.Equal(dummyValue2, dummyTestOptions.Option2);
        }

        [Fact]
        public void TryGetJsonOptionsBlock_ReturnsNullIfAJsonOptionsBlockDoesNotExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var jsonOptionsService = new FlexiOptionsService();

            // Act
            FlexiOptionsBlock result = jsonOptionsService.TryGetJsonOptionsBlock(dummyBlockProcessor, 0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetJsonOptionsBlock_ThrowsExceptionIfJsonOptionsBlockDoesNotImmediatelyPrecedeCurrentLine()
        {
            // Arrange
            const string dummyJson = "dummyJson";
            const int dummyLine = 0;
            const int dummyOptionsEndLine = 1;
            const int dummyColumn = 2;
            var dummyJsonOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLine,
                EndLine = dummyOptionsEndLine,
                Column = dummyColumn,
                Lines = new StringLineGroup(dummyJson)
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.FLEXI_OPTIONS, dummyJsonOptionsBlock);
            var jsonOptionsService = new FlexiOptionsService();

            // Act and Assert
            InvalidOperationException result = Assert.
                Throws<InvalidOperationException>(() => 
                    jsonOptionsService.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyOptionsEndLine + 2));  // 1 line gap between options block and current line
            Assert.Equal(string.Format(Strings.InvalidOperationException_JsonOptionsDoesNotImmediatelyPrecedeConsumingBlock,
                dummyJson,
                dummyLine,
                dummyColumn), 
                result.Message);
        }

        [Fact]
        public void TryGetJsonOptionsBlock_IfSuccessfulReturnsJsonOptionsBlockAndRemovesItFromDocumentData()
        {
            // Arrange
            const int dummyOptionsEndLine = 1;
            var dummyJsonOptionsBlock = new FlexiOptionsBlock(null)
            {
                EndLine = dummyOptionsEndLine
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockParser.FLEXI_OPTIONS, dummyJsonOptionsBlock);
            var jsonOptionsService = new FlexiOptionsService();

            // Act
            FlexiOptionsBlock result = jsonOptionsService.TryGetJsonOptionsBlock(dummyBlockProcessor, dummyOptionsEndLine + 1); // 1 line gap between options block and current line

            // Assert
            Assert.Same(dummyJsonOptionsBlock, result);
            Assert.Null(dummyBlockProcessor.Document.GetData(FlexiOptionsBlockParser.FLEXI_OPTIONS));
        }

        private class TestOptions
        {
            public string Option1 { get; set; }
            public int Option2 { get; set; }
        }
    }
}
