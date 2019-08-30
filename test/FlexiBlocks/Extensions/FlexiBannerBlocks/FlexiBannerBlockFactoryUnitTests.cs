using Jering.Markdig.Extensions.FlexiBlocks.FlexiBannerBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiBannerBlocks
{
    public class FlexiBannerBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiBannerBlockFactory(null, new PlainBlockParser()));
        }

        [Fact]
        public void Create_CreatesFlexiBannerBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const string dummyLogoIcon = "dummyLogoIcon";
            const string dummyBackgroundIcon = "dummyBackgroundIcon";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiBannerBlockOptions> mockFlexiBannerBlockOptions = _mockRepository.Create<IFlexiBannerBlockOptions>();
            mockFlexiBannerBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiBannerBlockOptions.Setup(f => f.LogoIcon).Returns(dummyLogoIcon);
            mockFlexiBannerBlockOptions.Setup(f => f.BackgroundIcon).Returns(dummyBackgroundIcon);
            mockFlexiBannerBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            Mock<IOptionsService<IFlexiBannerBlockOptions, IFlexiBannerBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiBannerBlockOptions, IFlexiBannerBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((mockFlexiBannerBlockOptions.Object, (IFlexiBannerBlocksExtensionOptions)null));
            Mock<FlexiBannerBlockFactory> mockTestSubject = CreateMockFlexiBannerBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);

            // Act
            FlexiBannerBlock result = mockTestSubject.Object.Create(dummyBlockProcessor, dummyBlockParser.Object);

            // TODO how can we verify that ProcessInlineEnd event handler is added? Due to language restrictions, no simple solution, got to get Block.OnProcessInlinesEnd to fire
            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummyLogoIcon, result.LogoIcon);
            Assert.Equal(dummyBackgroundIcon, result.BackgroundIcon);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(0, result.Span.End);
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiBannerBlockFactory testSubject = CreateFlexiBannerBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-banner";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        private Mock<FlexiBannerBlockFactory> CreateMockFlexiBannerBlockFactory(IOptionsService<IFlexiBannerBlockOptions, IFlexiBannerBlocksExtensionOptions> optionsService = null,
            PlainBlockParser plainBlockParser = null)
        {
            return _mockRepository.
                Create<FlexiBannerBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiBannerBlockOptions, IFlexiBannerBlocksExtensionOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }

        private FlexiBannerBlockFactory CreateFlexiBannerBlockFactory(IOptionsService<IFlexiBannerBlockOptions, IFlexiBannerBlocksExtensionOptions> optionsService = null,
            PlainBlockParser plainBlockParser = null)
        {
            return new FlexiBannerBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiBannerBlockOptions, IFlexiBannerBlocksExtensionOptions>>().Object,
                plainBlockParser ?? new PlainBlockParser());
        }
    }
}
