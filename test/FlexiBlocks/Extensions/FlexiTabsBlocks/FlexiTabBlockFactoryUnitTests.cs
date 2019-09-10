using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTabsBlocks
{
    public class FlexiTabBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiTabBlockOptionsFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTabBlockFactory(null, new PlainBlockParser()));
        }

        [Fact]
        public void Create_CreatesFlexiTabBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            IFlexiTabBlockOptions dummyDefaultTabOptions = _mockRepository.Create<IFlexiTabBlockOptions>().Object;
            Mock<IFlexiTabsBlockOptions> mockFlexiTabsBlockOptions = _mockRepository.Create<IFlexiTabsBlockOptions>();
            mockFlexiTabsBlockOptions.Setup(f => f.DefaultTabOptions).Returns(dummyDefaultTabOptions);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            ProxyFlexiTabsBlock dummyProxyFlexiTabsBlock = CreateProxyFlexiTabsBlock(mockFlexiTabsBlockOptions.Object, blockParser: dummyBlockParser.Object);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // Following 3 lines set dummyBlockProcessor.CurrentContainer
            dummyBlockParser.Setup(b => b.TryContinue(dummyBlockProcessor, dummyProxyFlexiTabsBlock)).Returns(BlockState.ContinueDiscard);
            dummyBlockProcessor.Open(dummyProxyFlexiTabsBlock);
            dummyBlockProcessor.ProcessLine(new StringSlice(string.Empty));
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiTabBlockOptions> mockFlexiTabBlockOptions = _mockRepository.Create<IFlexiTabBlockOptions>();
            mockFlexiTabBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IBlockOptionsFactory<IFlexiTabBlockOptions>> mockFlexiTabBlockOptionsFactory = _mockRepository.
                Create<IBlockOptionsFactory<IFlexiTabBlockOptions>>();
            mockFlexiTabBlockOptionsFactory.Setup(f => f.Create(dummyDefaultTabOptions, dummyBlockProcessor)).Returns(mockFlexiTabBlockOptions.Object);
            FlexiTabBlockFactory testSubject = CreateFlexiTabBlockFactory(mockFlexiTabBlockOptionsFactory.Object);

            // Act
            FlexiTabBlock result = testSubject.Create(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(0, result.Span.End);
        }

        private FlexiTabBlockFactory CreateFlexiTabBlockFactory(IBlockOptionsFactory<IFlexiTabBlockOptions> flexiTabBlockOptionsFactory = null,
            PlainBlockParser plainBlockParser = null)
        {
            return new FlexiTabBlockFactory(flexiTabBlockOptionsFactory ?? _mockRepository.Create<IBlockOptionsFactory<IFlexiTabBlockOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private static ProxyFlexiTabsBlock CreateProxyFlexiTabsBlock(IFlexiTabsBlockOptions flexiTabsBlockOptions = default,
            int openingFenceIndex = default,
            int openingFenceCharCount = default,
            BlockParser blockParser = default)
        {
            return new ProxyFlexiTabsBlock(flexiTabsBlockOptions, openingFenceIndex, openingFenceCharCount, blockParser);
        }
    }
}
