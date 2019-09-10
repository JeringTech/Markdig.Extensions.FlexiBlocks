using Jering.Markdig.Extensions.FlexiBlocks.FlexiTabsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiTabsBlocks
{
    public class FlexiTabsBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiTabsBlockFactory(null));
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
            IFlexiTabsBlockOptions dummyFlexiTabsBlockOptions = _mockRepository.Create<IFlexiTabsBlockOptions>().Object;
            Mock<IOptionsService<IFlexiTabsBlockOptions, IFlexiTabsBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiTabsBlockOptions, IFlexiTabsBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((dummyFlexiTabsBlockOptions, (IFlexiTabsBlocksExtensionOptions)null));
            Mock<FlexiTabsBlockFactory> mockTestSubject = CreateMockFlexiTabsBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ValidateDefaultTabOptions(dummyFlexiTabsBlockOptions));

            // Act
            ProxyFlexiTabsBlock result = mockTestSubject.Object.CreateProxyFencedBlock(dummyOpeningFenceIndent,
                dummyOpeningFenceCharCount,
                dummyBlockProcessor,
                dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiTabsBlockOptions, result.FlexiTabsBlockOptions);
            Assert.Equal(dummyOpeningFenceIndent, result.OpeningFenceIndent);
            Assert.Equal(dummyOpeningFenceCharCount, result.OpeningFenceCharCount);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(0, result.Span.End);
        }

        [Fact]
        public void Create_CreatesFlexiTabsBlock()
        {
            // Arrange
            var dummySpan = new SourceSpan(6, 19);
            const int dummyLine = 5;
            const int dummyColumn = 2;
            BlockParser dummyBlockParser = _mockRepository.Create<BlockParser>().Object;
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiTabsBlockOptions> mockFlexiTabsBlockOptions = _mockRepository.Create<IFlexiTabsBlockOptions>();
            mockFlexiTabsBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiTabsBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            ProxyFlexiTabsBlock dummyProxyFlexiTabsBlock = CreateProxyFlexiTabsBlock(mockFlexiTabsBlockOptions.Object, blockParser: dummyBlockParser);
            dummyProxyFlexiTabsBlock.Line = dummyLine;
            dummyProxyFlexiTabsBlock.Column = dummyColumn;
            dummyProxyFlexiTabsBlock.Span = dummySpan;
            Mock<FlexiTabsBlockFactory> mockTestSubject = CreateMockFlexiTabsBlockFactory();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Protected().Setup("MoveChildren", dummyProxyFlexiTabsBlock, ItExpr.IsAny<FlexiTabsBlock>());

            // Act
            FlexiTabsBlock result = mockTestSubject.Object.Create(dummyProxyFlexiTabsBlock, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
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
            FlexiTabsBlockFactory testSubject = CreateFlexiTabsBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-tabs";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Fact]
        public void ValidateDefaultTabOptions_ThrowsOptionsExceptionIfDefaultTabOptionsIsNull()
        {
            // Arrange
            Mock<IFlexiTabsBlockOptions> mockFlexiTabsBlockOptions = _mockRepository.Create<IFlexiTabsBlockOptions>();
            mockFlexiTabsBlockOptions.Setup(f => f.DefaultTabOptions).Returns((IFlexiTabBlockOptions)null);
            FlexiTabsBlockFactory testSubject = CreateFlexiTabsBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateDefaultTabOptions(mockFlexiTabsBlockOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiTabsBlockOptions.DefaultTabOptions),
                    Strings.OptionsException_Shared_ValueMustNotBeNull),
                result.Message);
        }

        [Fact]
        public void ValidateDefaultTabOptions_DoesNothingIfDefaultTabOptionsIsNotNull()
        {
            // Arrange
            Mock<IFlexiTabsBlockOptions> mockFlexiTabsBlockOptions = _mockRepository.Create<IFlexiTabsBlockOptions>();
            mockFlexiTabsBlockOptions.Setup(f => f.DefaultTabOptions).Returns(_mockRepository.Create<IFlexiTabBlockOptions>().Object);
            FlexiTabsBlockFactory testSubject = CreateFlexiTabsBlockFactory();

            // Act and assert
            testSubject.ValidateDefaultTabOptions(mockFlexiTabsBlockOptions.Object); // All good as long as this doesn't throw
            _mockRepository.VerifyAll();
        }

        private Mock<FlexiTabsBlockFactory> CreateMockFlexiTabsBlockFactory(IOptionsService<IFlexiTabsBlockOptions, IFlexiTabsBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.
                Create<FlexiTabsBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiTabsBlockOptions, IFlexiTabsBlocksExtensionOptions>>().Object);
        }

        private FlexiTabsBlockFactory CreateFlexiTabsBlockFactory(IOptionsService<IFlexiTabsBlockOptions, IFlexiTabsBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiTabsBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiTabsBlockOptions, IFlexiTabsBlocksExtensionOptions>>().Object);
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
