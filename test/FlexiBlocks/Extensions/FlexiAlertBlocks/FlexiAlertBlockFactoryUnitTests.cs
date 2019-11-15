using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockFactoryUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Empty };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiAlertBlockFactory(null));
        }

        [Fact]
        public void Create_CreatesFlexiAlertBlock()
        {
            // Arrange
            const int dummyColumn = 2;
            var dummyLine = new StringSlice("dummyText", 3, 8);
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const string dummyType = "dummyType";
            const string dummyResolvedType = "dummyResolvedType";
            const string dummyIcon = "dummyIcon";
            const string dummyResolvedIcon = "dummyResolvedIcon";
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = dummyColumn;
            dummyBlockProcessor.Line = dummyLine;
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiAlertBlockOptions> mockFlexiAlertBlockOptions = _mockRepository.Create<IFlexiAlertBlockOptions>();
            mockFlexiAlertBlockOptions.Setup(f => f.BlockName).Returns(dummyBlockName);
            mockFlexiAlertBlockOptions.Setup(f => f.Type).Returns(dummyType);
            mockFlexiAlertBlockOptions.Setup(f => f.Icon).Returns(dummyIcon);
            mockFlexiAlertBlockOptions.Setup(f => f.Attributes).Returns(dummyAttributes);
            var dummyIcons = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<IFlexiAlertBlocksExtensionOptions> mockFlexiAlertBlockExtensionOptions = _mockRepository.Create<IFlexiAlertBlocksExtensionOptions>();
            mockFlexiAlertBlockExtensionOptions.Setup(f => f.Icons).Returns(dummyIcons);
            Mock<IOptionsService<IFlexiAlertBlockOptions, IFlexiAlertBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiAlertBlockOptions, IFlexiAlertBlocksExtensionOptions>>();
            mockOptionsService.Setup(f => f.CreateOptions(dummyBlockProcessor)).
                Returns((mockFlexiAlertBlockOptions.Object, mockFlexiAlertBlockExtensionOptions.Object));
            Mock<FlexiAlertBlockFactory> mockTestSubject = CreateMockFlexiAlertBlockFactory(mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Setup(t => t.ResolveType(dummyType)).Returns(dummyResolvedType);
            mockTestSubject.Setup(t => t.ResolveIcon(dummyIcon, dummyResolvedType, dummyIcons)).Returns(dummyResolvedIcon);

            // Act
            FlexiAlertBlock result = mockTestSubject.Object.Create(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummyResolvedType, result.Type);
            Assert.Equal(dummyResolvedIcon, result.Icon);
            Assert.Same(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine.Start, result.Span.Start);
            Assert.Equal(dummyLine.End, result.Span.End);
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiAlertBlockFactory testSubject = CreateFlexiAlertBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-alert";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveType_ResolvesType_Data))]
        public void ResolveType_ResolvesType(string dummyType, string expectedResult)
        {
            // Arrange
            FlexiAlertBlockFactory testSubject = CreateFlexiAlertBlockFactory();

            // Act
            string result = testSubject.ResolveType(dummyType);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveType_ResolvesType_Data()
        {
            const string dummyType = "dummyType";
            const string defaultType = "info";

            return new object[][]
            {
                new object[]{dummyType, dummyType},
                new object[]{null, defaultType},
                new object[]{" ", defaultType},
                new object[]{string.Empty, defaultType}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveIcon_ResolvesIcon_Data))]
        public void ResolveIcon_ResolvesIcon(string dummyIcon, string dummyType, Dictionary<string, string> dummyIcons, string expectedResult)
        {
            // Arrange
            FlexiAlertBlockFactory testSubject = CreateFlexiAlertBlockFactory();

            // Act
            string result = testSubject.ResolveIcon(dummyIcon, dummyType, new ReadOnlyDictionary<string, string>(dummyIcons));

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveIcon_ResolvesIcon_Data()
        {
            const string dummyType = "dummyType";
            const string dummyIcon = "dummyIcon";

            return new object[][]
            {
                new object[]{dummyIcon, null, new Dictionary<string, string>(), dummyIcon},
                // If Icon is null, whitespace or an empty string, an icon for the given type is retrieved from Icons if possible
                new object[]{null, dummyType, new Dictionary<string, string>() { { dummyType, dummyIcon } }, dummyIcon},
                new object[]{" ", dummyType, new Dictionary<string, string>() { { dummyType, dummyIcon } }, dummyIcon},
                new object[]{string.Empty, dummyType, new Dictionary<string, string>() { { dummyType, dummyIcon } }, dummyIcon},
                // If Icons doesn't have an icon for the given type
                new object[]{null, dummyType, new Dictionary<string, string>(), null}
            };
        }

        private Mock<FlexiAlertBlockFactory> CreateMockFlexiAlertBlockFactory(IOptionsService<IFlexiAlertBlockOptions, IFlexiAlertBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.
                Create<FlexiAlertBlockFactory>(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiAlertBlockOptions, IFlexiAlertBlocksExtensionOptions>>().Object);
        }

        private FlexiAlertBlockFactory CreateFlexiAlertBlockFactory(IOptionsService<IFlexiAlertBlockOptions, IFlexiAlertBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiAlertBlockFactory(optionsService ?? _mockRepository.Create<IOptionsService<IFlexiAlertBlockOptions, IFlexiAlertBlocksExtensionOptions>>().Object);
        }
    }
}
