using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.JsonOptions
{
    public class JsonOptionsServiceIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryExtractOptions_ReturnsNullIfNoJsonOptionsBlockExists()
        {
            // Arrange
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.CallBase = true;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor)).Returns((JsonOptionsBlock)null);

            // Act
            TestOptions result = mockJsonOptionsService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Null(result);
        }

        [Fact]
        public void TryExtractOptions_ThrowsExceptionIfJsonParsingFails()
        {
            // Arrange
            string dummyJson = "{invalid json}";
            int dummyLine = 1;
            int dummyColumn = 2;
            JsonOptionsBlock dummyJsonOptionsBlock = new JsonOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLine,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor)).Returns(dummyJsonOptionsBlock);

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockJsonOptionsService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnableToParseJson, dummyJson, dummyLine, dummyColumn), result.Message);
            Assert.True(result.InnerException is JsonException);
        }

        [Fact]
        public void TryExtractOptions_ExtractsObjectFromJsonOptionsBlock()
        {
            // Arrange
            string dummyValue1 = "value1";
            int dummyValue2 = 2;
            JsonOptionsBlock dummyJsonOptionsBlock = new JsonOptionsBlock(null)
            {
                Lines = new StringLineGroup($"{{\"Option1\": \"{dummyValue1}\", \"Option2\": {dummyValue2}}}")
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor)).Returns(dummyJsonOptionsBlock);

            // Act
            TestOptions result = mockJsonOptionsService.Object.TryExtractOptions<TestOptions>(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyValue1, result.Option1);
            Assert.Equal(dummyValue2, result.Option2);
        }

        [Fact]
        public void TryPopulateOptions_ReturnsFalseIfNoJsonOptionsBlockExists()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor)).Returns((JsonOptionsBlock)null);
            TestOptions dummyTestOptions = new TestOptions();

            // Act
            bool result = mockJsonOptionsService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions);

            // Assert
            _mockRepository.VerifyAll();
            Assert.False(result);
        }

        [Fact]
        public void TryPopulateOptions_ThrowsExceptionIfJsonParsingFails()
        {
            // Arrange
            string dummyJson = "{invalid json}";
            int dummyLine = 1;
            int dummyColumn = 2;
            JsonOptionsBlock dummyJsonOptionsBlock = new JsonOptionsBlock(null)
            {
                Lines = new StringLineGroup(dummyJson),
                Line = dummyLine,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor)).Returns(dummyJsonOptionsBlock);
            TestOptions dummyTestOptions = new TestOptions();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockJsonOptionsService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_UnableToParseJson, dummyJson, dummyLine, dummyColumn), result.Message);
            Assert.True(result.InnerException is JsonException);
        }

        [Fact]
        public void TryPopulateOptions_PopulatesObjectFromJsonOptionsBlock()
        {
            // Arrange
            string dummyValue1 = "value1";
            int dummyValue2 = 2;
            JsonOptionsBlock dummyJsonOptionsBlock = new JsonOptionsBlock(null)
            {
                Lines = new StringLineGroup($"{{\"Option1\": \"{dummyValue1}\"}}")
            };
            TestOptions dummyTestOptions = new TestOptions()
            {
                Option2 = dummyValue2
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.CallBase = true;
            mockJsonOptionsService.Setup(j => j.TryGetJsonOptionsBlock(dummyBlockProcessor)).Returns(dummyJsonOptionsBlock);

            // Act
            mockJsonOptionsService.Object.TryPopulateOptions(dummyBlockProcessor, dummyTestOptions);

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
            JsonOptionsService jsonOptionsService = new JsonOptionsService();

            // Act
            JsonOptionsBlock result = jsonOptionsService.TryGetJsonOptionsBlock(dummyBlockProcessor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetJsonOptionsBlock_ThrowsExceptionIfJsonOptionsBlockDoesNotImmediatelyPrecedeCurrentLine()
        {
            // Arrange
            string dummyJson = "dummyJson";
            int dummyLine = 0;
            int dummyEndLine = 1;
            int dummyColumn = 2;
            JsonOptionsBlock dummyJsonOptionsBlock = new JsonOptionsBlock(null)
            {
                Line = dummyLine,
                EndLine = dummyEndLine,
                Column = dummyColumn,
                Lines = new StringLineGroup(dummyJson)
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(JsonOptionsParser.JSON_OPTIONS, dummyJsonOptionsBlock);
            dummyBlockProcessor.LineIndex = dummyEndLine + 2; // 1 line gap between options block and current line
            JsonOptionsService jsonOptionsService = new JsonOptionsService();

            // Act and Assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => jsonOptionsService.TryGetJsonOptionsBlock(dummyBlockProcessor));
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
            int dummyEndLine = 1;
            JsonOptionsBlock dummyJsonOptionsBlock = new JsonOptionsBlock(null)
            {
                EndLine = dummyEndLine
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(JsonOptionsParser.JSON_OPTIONS, dummyJsonOptionsBlock);
            dummyBlockProcessor.LineIndex = dummyEndLine + 1; // 1 line gap between options block and current line
            JsonOptionsService jsonOptionsService = new JsonOptionsService();

            // Act
            JsonOptionsBlock result = jsonOptionsService.TryGetJsonOptionsBlock(dummyBlockProcessor);

            // Assert
            Assert.Same(dummyJsonOptionsBlock, result);
            Assert.Null(dummyBlockProcessor.Document.GetData(JsonOptionsParser.JSON_OPTIONS));
        }

        private class TestOptions
        {
            public string Option1 { get; set; }
            public int Option2 { get; set; }
        }
    }
}
