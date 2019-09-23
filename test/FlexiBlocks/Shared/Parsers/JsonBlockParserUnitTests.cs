using System;
using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class JsonBlockUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfJsonBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExposedJsonBlockParser(null));
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfInCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // These three lines set IsCodeIndent to true
            dummyBlockProcessor.Column = 0;
            dummyBlockProcessor.RestartIndent();
            dummyBlockProcessor.Column = 4;
            ExposedJsonBlockParser testSubject = CreateExposedJsonBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.True(dummyBlockProcessor.IsCodeIndent);
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data))]
        public void TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters(string line)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(line);
            ExposedJsonBlockParser testSubject = CreateExposedJsonBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        public static IEnumerable<object[]> TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotBeginWithExpectedCharacters_Data()
        {
            return new object[][]
            {
                // Character after @ must be an opening brace
                new string[]{"@a"},
                // No whitespace between @ and {
                new string[]{"@ {"},
                new string[]{"@\n{"}
            };
        }

        [Fact]
        public void TryOpenBlock_IfSuccessfulCreatesNewProxyJsonBlockAndReturnsBlockStateContinue()
        {
            // Arrange
            const BlockState expectedResult = BlockState.Continue;
            var dummyLine = new StringSlice("o{dummy");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            Mock<IJsonBlockFactory<Block, ProxyJsonBlock>> mockJsonBlockFactory = _mockRepository.Create<IJsonBlockFactory<Block, ProxyJsonBlock>>();
            dummyLine.NextChar(); // TryOpenProxy advances line by 1 char (skips @) before calling ParseLine
            Mock<ExposedJsonBlockParser> mockTestSubject = CreateMockExposedJsonBlockParser(mockJsonBlockFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ParseLine(dummyLine, dummyProxyJsonBlock)).Returns(expectedResult);
            mockJsonBlockFactory.Setup(b => b.CreateProxyJsonBlock(dummyBlockProcessor, mockTestSubject.Object)).Returns(dummyProxyJsonBlock);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedResult, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            Assert.Same(dummyProxyJsonBlock, dummyBlockProcessor.NewBlocks.Peek());
        }

        [Fact]
        public void TryContinueBlock_AttemptsToContinueProxyBlock()
        {
            // Arrange
            const BlockState expectedResult = BlockState.Continue;
            var dummyLine = new StringSlice("o{dummy");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyLine;
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            Mock<ExposedJsonBlockParser> mockTestSubject = CreateMockExposedJsonBlockParser();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ParseLine(dummyLine, dummyProxyJsonBlock)).Returns(expectedResult);

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryContinueBlock(dummyBlockProcessor, dummyProxyJsonBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CloseProxy_ThrowsJsonExceptionIfJsonIsIncomplete()
        {
            // Arrange
            const string dummyLines = "dummyLines";
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            dummyProxyJsonBlock.Lines = new StringLineGroup(dummyLines);
            dummyProxyJsonBlock.NumOpenObjects = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            ExposedJsonBlockParser testSubject = CreateExposedJsonBlockParser();

            // Act and assert
            JsonException result = Assert.Throws<JsonException>(() => testSubject.ExposedCloseProxy(dummyBlockProcessor, dummyProxyJsonBlock));
            Assert.Equal(string.Format(Strings.JsonException_Shared_InvalidJson, dummyLines), result.Message);
        }

        [Fact]
        public void CloseProxy_ReturnsJsonBlockIfSuccessful()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            dummyProxyJsonBlock.NumOpenObjects = 0;
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<IJsonBlockFactory<Block, ProxyJsonBlock>> mockJsonBlockFactory = _mockRepository.Create<IJsonBlockFactory<Block, ProxyJsonBlock>>();
            mockJsonBlockFactory.Setup(j => j.Create(dummyProxyJsonBlock, dummyBlockProcessor)).Returns(dummyBlock.Object);
            ExposedJsonBlockParser testSubject = CreateExposedJsonBlockParser(mockJsonBlockFactory.Object);

            // Act
            Block result = testSubject.ExposedCloseProxy(dummyBlockProcessor, dummyProxyJsonBlock);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlock.Object, result);
        }

        [Theory]
        [MemberData(nameof(ParseLine_ParsesTheLineUpdatesBlockSpanEndAndReturnsBlockState_Data))]
        public void ParseLine_ParsesTheLineUpdatesBlockSpanEndAndReturnsBlockState(string dummyJson)
        {
            // Arrange
            ExposedJsonBlockParser testSubject = CreateExposedJsonBlockParser();
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            var lineReader = new LineReader(dummyJson);

            // Act and assert
            while (true)
            {
                BlockState result = testSubject.ParseLine(lineReader.ReadLine().Value, dummyProxyJsonBlock);

                if (result == BlockState.Break)
                {
                    Assert.Null(lineReader.ReadLine()); // If result is break we must be at the last line
                    break;
                }
                else
                {
                    Assert.Equal(BlockState.Continue, result);
                }
            }
            Assert.Equal(dummyJson.Length - 1, dummyProxyJsonBlock.Span.End);
        }

        public static IEnumerable<object[]> ParseLine_ParsesTheLineUpdatesBlockSpanEndAndReturnsBlockState_Data()
        {
            return new object[][]
            {
                        // Multi-line JSON
                        new object[]
                        {
                            @"{
            ""property1"": ""value1"",
            ""property2"": ""value2""
        }"
                        },
                        // Single-line JSON
                        new object[]
                        {
                            @"{""property1"": ""value1"", ""property2"": ""value2""}"
                        },
                        // Braces in strings
                        new object[]
                        {
                            @"{
            ""}property1"": ""}value1"",
            ""{property2"": ""{value2""
        }"
                        },
                        // Nested objects
                        new object[]
                        {
                            @"{
            ""property1"": ""value1"",
            ""property2"": {
                ""property3"": ""value3""
            }
        }"
                        },
                        // Escaped quotes in strings
                        new object[]
                        {
                            @"{
            ""prop\""erty1"": ""value1\"""",
            ""\""property2"": ""val\""ue2""
        }"
                        },
            };
        }

        public class ExposedJsonBlockParser : JsonBlockParser<Block, ProxyJsonBlock>
        {
            public ExposedJsonBlockParser(IJsonBlockFactory<Block, ProxyJsonBlock> jsonBlockFactory) : base(jsonBlockFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor processor)
            {
                return TryOpenBlock(processor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor processor, ProxyJsonBlock block)
            {
                return TryContinueBlock(processor, block);
            }

            public Block ExposedCloseProxy(BlockProcessor processor, ProxyJsonBlock block)
            {
                return CloseProxy(processor, block);
            }
        }

        private ExposedJsonBlockParser CreateExposedJsonBlockParser(IJsonBlockFactory<Block, ProxyJsonBlock> jsonBlockFactory = null)
        {
            return new ExposedJsonBlockParser(jsonBlockFactory ?? _mockRepository.Create<IJsonBlockFactory<Block, ProxyJsonBlock>>().Object);
        }

        private Mock<ExposedJsonBlockParser> CreateMockExposedJsonBlockParser(IJsonBlockFactory<Block, ProxyJsonBlock> jsonBlockFactory = null)
        {
            return _mockRepository.Create<ExposedJsonBlockParser>(jsonBlockFactory ?? _mockRepository.Create<IJsonBlockFactory<Block, ProxyJsonBlock>>().Object);
        }
    }
}
