using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCardsBlocks
{
    public class FlexiCardBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiCardBlockOptionsFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCardBlockFactory(null, new PlainBlockParser()));
        }

        [Fact]
        public void Create_CreatesFlexiCardBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            const string dummyUrl = "dummyUrl";
            const string dummyBackgroundIcon = "dummyBackgroundIcon";
            IFlexiCardBlockOptions dummyDefaultCardOptions = _mockRepository.Create<IFlexiCardBlockOptions>().Object;
            Mock<IFlexiCardsBlockOptions> mockFlexiCardsBlockOptions = _mockRepository.Create<IFlexiCardsBlockOptions>();
            mockFlexiCardsBlockOptions.Setup(f => f.DefaultCardOptions).Returns(dummyDefaultCardOptions);
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            ProxyFlexiCardsBlock dummyProxyFlexiCardsBlock = CreateProxyFlexiCardsBlock(mockFlexiCardsBlockOptions.Object, blockParser: dummyBlockParser.Object);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            // Following 3 lines set dummyBlockProcessor.CurrentContainer
            dummyBlockParser.Setup(b => b.TryContinue(dummyBlockProcessor, dummyProxyFlexiCardsBlock)).Returns(BlockState.ContinueDiscard);
            dummyBlockProcessor.Open(dummyProxyFlexiCardsBlock);
            dummyBlockProcessor.ProcessLine(new StringSlice(string.Empty));
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiCardBlockOptions> mockFlexiCardBlockOptions = _mockRepository.Create<IFlexiCardBlockOptions>();
            mockFlexiCardBlockOptions.Setup(f => f.Url).Returns(dummyUrl);
            mockFlexiCardBlockOptions.Setup(f => f.BackgroundIcon).Returns(dummyBackgroundIcon);
            mockFlexiCardBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IBlockOptionsFactory<IFlexiCardBlockOptions>> mockFlexiCardBlockOptionsFactory = _mockRepository.
                Create<IBlockOptionsFactory<IFlexiCardBlockOptions>>();
            mockFlexiCardBlockOptionsFactory.Setup(f => f.Create(dummyDefaultCardOptions, dummyBlockProcessor)).Returns(mockFlexiCardBlockOptions.Object);
            FlexiCardBlockFactory testSubject = CreateFlexiCardBlockFactory(mockFlexiCardBlockOptionsFactory.Object);

            // Act
            FlexiCardBlock result = testSubject.Create(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyUrl, result.Url);
            Assert.Equal(dummyBackgroundIcon, result.BackgroundIcon);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(0, result.Span.End);
        }

        private FlexiCardBlockFactory CreateFlexiCardBlockFactory(IBlockOptionsFactory<IFlexiCardBlockOptions> flexiCardBlockOptionsFactory = null,
            PlainBlockParser plainBlockParser = null)
        {
            return new FlexiCardBlockFactory(flexiCardBlockOptionsFactory ?? _mockRepository.Create<IBlockOptionsFactory<IFlexiCardBlockOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private static ProxyFlexiCardsBlock CreateProxyFlexiCardsBlock(IFlexiCardsBlockOptions flexiCardsBlockOptions = default,
            int openingFenceIndex = default,
            int openingFenceCharCount = default,
            BlockParser blockParser = default)
        {
            return new ProxyFlexiCardsBlock(flexiCardsBlockOptions, openingFenceIndex, openingFenceCharCount, blockParser);
        }
    }
}
