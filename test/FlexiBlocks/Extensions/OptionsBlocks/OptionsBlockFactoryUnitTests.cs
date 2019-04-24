using Jering.Markdig.Extensions.FlexiBlocks.OptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.OptionsBlocks
{
    public class OptionsBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateProxyJsonBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            OptionsBlockFactory testSubject = CreateOptionsBlockFactory();

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
            OptionsBlockFactory testSubject = CreateOptionsBlockFactory();

            // Act
            ProxyJsonBlock result = testSubject.CreateProxyJsonBlock(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            Assert.Equal(result.Column, dummyColumn);
            Assert.Equal(result.Span.Start, dummyLineStart);
            Assert.Equal(nameof(OptionsBlock), result.MainTypeName);
            Assert.Same(dummyBlockParser.Object, result.Parser);
        }

        [Fact]
        public void Create_ThrowsArgumentNullExceptionIfProxyJsonBlockIsNull()
        {
            // Arrange
            OptionsBlockFactory testSubject = CreateOptionsBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(null, MarkdigTypesFactory.CreateBlockProcessor()));
        }

        [Fact]
        public void Create_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            OptionsBlockFactory testSubject = CreateOptionsBlockFactory();

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => testSubject.Create(new ProxyJsonBlock(null, null), null));
        }

        [Fact]
        public void Create_ThrowsBlockExceptionIfThereIsAnUnconsumedOptionsBlock()
        {
            // Arrange
            const int dummyLineIndex = 1;
            const int dummyColumn = 2;
            var dummyPendingOptionsBlock = new OptionsBlock(null)
            {
                Line = dummyLineIndex,
                Column = dummyColumn
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(OptionsBlockFactory.PENDING_OPTIONS_BLOCK, dummyPendingOptionsBlock);
            OptionsBlockFactory testSubject = CreateOptionsBlockFactory();

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => testSubject.Create(new ProxyJsonBlock(null, null), dummyBlockProcessor));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    nameof(OptionsBlock),
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.BlockException_OptionsBlockParser_UnconsumedBlock),
                result.Message);
        }

        [Fact]
        public void Create_CreatesOptionsBlockAndAddsItToDocumentData()
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
            OptionsBlockFactory testSubject = CreateOptionsBlockFactory();

            // Act
            OptionsBlock result = testSubject.Create(dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            Assert.Null(result);
            var resultOptionsBlock = dummyBlockProcessor.Document.GetData(OptionsBlockFactory.PENDING_OPTIONS_BLOCK) as OptionsBlock;
            Assert.NotNull(resultOptionsBlock);
            Assert.Same(dummyBlockParser.Object, resultOptionsBlock.Parser);
            Assert.Equal(dummyLines, resultOptionsBlock.Lines); // Default ValueType comparer - https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/src/System/ValueType.cs
            Assert.Equal(dummyLine, resultOptionsBlock.Line);
            Assert.Equal(dummyColumn, resultOptionsBlock.Column);
            Assert.Equal(dummySpan, resultOptionsBlock.Span); // Default ValueType comparer - https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/src/System/ValueType.cs
        }

        private OptionsBlockFactory CreateOptionsBlockFactory()
        {
            return new OptionsBlockFactory();
        }
    }
}
