using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    public class FlexiVideoBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfVideoServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiVideoBlockFactory(null,
                _mockRepository.Create<IDirectoryService>().Object,
                _mockRepository.Create<IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions>>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiVideoBlockFactory(_mockRepository.Create<IVideoService>().Object,
                _mockRepository.Create<IDirectoryService>().Object,
                null));
        }

        [Fact]
        public void Create_CreatesFlexiVideoBlock()
        {
            // Arrange
            const int dummyColumn = 3;
            const int dummyLine = 5;
            var dummySpan = new SourceSpan(1, 5);
            const string dummyBlockName = "dummyBlockName";
            const string dummyResolvedBlockName = "dummyResolvedBlockName";
            const string dummySrc = "dummySrc";
            const string dummyType = "dummyType";
            const double dummyWidth = 123;
            const double dummyResolvedWidth = 515;
            const double dummyHeight = 321;
            const double dummyResolvedHeight = 356;
            const double dummyDuration = 123.456;
            const double dummyResolvedDuration = 321.654;
            const bool dummyGeneratePoster = true;
            const string dummyPoster = "dummyPoster";
            const string dummyResolvedPoster = "dummyResolvedPoster";
            const bool dummyEnableFileOperations = false;
            const bool dummyResolvedEnableFileOperations = true;
            const string dummySpinner = "dummySpinner";
            const string dummyPlayIcon = "dummyPlayIcon";
            const string dummyPauseIcon = "dummyPauseIcon";
            const string dummyFullscreenIcon = "dummyFullscreenIcon";
            const string dummyExitFullscreenIcon = "dummyExitFullscreenIcon";
            const string dummyErrorIcon = "dummyErrorIcon";
            var dummyAttributes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            const string dummyLocalMediaDirectory = "dummyLocalMediaDirectory";
            var dummyMimeTypes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            const string dummyFileName = "dummyFileName";
            const string dummyLocalAbsolutePath = "dummyLocalAbsolutePath";
            const double dummyAspectRatio = 968;
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, dummyBlockParser.Object)
            {
                Line = dummyLine,
                Column = dummyColumn,
                Span = dummySpan
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IFlexiVideoBlockOptions> mockFlexiVideoBlockOptions = _mockRepository.Create<IFlexiVideoBlockOptions>();
            mockFlexiVideoBlockOptions.Setup(i => i.BlockName).Returns(dummyBlockName);
            mockFlexiVideoBlockOptions.Setup(i => i.Src).Returns(dummySrc);
            mockFlexiVideoBlockOptions.Setup(i => i.Type).Returns(dummyType);
            mockFlexiVideoBlockOptions.Setup(i => i.Width).Returns(dummyWidth);
            mockFlexiVideoBlockOptions.Setup(i => i.Height).Returns(dummyHeight);
            mockFlexiVideoBlockOptions.Setup(i => i.Duration).Returns(dummyDuration);
            mockFlexiVideoBlockOptions.Setup(i => i.GeneratePoster).Returns(dummyGeneratePoster);
            mockFlexiVideoBlockOptions.Setup(i => i.Poster).Returns(dummyPoster);
            mockFlexiVideoBlockOptions.Setup(i => i.EnableFileOperations).Returns(dummyEnableFileOperations);
            mockFlexiVideoBlockOptions.Setup(i => i.Spinner).Returns(dummySpinner);
            mockFlexiVideoBlockOptions.Setup(i => i.PlayIcon).Returns(dummyPlayIcon);
            mockFlexiVideoBlockOptions.Setup(i => i.PauseIcon).Returns(dummyPauseIcon);
            mockFlexiVideoBlockOptions.Setup(i => i.FullscreenIcon).Returns(dummyFullscreenIcon);
            mockFlexiVideoBlockOptions.Setup(i => i.ExitFullscreenIcon).Returns(dummyExitFullscreenIcon);
            mockFlexiVideoBlockOptions.Setup(i => i.ErrorIcon).Returns(dummyErrorIcon);
            mockFlexiVideoBlockOptions.Setup(i => i.Attributes).Returns(dummyAttributes);
            Mock<IFlexiVideoBlocksExtensionOptions> mockFlexiVideoBlocksExtensionOptions = _mockRepository.Create<IFlexiVideoBlocksExtensionOptions>();
            mockFlexiVideoBlocksExtensionOptions.Setup(f => f.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            mockFlexiVideoBlocksExtensionOptions.Setup(f => f.MimeTypes).Returns(dummyMimeTypes);
            Mock<IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions>>();
            mockOptionsService.Setup(o => o.CreateOptions(dummyBlockProcessor, dummyProxyJsonBlock)).Returns((mockFlexiVideoBlockOptions.Object, mockFlexiVideoBlocksExtensionOptions.Object));
            Mock<FlexiVideoBlockFactory> mockTestSubject = CreateMockFlexiVideoBlockFactory(optionsService: mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ResolveBlockName(dummyBlockName)).Returns(dummyResolvedBlockName);
            mockTestSubject.Protected().Setup<string>("ValidateSrcAndResolveFileName", mockFlexiVideoBlockOptions.Object).Returns(dummyFileName);
            mockTestSubject.Setup(t => t.ResolveType(dummyFileName, dummyType, dummyMimeTypes)).Returns(dummyType);
            mockTestSubject.
                Setup(t => t.ResolveEnableFileOperations(dummyEnableFileOperations, dummyLocalMediaDirectory, dummyGeneratePoster, dummyPoster, dummyWidth, dummyHeight, dummyDuration)).
                Returns(dummyResolvedEnableFileOperations);
            mockTestSubject.Protected().Setup<string>("ResolveLocalAbsolutePath", false, dummyResolvedEnableFileOperations, dummyFileName, mockFlexiVideoBlocksExtensionOptions.Object).Returns(dummyLocalAbsolutePath);
            mockTestSubject.
                Setup(t => t.ResolveDimensionsAndDuration(dummyLocalAbsolutePath, dummyWidth, dummyHeight, dummyDuration)).
                Returns((dummyResolvedWidth, dummyResolvedHeight, dummyAspectRatio, dummyResolvedDuration));
            mockTestSubject.Setup(t => t.ResolvePoster(dummyLocalAbsolutePath, dummySrc, dummyPoster, dummyGeneratePoster)).Returns(dummyResolvedPoster);

            // Act
            FlexiVideoBlock result = mockTestSubject.Object.Create(dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyResolvedBlockName, result.BlockName);
            Assert.Equal(dummySrc, result.Src);
            Assert.Equal(dummyType, result.Type);
            Assert.Equal(dummyResolvedPoster, result.Poster);
            Assert.Equal(dummyResolvedWidth, result.Width);
            Assert.Equal(dummyResolvedHeight, result.Height);
            Assert.Equal(dummyAspectRatio, result.AspectRatio);
            Assert.Equal(dummyResolvedDuration, result.Duration);
            Assert.Equal(dummySpinner, result.Spinner);
            Assert.Equal(dummyPlayIcon, result.PlayIcon);
            Assert.Equal(dummyPauseIcon, result.PauseIcon);
            Assert.Equal(dummyFullscreenIcon, result.FullscreenIcon);
            Assert.Equal(dummyExitFullscreenIcon, result.ExitFullscreenIcon);
            Assert.Equal(dummyErrorIcon, result.ErrorIcon);
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
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory();

            // Act
            string result = testSubject.ResolveBlockName(dummyBlockName);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveBlockName_ResolvesBlockName_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string defaultBlockName = "flexi-video";

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
        public void ResolveType_ResolvesType(string dummyFileName,
            string dummyType,
            ReadOnlyDictionary<string, string> dummyMimeTypes,
            string expectedResult)
        {
            // Arrange
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory();

            // Act
            string result = testSubject.ResolveType(dummyFileName, dummyType, dummyMimeTypes);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveType_ResolvesType_Data()
        {
            const string dummyType = "dummyType";
            ReadOnlyDictionary<string, string> dummyMimeTypes = new FlexiVideoBlocksExtensionOptions().MimeTypes;

            return new object[][]
            {
                // Returns type if it isn't null, whitespace or an empty string
                new object[]{null, dummyType, null, dummyType},
                // Returns MIME type from MimeTypes if type is null, whitespace or an empty string and MimeTypes contains file's extension
                new object[]{"dummyFileName.mp4", null, dummyMimeTypes, "video/mp4"},
                new object[]{"dummyFileName.webm", null, dummyMimeTypes, "video/webm"},
                new object[]{"dummyFileName.ogg", null, dummyMimeTypes, "video/ogg"},
                // Returns null if type is null, whitespace or an empty string and MimeTypes does not contain file's extension
                new object[]{null, " ", new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()), null},
                new object[]{null, string.Empty, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()), null},
            };
        }

        [Theory]
        [MemberData(nameof(ResolveEnableFileOperations_ReturnsTrueIfFileOperationsAreEnabled_Data))]
        public void ResolveEnableFileOperations_ReturnsTrueIfFileOperationsAreEnabled(bool dummyEnableFileOperations,
            string dummyLocalMediaDirectory,
            bool dummyGeneratePoster,
            string dummyPoster,
            double dummyWidth,
            double dummyHeight,
            double dummyDuration,
            bool expectedResult)
        {
            // Arrange
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory();

            // Act
            bool result = testSubject.ResolveEnableFileOperations(dummyEnableFileOperations,
                dummyLocalMediaDirectory,
                dummyGeneratePoster,
                dummyPoster,
                dummyWidth,
                dummyHeight,
                dummyDuration);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveEnableFileOperations_ReturnsTrueIfFileOperationsAreEnabled_Data()
        {
            return new object[][]
            {
                // False if EnableFileOperations is false
                new object[]{false, "dummyLocalMediaDirectory", true, null, 0, 0, 0, false},
                // False if LocalMediaDirectory is null, whitespace or an empty string
                new object[]{true, null, true, null, 0, 0, 0, false},
                new object[]{true, " ", true, null, 0, 0, 0, false},
                new object[]{true, string.Empty, true, null, 0, 0, 0, false},
                // False if metadata are specified and poster is specified or we don't need to generate a poster
                new object[]{true, "dummyLocalMediaDirectory", true, "dummyPoster", 123, 321, 456, false},
                new object[]{true, "dummyLocalMediaDirectory", false, "dummyPoster", 123, 321, 456, false},
                new object[]{true, "dummyLocalMediaDirectory", false, null, 123, 321, 456, false},
                new object[]{true, "dummyLocalMediaDirectory", false, " ", 123, 321, 456, false},
                new object[]{true, "dummyLocalMediaDirectory", false, string.Empty, 123, 321, 456, false},
                // True otherwise
                new object[]{true, "dummyLocalMediaDirectory", true, null, 123, 321, 456, true}, // Need to generate poster
                new object[]{true, "dummyLocalMediaDirectory", true, " ", 123, 321, 456, true}, // Need to generate poster
                new object[]{true, "dummyLocalMediaDirectory", true, string.Empty, 123, 321, 456, true}, // Need to generate poster
                new object[]{true, "dummyLocalMediaDirectory", true, "dummyPoster", 0, 321, 456, true}, // Need to generate width
                new object[]{true, "dummyLocalMediaDirectory", true, "dummyPoster", 123, 0, 456, true}, // Need to generate height
                new object[]{true, "dummyLocalMediaDirectory", true, "dummyPoster", 123, 321, 0, true} // Need to generate duration

            };
        }

        [Theory]
        [MemberData(nameof(ResolveDimensionsAndDuration_ResolvesDimensionsWithoutFileOperationsWhenPossibleOrIfFileOperationsAreDisabled_Data))]
        public void ResolveDimensionsAndDuration_ResolvesDimensionsWithoutFileOperationsWhenPossibleOrIfFileOperationsAreDisabled(double dummySpecifiedWidth,
            double dummySpecifiedHeight,
            double dummySpecifiedDuration,
            string dummyLocalAbsolutePath,
            double expectedWidth,
            double expectedHeight,
            double expectedAspectRatio,
            double expectedDuration)
        {
            // Arrange
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory();

            // Act
            (double resultWidth, double resultHeight, double resultAspectRatio, double resultDuration) = testSubject.ResolveDimensionsAndDuration(dummyLocalAbsolutePath, dummySpecifiedWidth, dummySpecifiedHeight, dummySpecifiedDuration);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedWidth, resultWidth);
            Assert.Equal(expectedHeight, resultHeight);
            Assert.Equal(expectedAspectRatio, resultAspectRatio);
            Assert.Equal(expectedDuration, resultDuration);
        }

        public static IEnumerable<object[]> ResolveDimensionsAndDuration_ResolvesDimensionsWithoutFileOperationsWhenPossibleOrIfFileOperationsAreDisabled_Data()
        {
            const double dummySpecifiedWidth = 543;
            const double dummySpecifiedHeight = 897;
            const double dummySpecifiedDuration = 123;

            return new object[][]
            {
                // Metadata specified
                new object[]{
                    dummySpecifiedWidth,
                    dummySpecifiedHeight,
                    dummySpecifiedDuration,
                    "dummyLocalAbsoluteData",
                    dummySpecifiedWidth,
                    dummySpecifiedHeight,
                    dummySpecifiedHeight / dummySpecifiedWidth * 100,
                    dummySpecifiedDuration
                },
                // Width/height permutations, file operations disabled
                new object[]{dummySpecifiedWidth, 0, 0, null, dummySpecifiedWidth, 0, 0, 0},
                new object[]{dummySpecifiedWidth, dummySpecifiedHeight, 0, null, dummySpecifiedWidth, dummySpecifiedHeight, dummySpecifiedHeight / dummySpecifiedWidth * 100, 0},
                new object[]{0, dummySpecifiedHeight, 0, null, 0, dummySpecifiedHeight, 0, 0},
                new object[]{0, 0, 0, null, 0, 0, 0, 0},
                // Duration specified (unspecified case already verified in other tests), file operations disabled
                new object[]{0, 0, dummySpecifiedDuration, null, 0, 0, 0, dummySpecifiedDuration}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveDimensionsAndDuration_ResolvesDimensionsUsingFileOperationsWhenNecessaryAndFileOperationsAreEnabled_Data))]
        public void ResolveDimensionsAndDuration_ResolvesDimensionsUsingFileOperationsWhenNecessaryAndFileOperationsAreEnabled(double dummySpecifiedWidth,
            double dummySpecifiedHeight,
            double dummySpecifiedDuration,
            double dummyRetrievedWidth,
            double dummyRetrievedHeight,
            double dummyRetrievedDuration,
            double expectedWidth,
            double expectedHeight,
            double expectedDuration,
            double expectedAspectRatio)
        {
            // Arrange
            const string dummyLocalAbsolutePath = "dummyLocalAbsolutePath";
            Mock<IVideoService> mockVideoService = _mockRepository.Create<IVideoService>();
            mockVideoService.Setup(i => i.GetVideoDimensionsAndDuration(dummyLocalAbsolutePath)).Returns((dummyRetrievedWidth, dummyRetrievedHeight, dummyRetrievedDuration));
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory(mockVideoService.Object);

            // Act
            (double resultWidth, double resultHeight, double resultAspectRatio, double resultDuration) = testSubject.
                ResolveDimensionsAndDuration(dummyLocalAbsolutePath, dummySpecifiedWidth, dummySpecifiedHeight, dummySpecifiedDuration);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedWidth, resultWidth);
            Assert.Equal(expectedHeight, resultHeight);
            Assert.Equal(expectedAspectRatio, resultAspectRatio);
            Assert.Equal(expectedDuration, resultDuration);
        }

        public static IEnumerable<object[]> ResolveDimensionsAndDuration_ResolvesDimensionsUsingFileOperationsWhenNecessaryAndFileOperationsAreEnabled_Data()
        {
            const double dummySpecifiedWidth = 543;
            const double dummySpecifiedHeight = 897;
            const double dummySpecifiedDuration = 123;
            const double dummyRetrievedWidth = 563;
            const double dummyRetrievedHeight = 123;
            const double dummyRetrievedDuration = 321;

            // Specified values take precedence
            return new object[][]
            {
                // Width/height permutations
                new object[]{
                    dummySpecifiedWidth,
                    0,
                    0,
                    dummyRetrievedWidth,
                    dummyRetrievedHeight,
                    0,
                    dummySpecifiedWidth,
                    dummyRetrievedHeight,
                    0,
                    dummyRetrievedHeight / dummySpecifiedWidth * 100
                },
                new object[]{
                    0,
                    dummySpecifiedHeight,
                    0,
                    dummyRetrievedWidth,
                    dummyRetrievedHeight,
                    0,
                    dummyRetrievedWidth,
                    dummySpecifiedHeight,
                    0,
                    dummySpecifiedHeight / dummyRetrievedWidth * 100
                },
                new object[]{
                    dummySpecifiedWidth,
                    dummySpecifiedHeight,
                    0,
                    dummyRetrievedWidth,
                    dummyRetrievedHeight,
                    0,
                    dummySpecifiedWidth,
                    dummySpecifiedHeight,
                    0,
                    dummySpecifiedHeight / dummySpecifiedWidth * 100
                },
                new object[]{
                    0,
                    0,
                    0,
                    dummyRetrievedWidth,
                    dummyRetrievedHeight,
                    0,
                    dummyRetrievedWidth,
                    dummyRetrievedHeight,
                    0,
                    dummyRetrievedHeight / dummyRetrievedWidth * 100
                },
                // If video is wierd or metadata is corrupted, retrieved dimensions may be 0. We can't differentiate between wierdness and messed up metadata so we just 
                // avoid divide by 0 exceptions
                new object[]{0, 0, 0, dummyRetrievedWidth, 0, 0, dummyRetrievedWidth, 0, 0, 0 },
                new object[]{0, 0, 0, 0, dummyRetrievedHeight, 0, 0, dummyRetrievedHeight, 0, 0 },
                new object[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                // Duration
                new object[]{0, 0, dummySpecifiedDuration, 0, 0, dummyRetrievedDuration, 0, 0, dummySpecifiedDuration, 0 },
                new object[]{0, 0, 0, 0, 0, dummyRetrievedDuration, 0, 0, dummyRetrievedDuration, 0 }
            };
        }

        [Theory]
        [MemberData(nameof(ResolvePoster_ReturnsPosterIfPosterIsSpecifiedAndNullIfPosterGenerationIsDisabled_Data))]
        public void ResolvePoster_ResolvesPosterWithoutGeneratingPosterIfPossible(string dummyPoster,
            bool dummyGeneratePoster,
            string dummyLocalAbsolutePath,
            string expectedResult)
        {
            // Arrange
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory();

            // Act
            string result = testSubject.ResolvePoster(dummyLocalAbsolutePath, null, dummyPoster, dummyGeneratePoster);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolvePoster_ReturnsPosterIfPosterIsSpecifiedAndNullIfPosterGenerationIsDisabled_Data()
        {
            const string dummyPoster = "dummyPoster";

            return new object[][]
            {
                // Poster specified
                new object[]{ dummyPoster, false, null, dummyPoster },
                // Generate poster false
                new object[]{ null, false, "dummyLocalAbsolutePath", null },
                // File operations disabled (no local absolute path)
                new object[]{ null, true, null, null }
            };
        }

        [Theory]
        [MemberData(nameof(ResolvePoster_GeneratesPoster_Data))]
        public void ResolvePoster_GeneratesPoster(string dummyLocalAbsolutePath, string dummyPosterLocalAbsolutePath, string dummySrc, string expectedResult)
        {
            // Arrange
            FlexiVideoBlockFactory testSubject = CreateFlexiVideoBlockFactory();
            Mock<IVideoService> mockVideoService = _mockRepository.Create<IVideoService>();
            mockVideoService.Setup(v => v.GeneratePoster(dummyLocalAbsolutePath, dummyPosterLocalAbsolutePath));

            // Act
            string result = testSubject.ResolvePoster(dummyLocalAbsolutePath, dummySrc, null, true);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolvePoster_GeneratesPoster_Data()
        {
            return new object[][]
            {
                new object[]{
                    "file:///host/dummy/local/path/file.mp4",
                    "file:///host/dummy/local/path/file_poster.mp4",
                    "https://host/dummy/src/file.mp4",
                    "https://host/dummy/src/file_poster.png"
                },
                new object[]{
                    "file:///host/dummy/local/path/file",
                    "file:///host/dummy/local/path/file_poster.png",
                    "https://host/dummy/src/file",
                    "https://host/dummy/src/file_poster.png"
                }
            };
        }

        private Mock<FlexiVideoBlockFactory> CreateMockFlexiVideoBlockFactory(IVideoService videoService = null,
            IDirectoryService directoryService = null,
            IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions> optionsService = null)
        {
            return _mockRepository.Create<FlexiVideoBlockFactory>(videoService ?? _mockRepository.Create<IVideoService>().Object,
                directoryService ?? _mockRepository.Create<IDirectoryService>().Object,
                optionsService ?? _mockRepository.Create<IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions>>().Object);
        }

        private FlexiVideoBlockFactory CreateFlexiVideoBlockFactory(IVideoService videoService = null,
            IDirectoryService directoryService = null,
            IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions> optionsService = null)
        {
            return new FlexiVideoBlockFactory(videoService ?? _mockRepository.Create<IVideoService>().Object,
                directoryService ?? _mockRepository.Create<IDirectoryService>().Object,
                optionsService ?? _mockRepository.Create<IOptionsService<IFlexiVideoBlockOptions, IFlexiVideoBlocksExtensionOptions>>().Object);
        }
    }
}
