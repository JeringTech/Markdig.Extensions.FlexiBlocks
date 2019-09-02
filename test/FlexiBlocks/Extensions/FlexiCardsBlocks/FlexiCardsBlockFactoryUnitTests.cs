using Jering.Markdig.Extensions.FlexiBlocks.FlexiCardsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCardsBlocks
{
    public class FlexiCardsBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiCardsBlockFactory(null));
        }

        [Fact]
        public void CreateProxyFencedBlock_CreatesProxyFencedBlock()
        {
            // Arrange
            const int dummyOpeningFenceIndent = 5;
            const int dummyOpeningFenceCharCount = 6;
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            IFlexiCardsBlockOptions dummyFlexiCardsBlockOptions = _mockRepository.Create<IFlexiCardsBlockOptions>().Object;
            Mock<IOptionsService<IFlexiCardsBlockOptions, IFlexiCardsBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiCardsBlockOptions, IFlexiCardsBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((dummyFlexiCardsBlockOptions, (IFlexiCardsBlocksExtensionOptions)null));
            Mock<FlexiCardsBlockFactory> mockTestSubject = CreateMockFlexiCardsBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ValidateDefaultCardOptions(dummyFlexiCardsBlockOptions));

            // Act
            ProxyFlexiCardsBlock result = mockTestSubject.Object.CreateProxyFencedBlock(dummyOpeningFenceIndent,
                dummyOpeningFenceCharCount,
                dummyBlockProcessor,
                dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiCardsBlockOptions, result.FlexiCardsBlockOptions);
            Assert.Equal(dummyOpeningFenceIndent, result.OpeningFenceIndent);
            Assert.Equal(dummyOpeningFenceCharCount, result.OpeningFenceCharCount);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(0, result.Span.End);
        }

        [Fact]
        public void Create_CreatesFlexiCardsBlock()
        {
            // Arrange
            var dummySpan = new SourceSpan(6, 19);
            const int dummyLine = 5;
            const int dummyColumn = 2;
            BlockParser dummyBlockParser = _mockRepository.Create<BlockParser>().Object;
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const FlexiCardBlockSize dummyCardSize = FlexiCardBlockSize.Medium;
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiCardsBlockOptions> mockFlexiCardsBlockOptions = _mockRepository.Create<IFlexiCardsBlockOptions>();
            mockFlexiCardsBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiCardsBlockOptions.Setup(f => f.CardSize).Returns(dummyCardSize);
            mockFlexiCardsBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            ProxyFlexiCardsBlock dummyProxyFlexiCardsBlock = CreateProxyFlexiCardsBlock(mockFlexiCardsBlockOptions.Object, blockParser: dummyBlockParser);
            dummyProxyFlexiCardsBlock.Line = dummyLine;
            dummyProxyFlexiCardsBlock.Column = dummyColumn;
            dummyProxyFlexiCardsBlock.Span = dummySpan;
            Mock<FlexiCardsBlockFactory> mockTestSubject = CreateMockFlexiCardsBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Setup(t => t.ValidateCardSize(dummyCardSize));
            mockTestSubject.Protected().Setup("MoveChildren", dummyProxyFlexiCardsBlock, ItExpr.IsAny<FlexiCardsBlock>());

            // Act
            FlexiCardsBlock result = mockTestSubject.Object.Create(dummyProxyFlexiCardsBlock, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummyCardSize, result.CardSize);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine, result.Line);
            Assert.Equal(dummySpan, result.Span);
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiCardsBlockFactory testSubject = CreateFlexiCardsBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-cards";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Fact]
        public void ValidateDefaultCardOptions_ThrowsOptionsExceptionIfDefaultCardOptionsIsNull()
        {
            // Arrange
            Mock<IFlexiCardsBlockOptions> mockFlexiCardsBlockOptions = _mockRepository.Create<IFlexiCardsBlockOptions>();
            mockFlexiCardsBlockOptions.Setup(f => f.DefaultCardOptions).Returns((IFlexiCardBlockOptions)null);
            FlexiCardsBlockFactory testSubject = CreateFlexiCardsBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateDefaultCardOptions(mockFlexiCardsBlockOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiCardsBlockOptions.DefaultCardOptions),
                    Strings.OptionsException_Shared_ValueMustNotBeNull),
                result.Message);
        }

        [Fact]
        public void ValidateDefaultCardOptions_DoesNothingIfDefaultCardOptionsIsNotNull()
        {
            // Arrange
            Mock<IFlexiCardsBlockOptions> mockFlexiCardsBlockOptions = _mockRepository.Create<IFlexiCardsBlockOptions>();
            mockFlexiCardsBlockOptions.Setup(f => f.DefaultCardOptions).Returns(_mockRepository.Create<IFlexiCardBlockOptions>().Object);
            FlexiCardsBlockFactory testSubject = CreateFlexiCardsBlockFactory();

            // Act and assert
            testSubject.ValidateDefaultCardOptions(mockFlexiCardsBlockOptions.Object); // All good as long as this doesn't throw
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ValidateCardSize_ThrowsOptionsExceptionIfCardSizeIsInvalid()
        {
            // Arrange
            FlexiCardsBlockFactory testSubject = CreateFlexiCardsBlockFactory();
            const FlexiCardBlockSize dummyCardSize = (FlexiCardBlockSize)9;

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateCardSize(dummyCardSize));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiCardsBlockOptions.CardSize),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummyCardSize,
                        nameof(FlexiCardBlockSize))),
                result.Message);
        }

        private Mock<FlexiCardsBlockFactory> CreateMockFlexiCardsBlockFactory(IOptionsService<IFlexiCardsBlockOptions, IFlexiCardsBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.
                Create<FlexiCardsBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiCardsBlockOptions, IFlexiCardsBlocksExtensionOptions>>().Object);
        }

        private FlexiCardsBlockFactory CreateFlexiCardsBlockFactory(IOptionsService<IFlexiCardsBlockOptions, IFlexiCardsBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiCardsBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiCardsBlockOptions, IFlexiCardsBlocksExtensionOptions>>().Object);
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
