using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    public class FlexiPictureBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfImageServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiPictureBlockFactory(null,
                _mockRepository.Create<IDirectoryService>().Object,
                _mockRepository.Create<IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiPictureBlockFactory(_mockRepository.Create<IImageService>().Object,
                _mockRepository.Create<IDirectoryService>().Object,
                null));
        }

        [Fact]
        public void Create_CreatesFlexiPictureBlock()
        {
            // Arrange
            const int dummyColumn = 3;
            const int dummyLine = 5;
            const double dummyWidth = 123;
            const double dummyResolvedWidth = 515;
            const double dummyHeight = 321;
            const double dummyResolvedHeight = 356;
            const double dummyResolvedAspectRatio = 968;
            var dummySpan = new SourceSpan(1, 5);
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const string dummySrc = "dummySrc";
            const string dummyFileName = "dummyFileName";
            const string dummyAlt = "dummyAlt";
            const bool dummyLazy = false;
            const string dummyExitFullscreenIcon = "dummyExitFullscreenIcon";
            const string dummyErrorIcon = "dummyErrorIcon";
            const string dummySpinner = "dummySpinner";
            const string dummyLocalAbsolutePath = "dummyLocalAbsolutePath";
            const bool dummyEnableFileOperations = false;
            const bool dummyResolvedEnableFileOperations = true;
            const string dummyLocalMediaDirectory = "dummyLocalMediaDirectory";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, dummyBlockParser.Object)
            {
                Line = dummyLine,
                Column = dummyColumn,
                Span = dummySpan
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IFlexiPictureBlockOptions> mockFlexiPictureBlockOptions = _mockRepository.Create<IFlexiPictureBlockOptions>();
            mockFlexiPictureBlockOptions.Setup(i => i.BlockName).Returns(dummyBlockName);
            mockFlexiPictureBlockOptions.Setup(i => i.Src).Returns(dummySrc);
            mockFlexiPictureBlockOptions.Setup(i => i.Alt).Returns(dummyAlt);
            mockFlexiPictureBlockOptions.Setup(i => i.Lazy).Returns(dummyLazy);
            mockFlexiPictureBlockOptions.Setup(i => i.ExitFullscreenIcon).Returns(dummyExitFullscreenIcon);
            mockFlexiPictureBlockOptions.Setup(i => i.ErrorIcon).Returns(dummyErrorIcon);
            mockFlexiPictureBlockOptions.Setup(i => i.Spinner).Returns(dummySpinner);
            mockFlexiPictureBlockOptions.Setup(i => i.Width).Returns(dummyWidth);
            mockFlexiPictureBlockOptions.Setup(i => i.Height).Returns(dummyHeight);
            mockFlexiPictureBlockOptions.Setup(i => i.EnableFileOperations).Returns(dummyEnableFileOperations);
            mockFlexiPictureBlockOptions.Setup(i => i.Attributes).Returns(dummyAttributes);
            Mock<IFlexiPictureBlocksExtensionOptions> mockFlexiPictureBlocksExtensionOptions = _mockRepository.Create<IFlexiPictureBlocksExtensionOptions>();
            mockFlexiPictureBlocksExtensionOptions.Setup(f => f.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            Mock<IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions>>();
            mockOptionsService.Setup(o => o.CreateOptions(dummyBlockProcessor, dummyProxyJsonBlock)).Returns((mockFlexiPictureBlockOptions.Object, mockFlexiPictureBlocksExtensionOptions.Object));
            Mock<FlexiPictureBlockFactory> mockTestSubject = CreateMockFlexiPictureBlockFactory(optionsService: mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Protected().Setup<string>("ValidateSrcAndResolveFileName", mockFlexiPictureBlockOptions.Object).Returns(dummyFileName);
            mockTestSubject.Setup(t => t.ResolveEnableFileOperations(dummyEnableFileOperations, dummyLocalMediaDirectory, dummyWidth, dummyHeight)).Returns(dummyResolvedEnableFileOperations);
            mockTestSubject.Protected().Setup<string>("ResolveLocalAbsolutePath", false, dummyResolvedEnableFileOperations, dummyFileName, mockFlexiPictureBlocksExtensionOptions.Object).Returns(dummyLocalAbsolutePath);
            mockTestSubject.Setup(t => t.ResolveDimensions(dummyLocalAbsolutePath, dummyWidth, dummyHeight)).Returns((dummyResolvedWidth, dummyResolvedHeight, dummyResolvedAspectRatio));

            // Act
            FlexiPictureBlock result = mockTestSubject.Object.Create(dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummySrc, result.Src);
            Assert.Equal(dummyAlt, result.Alt);
            Assert.Equal(dummyLazy, result.Lazy);
            Assert.Equal(dummyResolvedWidth, result.Width);
            Assert.Equal(dummyResolvedHeight, result.Height);
            Assert.Equal(dummyResolvedAspectRatio, result.AspectRatio);
            Assert.Equal(dummyExitFullscreenIcon, result.ExitFullscreenIcon);
            Assert.Equal(dummyErrorIcon, result.ErrorIcon);
            Assert.Equal(dummySpinner, result.Spinner);
            Assert.Equal(dummyAttributes, result.Attributes);
            Assert.Same(dummyBlockParser.Object, result.Parser);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyLine, result.Line);
            Assert.Equal(dummySpan, result.Span);
        }

        [Theory]
        [MemberData(nameof(ResolveBlockName_ResolvesBlockName_Data))]
        public void ResolveBlockName_ResolvesBlockName(string dummyBlockName, string expectedResult)
        {
            // Arrange
            FlexiPictureBlockFactory testSubject = CreateFlexiPictureBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-picture";

            return new object[][]
            {
                new object[]{dummyBlockName, dummyBlockName},
                new object[]{null, defaultBlockName},
                new object[]{" ", defaultBlockName},
                new object[]{string.Empty, defaultBlockName}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveEnableFileOperations_ReturnsTrueIfFileOperationsAreEnabled_Data))]
        public void ResolveEnableFileOperations_ReturnsTrueIfFileOperationsAreEnabled(bool dummyEnableFileOperations,
            string dummyLocalMediaDirectory,
            double dummyWidth,
            double dummyHeight,
            bool expectedResult)
        {
            // Arrange
            FlexiPictureBlockFactory testSubject = CreateFlexiPictureBlockFactory();

            // Act
            bool result = testSubject.ResolveEnableFileOperations(dummyEnableFileOperations, dummyLocalMediaDirectory, dummyWidth, dummyHeight);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveEnableFileOperations_ReturnsTrueIfFileOperationsAreEnabled_Data()
        {
            return new object[][]
            {
                // False if EnableFileOperations is false
                new object[]{false, "dummyLocalMediaDirectory", 0, 0, false},
                // False if LocalMediaDirectory is null, whitespace or an empty string
                new object[]{true, null, 0, 0, false},
                new object[]{true, " ", 0, 0, false},
                new object[]{true, string.Empty, 0, 0, false},
                // False if dimensions are specified
                new object[]{true, "dummyLocalMediaDirectory", 123, 321, false},
                // True otherwise
                new object[]{true, "dummyLocalMediaDirectory", 123, 0, true},
                new object[]{true, "dummyLocalMediaDirectory", 0, 321, true},
                new object[]{true, "dummyLocalMediaDirectory", 0, 0, true},
            };
        }

        [Theory]
        [MemberData(nameof(ResolveDimensions_ResolvesDimensionsWithoutFileOperationsWhenPossibleOrIfFileOperationsAreDisabled_Data))]
        public void ResolveDimensions_ResolvesDimensionsWithoutFileOperationsWhenPossibleOrIfFileOperationsAreDisabled(double dummySpecifiedWidth,
            double dummySpecifiedHeight,
            string dummyLocalAbsolutePath,
            double expectedWidth,
            double expectedHeight,
            double expectedAspectRatio)
        {
            // Arrange
            FlexiPictureBlockFactory testSubject = CreateFlexiPictureBlockFactory();

            // Act
            (double resultWidth, double resultHeight, double resultAspectRatio) = testSubject.ResolveDimensions(dummyLocalAbsolutePath, dummySpecifiedWidth, dummySpecifiedHeight);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedWidth, resultWidth);
            Assert.Equal(expectedHeight, resultHeight);
            Assert.Equal(expectedAspectRatio, resultAspectRatio);
        }

        public static IEnumerable<object[]> ResolveDimensions_ResolvesDimensionsWithoutFileOperationsWhenPossibleOrIfFileOperationsAreDisabled_Data()
        {
            const double dummySpecifiedWidth = 543;
            const double dummySpecifiedHeight = 897;

            return new object[][]
            {
                // Width and height specified
                new object[]{dummySpecifiedWidth, dummySpecifiedHeight, "dummyLocalAbsoluteData", dummySpecifiedWidth, dummySpecifiedHeight, dummySpecifiedHeight / dummySpecifiedWidth * 100},
                // Width and/or height not specified, file operations disabled
                new object[]{dummySpecifiedWidth, 0, null, dummySpecifiedWidth, 0, 0},
                new object[]{0, dummySpecifiedHeight, null, 0, dummySpecifiedHeight, 0},
                new object[]{0, 0, null, 0, 0, 0},
            };
        }

        [Theory]
        [MemberData(nameof(ResolveDimensions_ResolvesDimensionsUsingFileOperationsWhenNecessaryAndFileOperationsAreEnabled_Data))]
        public void ResolveDimensions_ResolvesDimensionsUsingFileOperationsWhenNecessaryAndFileOperationsAreEnabled(double dummySpecifiedWidth,
            double dummySpecifiedHeight,
            int dummyRetrievedWidth,
            int dummyRetrievedHeight,
            double expectedWidth,
            double expectedHeight,
            double expectedAspectRatio)
        {
            // Arrange
            const string dummyLocalAbsolutePath = "dummyLocalAbsolutePath";
            Mock<IImageService> mockImageService = _mockRepository.Create<IImageService>();
            mockImageService.Setup(i => i.GetImageDimensions(dummyLocalAbsolutePath)).Returns((dummyRetrievedWidth, dummyRetrievedHeight));
            FlexiPictureBlockFactory testSubject = CreateFlexiPictureBlockFactory(mockImageService.Object);

            // Act
            (double resultWidth, double resultHeight, double resultAspectRatio) = testSubject.ResolveDimensions(dummyLocalAbsolutePath, dummySpecifiedWidth, dummySpecifiedHeight);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedWidth, resultWidth);
            Assert.Equal(expectedHeight, resultHeight);
            Assert.Equal(expectedAspectRatio, resultAspectRatio);
        }

        public static IEnumerable<object[]> ResolveDimensions_ResolvesDimensionsUsingFileOperationsWhenNecessaryAndFileOperationsAreEnabled_Data()
        {
            const double dummySpecifiedWidth = 543;
            const double dummySpecifiedHeight = 897;
            const int dummyRetrievedWidth = 563;
            const int dummyRetrievedHeight = 123;

            return new object[][]
            {
                // No specified width and or height
                new object[]{dummySpecifiedWidth, 0, dummyRetrievedWidth, dummyRetrievedHeight, dummySpecifiedWidth, dummyRetrievedHeight, dummyRetrievedHeight / dummySpecifiedWidth * 100},
                new object[]{0, dummySpecifiedHeight, dummyRetrievedWidth, dummyRetrievedHeight, dummyRetrievedWidth, dummySpecifiedHeight, dummySpecifiedHeight / dummyRetrievedWidth * 100},
                new object[]{0, 0, dummyRetrievedWidth, dummyRetrievedHeight, dummyRetrievedWidth, dummyRetrievedHeight, dummyRetrievedHeight / (double) dummyRetrievedWidth * 100 },
                // If image is wierd or metadata is corrupted, retrieved dimensions may be 0. We can't differentiate between wierdness and messed up metadata so we just 
                // avoid divide by 0 exceptions
                new object[]{0, 0, dummyRetrievedWidth, 0, dummyRetrievedWidth, 0, 0 },
                new object[]{0, 0, 0, dummyRetrievedHeight, 0, dummyRetrievedHeight, 0 },
                new object[]{0, 0, 0, 0, 0, 0, 0 }
            };
        }

        private Mock<FlexiPictureBlockFactory> CreateMockFlexiPictureBlockFactory(IImageService imageService = null,
            IDirectoryService directoryService = null,
            IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.Create<FlexiPictureBlockFactory>(imageService ?? _mockRepository.Create<IImageService>().Object,
                directoryService ?? _mockRepository.Create<IDirectoryService>().Object,
                optionsService ?? _mockRepository.Create<IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions>>().Object);
        }

        private FlexiPictureBlockFactory CreateFlexiPictureBlockFactory(IImageService imageService = null,
            IDirectoryService directoryService = null,
            IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiPictureBlockFactory(imageService ?? _mockRepository.Create<IImageService>().Object,
                directoryService ?? _mockRepository.Create<IDirectoryService>().Object,
                optionsService ?? _mockRepository.Create<IOptionsService<IFlexiPictureBlockOptions, IFlexiPictureBlocksExtensionOptions>>().Object);
        }
    }
}
