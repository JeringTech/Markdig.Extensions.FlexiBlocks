using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiOptionsBlocks
{
    public class FlexiOptionsBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateProxyJsonBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            FlexiOptionsBlockFactory testSubject = CreateFlexiOptionsBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.CreateProxyJsonBlock(null, _mockRepository.Create<BlockParser>().Object));
        }

        [Fact]
        public void CreateProxyJsonBlock_CreatesProxyJsonBlock()
        {
            // Arrange
            const int dummyColumn = 4;
            const int dummyLineStart = 2;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = new StringSlice("", dummyLineStart, 10);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            FlexiOptionsBlockFactory testSubject = CreateFlexiOptionsBlockFactory();

            // Act
            ProxyJsonBlock result = testSubject.CreateProxyJsonBlock(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            Assert.Equal(result.Column, dummyColumn);
            Assert.Equal(result.Span.Start, dummyLineStart);
            Assert.Equal(nameof(FlexiOptionsBlock), result.MainTypeName);
            Assert.Same(dummyBlockParser.Object, result.Parser);
        }

        [Fact]
        public void Create_ThrowsArgumentNullExceptionIfProxyJsonBlockIsNull()
        {
            // Arrange
            FlexiOptionsBlockFactory testSubject = CreateFlexiOptionsBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(null, MarkdigTypesFactory.CreateBlockProcessor()));
        }

        [Fact]
        public void Create_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            FlexiOptionsBlockFactory testSubject = CreateFlexiOptionsBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(new ProxyJsonBlock(null, null), null));
        }

        [Fact]
        public void Create_ThrowsBlockExceptionIfThereIsAnUnconsumedFlexiOptionsBlock()
        {
            // Arrange
            const int dummyLineIndex = 1;
            const int dummyColumn = 2;
            var dummyPendingFlexiOptionsBlock = new FlexiOptionsBlock(null)
            {
                Line = dummyLineIndex,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiOptionsBlockFactory.PENDING_FLEXI_OPTIONS_BLOCK, dummyPendingFlexiOptionsBlock);
            FlexiOptionsBlockFactory testSubject = CreateFlexiOptionsBlockFactory();

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => testSubject.Create(new ProxyJsonBlock(null, null), dummyBlockProcessor));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    nameof(FlexiOptionsBlock),
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.BlockException_FlexiOptionsBlockParser_UnconsumedBlock),
                result.Message);
        }

        [Fact]
        public void Create_CreatesFlexiOptionsBlockAndAddsItToDocumentData()
        {
            // Arrange
            const int dummyLine = 6;
            const int dummyColumn = 3;
            var dummySpan = new SourceSpan(9, 12);
            var dummyLines = new StringLineGroup(2);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, dummyBlockParser.Object)
            {
                Lines = dummyLines,
                Line = dummyLine,
                Column = dummyColumn,
                Span = dummySpan
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiOptionsBlockFactory testSubject = CreateFlexiOptionsBlockFactory();

            // Act
            FlexiOptionsBlock result = testSubject.Create(dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            Assert.Null(result);
            var resultFlexiOptionsBlock = dummyBlockProcessor.Document.GetData(FlexiOptionsBlockFactory.PENDING_FLEXI_OPTIONS_BLOCK) as FlexiOptionsBlock;
            Assert.NotNull(resultFlexiOptionsBlock);
            Assert.Same(dummyBlockParser.Object, resultFlexiOptionsBlock.Parser);
            Assert.Equal(dummyLines, resultFlexiOptionsBlock.Lines); // Default ValueType comparer - https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/src/System/ValueType.cs
            Assert.Equal(dummyLine, resultFlexiOptionsBlock.Line);
            Assert.Equal(dummyColumn, resultFlexiOptionsBlock.Column);
            Assert.Equal(dummySpan, resultFlexiOptionsBlock.Span); // Default ValueType comparer - https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/src/System/ValueType.cs
        }

        private FlexiOptionsBlockFactory CreateFlexiOptionsBlockFactory()
        {
            return new FlexiOptionsBlockFactory();
        }
    }
}
