using Jering.IocServices.System.IO;
using Jering.Markdig.Extensions.FlexiBlocks.ContextObjects;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class FlexiIncludeBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfContextObjectsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiIncludeBlockFactory(null,
                _mockRepository.Create<IDirectoryService>().Object,
                _mockRepository.Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>().Object,
                _mockRepository.Create<IContentRetrieverService>().Object,
                _mockRepository.Create<ILeadingWhitespaceEditorService>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfDirectoryServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiIncludeBlockFactory(_mockRepository.Create<IContextObjectsService>().Object,
                null,
                _mockRepository.Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>().Object,
                _mockRepository.Create<IContentRetrieverService>().Object,
                _mockRepository.Create<ILeadingWhitespaceEditorService>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfOptionsServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiIncludeBlockFactory(_mockRepository.Create<IContextObjectsService>().Object,
                _mockRepository.Create<IDirectoryService>().Object,
                null,
                _mockRepository.Create<IContentRetrieverService>().Object,
                _mockRepository.Create<ILeadingWhitespaceEditorService>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfContentRetrieverServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiIncludeBlockFactory(_mockRepository.Create<IContextObjectsService>().Object,
                _mockRepository.Create<IDirectoryService>().Object,
                _mockRepository.Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>().Object,
                null,
                _mockRepository.Create<ILeadingWhitespaceEditorService>().Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfLeadingWhitespaceEditorServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiIncludeBlockFactory(_mockRepository.Create<IContextObjectsService>().Object,
                _mockRepository.Create<IDirectoryService>().Object,
                _mockRepository.Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>().Object,
                _mockRepository.Create<IContentRetrieverService>().Object,
                null));
        }

        [Fact]
        public void CreateProxyJsonBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

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
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            ProxyJsonBlock result = testSubject.CreateProxyJsonBlock(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            Assert.Equal(result.Column, dummyColumn);
            Assert.Equal(result.Span.Start, dummyLineStart);
            Assert.Equal(nameof(FlexiIncludeBlock), result.MainTypeName);
            Assert.Same(dummyBlockParser.Object, result.Parser);
        }

        [Fact]
        public void Create_CreatesFlexiIncludeBlock()
        {
            // Arrange
            const int dummyColumn = 3;
            const int dummyLine = 5;
            var dummySpan = new SourceSpan(1, 5);
            const string dummySource = "dummySource";
            const FlexiIncludeType dummyType = FlexiIncludeType.Markdown;
            const bool dummyCache = false;
            const string dummyCacheDirectory = "dummyCacheDirectory";
            const string dummyResolvedCacheDirectory = "dummyResolvedCacheDirectory";
            const string dummyContainingSource = "dummyContainingSource";
            const string dummyBaseUri = "dummyBaseUri";
            var dummySourceAbsoluteUri = new Uri("C:/test");
            FlexiIncludeBlock dummyParent = CreateFlexiIncludeBlock();
            Mock<BlockParser> dummyBlockParser = _mockRepository.Create<BlockParser>();
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, dummyBlockParser.Object)
            {
                Line = dummyLine,
                Column = dummyColumn,
                Span = dummySpan
            };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyClippings = new ReadOnlyCollection<Clipping>(new List<Clipping>());
            Mock<IFlexiIncludeBlockOptions> mockFlexiIncludeBlockOptions = _mockRepository.Create<IFlexiIncludeBlockOptions>();
            mockFlexiIncludeBlockOptions.Setup(i => i.Source).Returns(dummySource);
            mockFlexiIncludeBlockOptions.Setup(i => i.Type).Returns(dummyType);
            mockFlexiIncludeBlockOptions.Setup(i => i.Cache).Returns(dummyCache);
            mockFlexiIncludeBlockOptions.Setup(i => i.CacheDirectory).Returns(dummyCacheDirectory);
            mockFlexiIncludeBlockOptions.Setup(i => i.Clippings).Returns(dummyClippings);
            Mock<IFlexiIncludeBlocksExtensionOptions> mockFlexiIncludeBlocksExtensionOptions = _mockRepository.Create<IFlexiIncludeBlocksExtensionOptions>();
            mockFlexiIncludeBlocksExtensionOptions.Setup(i => i.BaseUri).Returns(dummyBaseUri);
            Mock<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>> mockOptionsService = _mockRepository.
                Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>();
            mockOptionsService.Setup(o => o.CreateOptions(dummyBlockProcessor, dummyProxyJsonBlock)).Returns((mockFlexiIncludeBlockOptions.Object, mockFlexiIncludeBlocksExtensionOptions.Object));
            Mock<FlexiIncludeBlockFactory> mockTestSubject = CreateMockFlexiIncludeBlockFactory(optionsService: mockOptionsService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ValidateSource(dummySource));
            mockTestSubject.Setup(t => t.ValidateType(dummyType));
            mockTestSubject.Setup(t => t.ResolveAndValidateCacheDirectory(dummyCache, dummyCacheDirectory)).Returns(dummyResolvedCacheDirectory);
            mockTestSubject.Setup(t => t.ResolveParent(dummyBlockProcessor)).Returns(dummyParent);
            mockTestSubject.Setup(t => t.ResolveContainingSource(dummyParent)).Returns(dummyContainingSource);
            mockTestSubject.Setup(t => t.ResolveSourceAbsoluteUri(dummySource, dummyBaseUri, dummyParent)).Returns(dummySourceAbsoluteUri);
            mockTestSubject.Setup(t => t.ProcessFlexiIncludeBlock(It.Is<FlexiIncludeBlock>(flexiIncludeBlock =>
                    flexiIncludeBlock.Source == dummySourceAbsoluteUri &&
                    flexiIncludeBlock.Clippings == dummyClippings &&
                    flexiIncludeBlock.Type == dummyType &&
                    flexiIncludeBlock.CacheDirectory == dummyResolvedCacheDirectory &&
                    flexiIncludeBlock.ParentFlexiIncludeBlock == dummyParent &&
                    flexiIncludeBlock.ContainingSource == dummyContainingSource &&
                    flexiIncludeBlock.Parser == dummyBlockParser.Object &&
                    flexiIncludeBlock.ParentFlexiIncludeBlock.Children[0] == flexiIncludeBlock &&
                    flexiIncludeBlock.Column == dummyColumn &&
                    flexiIncludeBlock.Span == dummySpan &&
                    flexiIncludeBlock.Line == dummyLine),
                dummyProxyJsonBlock,
                dummyBlockProcessor));

            // Act
            mockTestSubject.Object.Create(dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ValidateSource_ThrowsOptionsExceptionIfSourceIsNull()
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateSource(null));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption, nameof(IFlexiIncludeBlockOptions.Source),
                    Strings.OptionsException_Shared_ValueMustNotBeNull),
                result.Message);
        }

        [Fact]
        public void ValidateType_ThrowsOptionsExceptionIfTypeIsNotAValidEnumValue()
        {
            // Arrange
            const FlexiIncludeType dummyType = (FlexiIncludeType)98;
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ValidateType(dummyType));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption, nameof(IFlexiIncludeBlockOptions.Type),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAValidEnumValue, dummyType, nameof(FlexiIncludeType))),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(ResolveAndValidateCacheDirectory_ResolvesCacheDirectory_Data))]
        public void ResolveAndValidateCacheDirectory_ResolvesCacheDirectory(bool dummyCache, string dummyCacheDirectory, string expectedResult)
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            string result = testSubject.ResolveAndValidateCacheDirectory(dummyCache, dummyCacheDirectory);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveAndValidateCacheDirectory_ResolvesCacheDirectory_Data()
        {
            string expectedDefaultCacheDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ContentCache"); // TODO sure current directory doesn't change?

            return new object[][]
            {
                // Cache == false
                new object[]{false, null, null},
                // CacheDirectory == null, whitespace or an empty string
                new object[]{true, null, expectedDefaultCacheDirectory},
                new object[]{true, " ", expectedDefaultCacheDirectory},
                new object[]{true, string.Empty, expectedDefaultCacheDirectory},
            };
        }

        [Fact]
        public void ResolveAndValidateCacheDirectory_ValidatesThatCacheDirectoryExistsIfItsUserSpecified()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.Setup(d => d.Exists(dummyCacheDirectory)).Returns(true);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(directoryService: mockDirectoryService.Object);

            // Act
            string result = testSubject.ResolveAndValidateCacheDirectory(true, dummyCacheDirectory);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCacheDirectory, result);
        }

        [Fact]
        public void ResolveAndValidateCacheDirectory_ThrowsOptionsExceptionIfUseSpecifiedCacheDirectoryDoesNotExist()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.Setup(d => d.Exists(dummyCacheDirectory)).Returns(false);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(directoryService: mockDirectoryService.Object);

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ResolveAndValidateCacheDirectory(true, dummyCacheDirectory));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption, nameof(IFlexiIncludeBlockOptions.CacheDirectory),
                    string.Format(Strings.OptionsException_Shared_DirectoryDoesNotExist, dummyCacheDirectory)),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(ResolveParent_ResolvesParent_Data))]
        public void ResolveParent_ResolvesParent(Stack<FlexiIncludeBlock> dummyClosingFlexiIncludeBlocks, FlexiIncludeBlock expectedResult)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<FlexiIncludeBlockFactory> testSubject = CreateMockFlexiIncludeBlockFactory();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor)).Returns(dummyClosingFlexiIncludeBlocks);

            // Act
            FlexiIncludeBlock result = testSubject.Object.ResolveParent(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveParent_ResolvesParent_Data()
        {
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock();
            FlexiIncludeBlock dummyParent = CreateFlexiIncludeBlock();

            return new object[][]
            {
                // No closing FlexiIncludeBlocks
                new object[]{new Stack<FlexiIncludeBlock>(), null},
                // Multiple closing FlexiIncludeBlocks, returns top of stack
                new object[]{new Stack<FlexiIncludeBlock>(new List<FlexiIncludeBlock> { dummyFlexiIncludeBlock, dummyParent }), dummyParent}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveContainingSource_ResolvesContainingSource_Data))]
        public void ResolveContainingSource_ResolvesContainingSource(FlexiIncludeBlock dummyParent, string expectedResult)
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            string result = testSubject.ResolveContainingSource(dummyParent);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ResolveContainingSource_ResolvesContainingSource_Data()
        {
            var dummySource = new Uri("C:/example/path.txt");

            return new object[][]
            {
                // No parent
                new object[]{null, null},
                // Has parent
                new object[]{CreateFlexiIncludeBlock(source: dummySource), dummySource.AbsoluteUri}
            };
        }

        [Theory]
        [MemberData(nameof(ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfSourceSchemeIsUnsupported_Data))]
        public void ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfSourceSchemeIsUnsupported(string dummySource, string dummyScheme)
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ResolveSourceAbsoluteUri(dummySource, default, default));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiIncludeBlockOptions.Source),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAUriWithASupportedScheme,
                        dummySource,
                        dummyScheme,
                        "FILE, HTTP or HTTPS")),
                result.Message);
        }

        public static IEnumerable<object[]> ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfSourceSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                        new object[]{ "ftp://base/uri", "ftp" },
                        new object[]{ "mailto:base@uri.com", "mailto" },
                        new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfRootBaseUriIsNotNullOrAnAbsoluteUri_Data))]
        public void ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfRootBaseUriIsNotNullOrAnAbsoluteUri(string dummyRootBaseUri)
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ResolveSourceAbsoluteUri("relative/uri", dummyRootBaseUri, null));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiIncludeBlocksExtensionOptions.BaseUri),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAnAbsoluteUri,
                        dummyRootBaseUri)),
                result.Message);
        }

        public static IEnumerable<object[]> ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfRootBaseUriIsNotNullOrAnAbsoluteUri_Data()
        {
            return new object[][]
            {
                // Common relative (non absolute) URIs, see http://www.ietf.org/rfc/rfc3986.txt, section 5.4.1
                // Note: "/relative/uri" is considered a relative URI on Windows but it is considered an absolute URI on 
                // Linux/macOS, so we can't include a test for it.
                new object[]{ "./relative/uri" },
                new object[]{ "../relative/uri" },
                new object[]{ "relative/uri"  }
            };
        }

        [Theory]
        [MemberData(nameof(ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfRootBaseUriSchemeIsUnsupported_Data))]
        public void ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfRootBaseUriSchemeIsUnsupported(string dummyRootBaseUri, string expectedScheme)
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.
                Throws<OptionsException>(() => testSubject.ResolveSourceAbsoluteUri("relative/uri", dummyRootBaseUri, null));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IFlexiIncludeBlocksExtensionOptions.BaseUri),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAUriWithASupportedScheme,
                        dummyRootBaseUri,
                        expectedScheme,
                        "FILE, HTTP or HTTPS")),
                result.Message);
        }

        public static IEnumerable<object[]> ResolveSourceAbsoluteUri_ThrowsOptionsExceptionIfRootBaseUriSchemeIsUnsupported_Data()
        {
            return new object[][]
            {
                        new object[]{ "ftp://base/uri", "ftp" },
                        new object[]{ "mailto:base@uri.com", "mailto" },
                        new object[]{ "gopher://base.uri.com/", "gopher" }
            };
        }

        [Theory]
        [MemberData(nameof(ResolveSourceAbsoluteUri_ReturnsSourceAbsoluteUri_Data))]
        public void ResolveSourceAbsoluteUri_ReturnsSourceAbsoluteUri(string dummySource,
            string dummyRootBaseUri,
            FlexiIncludeBlock dummyParent,
            string expectedAbsoluteSource)
        {
            // Arrange
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            Uri result = testSubject.ResolveSourceAbsoluteUri(dummySource, dummyRootBaseUri, dummyParent);

            // Assert
            Assert.Equal(expectedAbsoluteSource, result.AbsoluteUri);
        }

        public static IEnumerable<object[]> ResolveSourceAbsoluteUri_ReturnsSourceAbsoluteUri_Data()
        {
            const string dummyAbsoluteSource = "C:/absolute/source/uri";
            var dummyAbsoluteSourceUri = new Uri("C:/absolute/source/uri");
            const string dummyRelativeSource = "../../../relative/source/uri";
            var dummyCombinedUri = new Uri(dummyAbsoluteSourceUri, dummyRelativeSource);

            return new object[][]
            {
                // Absolute source
                new object[]
                {
                    dummyAbsoluteSource,
                    null,
                    null,
                    dummyAbsoluteSourceUri.AbsoluteUri
                },
                // Relative source with parent
                new object[]
                {
                    dummyRelativeSource,
                    null,
                    CreateFlexiIncludeBlock(dummyAbsoluteSourceUri),
                    dummyCombinedUri.AbsoluteUri
                },
                // Relative source with rootBaseUri and no parent
                new object[]
                {
                    dummyRelativeSource,
                    dummyAbsoluteSource,
                    null,
                    dummyCombinedUri.AbsoluteUri
                },
                // Relative source with no rootBaseUri and no parent
                new object[]
                {
                    dummyRelativeSource,
                    null,
                    null,
                    new Uri(new Uri(Directory.GetCurrentDirectory() + "/"), dummyRelativeSource).AbsoluteUri
                }
            };
        }

        [Fact]
        public void ProcessFlexiIncludeBlock_IfFlexiIncludeTypeIsMarkdownChecksForCycleGetsContentAndProcessesIt()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            dummyParentOfNewBlocks.Object.Add(dummyProxyJsonBlock);
            var dummySource = new Uri("C:/dummy/source.txt"); // This constructor requires an absolute URI - https://docs.microsoft.com/en-us/dotnet/api/system.uri.-ctor?view=netframework-4.8#overloads
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(dummySource, type: FlexiIncludeType.Markdown, cacheDirectory: dummyCacheDirectory);
            var dummyClosingFlexiIncludeBlocks = new Stack<FlexiIncludeBlock>(new FlexiIncludeBlock[0]);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyContent = new ReadOnlyCollection<string>(new List<string>());
            Mock<IContentRetrieverService> mockContentRetrieverService = _mockRepository.Create<IContentRetrieverService>();
            mockContentRetrieverService.
                Setup(s => s.GetContent(dummySource, dummyCacheDirectory, default)).
                Returns(dummyContent);
            Mock<FlexiIncludeBlockFactory> mockTestSubject = CreateMockFlexiIncludeBlockFactory(contentRetrieverService: mockContentRetrieverService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor)).Returns(dummyClosingFlexiIncludeBlocks);
            mockTestSubject.Setup(t => t.CheckForCycle(dummyClosingFlexiIncludeBlocks, dummyFlexiIncludeBlock));
            mockTestSubject.Setup(t => t.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, dummyContent));
            mockTestSubject.Setup(t => t.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor));

            // Act
            mockTestSubject.Object.ProcessFlexiIncludeBlock(dummyFlexiIncludeBlock, dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Empty(dummyClosingFlexiIncludeBlocks); // Removed once the block is replaced
            Assert.Empty(dummyParentOfNewBlocks.Object); // ProxyJsonBlock gets removed
        }

        [Fact]
        public void ProcessFlexiIncludeBlock_IfFlexiIncludeTypeIsCodeGetsContentAndProcessesIt()
        {
            // Arrange
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            dummyParentOfNewBlocks.Object.Add(dummyProxyJsonBlock);
            var dummySource = new Uri("C:/dummy/source.txt"); // This constructor requires an absolute URI - https://docs.microsoft.com/en-us/dotnet/api/system.uri.-ctor?view=netframework-4.8#overloads
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(dummySource, type: FlexiIncludeType.Code, cacheDirectory: dummyCacheDirectory);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyContent = new ReadOnlyCollection<string>(new List<string>());
            Mock<IContentRetrieverService> mockContentRetrieverService = _mockRepository.Create<IContentRetrieverService>();
            mockContentRetrieverService.
                Setup(s => s.GetContent(dummySource, dummyCacheDirectory, default)).
                Returns(dummyContent);
            Mock<FlexiIncludeBlockFactory> mockTestSubject = CreateMockFlexiIncludeBlockFactory(contentRetrieverService: mockContentRetrieverService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, dummyContent));
            mockTestSubject.Setup(t => t.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor));

            // Act
            mockTestSubject.Object.ProcessFlexiIncludeBlock(dummyFlexiIncludeBlock, dummyProxyJsonBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Empty(dummyParentOfNewBlocks.Object); // ProxyJsonBlock gets removed
        }

        [Fact]
        public void ProcessFlexiIncludeBlock_WrapsExceptionsInBlockExceptionsForCompleteContext()
        {
            // Arrange
            const int dummyLine = 6;
            const int dummyColumn = 23;
            const string dummyCacheDirectory = "dummyCacheDirectory";
            var dummyProxyJsonBlock = new ProxyJsonBlock(null, null);
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            dummyParentOfNewBlocks.Object.Add(dummyProxyJsonBlock);
            var dummySource = new Uri("C:/dummy/source.txt"); // This constructor requires an absolute URI - https://docs.microsoft.com/en-us/dotnet/api/system.uri.-ctor?view=netframework-4.8#overloads
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(dummySource, type: FlexiIncludeType.Code, cacheDirectory: dummyCacheDirectory);
            dummyFlexiIncludeBlock.Line = dummyLine;
            dummyFlexiIncludeBlock.Column = dummyColumn;
            var dummyContent = new ReadOnlyCollection<string>(new List<string>());
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<IContentRetrieverService> mockContentRetrieverService = _mockRepository.Create<IContentRetrieverService>();
            mockContentRetrieverService.Setup(s => s.GetContent(dummySource, dummyCacheDirectory, default)).Returns(dummyContent);
            var dummyException = new BlockException();
            Mock<FlexiIncludeBlockFactory> mockTestSubject = CreateMockFlexiIncludeBlockFactory(contentRetrieverService: mockContentRetrieverService.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, dummyContent)).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ProcessFlexiIncludeBlock(dummyFlexiIncludeBlock, dummyProxyJsonBlock, dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Same(dummyException, result.InnerException);
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock, nameof(FlexiIncludeBlock), dummyLine + 1, dummyColumn,
                    string.Format(Strings.BlockException_FlexiIncludeBlockFactory_ExceptionOccurredWhileProcessingContent, dummySource.AbsoluteUri)),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void GetOrCreateClosingFlexiIncludeBlocks_GetsClosingFlexiIncludeBlockIfItAlreadyExists()
        {
            // Arrange
            var dummyClosingFlexiIncludeBlocks = new Stack<FlexiIncludeBlock>();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiIncludeBlockFactory.CLOSING_FLEXI_INCLUDE_BLOCKS_KEY, dummyClosingFlexiIncludeBlocks);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            Stack<FlexiIncludeBlock> result = testSubject.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor);

            // Assert
            Assert.Same(dummyClosingFlexiIncludeBlocks, result);
        }

        [Fact]
        public void GetOrCreateClosingFlexiIncludeBlocks_CreatesClosingFlexiIncludeBlocksIfAnObjectWithTheWrongTypeIsFound()
        {
            // Arrange
            var dummyObject = new object();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Document.SetData(FlexiIncludeBlockFactory.CLOSING_FLEXI_INCLUDE_BLOCKS_KEY, dummyObject);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            Stack<FlexiIncludeBlock> result = testSubject.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetOrCreateClosingFlexiIncludeBlocks_CreatesClosingFlexiIncludeBlocksIfItDoesntAlreadyExist()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            Stack<FlexiIncludeBlock> result = testSubject.GetOrCreateClosingFlexiIncludeBlocks(dummyBlockProcessor);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CheckForCycle_ThrowsInvalidOperationExceptionIfACycleIsFound()
        {
            // Arrange
            FlexiIncludeBlock dummyFlexiIncludeBlock1 = CreateFlexiIncludeBlock();
            dummyFlexiIncludeBlock1.Line = 32;
            FlexiIncludeBlock dummyFlexiIncludeBlock2 = CreateFlexiIncludeBlock(containingSource: "dummySource2");
            dummyFlexiIncludeBlock2.Line = 12;
            FlexiIncludeBlock dummyFlexiIncludeBlock3 = CreateFlexiIncludeBlock(containingSource: "dummySource3");
            dummyFlexiIncludeBlock3.Line = 5;
            var dummyClosingFlexiIncludeBlocks = new Stack<FlexiIncludeBlock>(new List<FlexiIncludeBlock> { dummyFlexiIncludeBlock1, dummyFlexiIncludeBlock2, dummyFlexiIncludeBlock3 });
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(containingSource: dummyFlexiIncludeBlock2.ContainingSource); // Same block as dummyFlexiIncludeBlock2
            dummyFlexiIncludeBlock.Line = dummyFlexiIncludeBlock2.Line;
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.CheckForCycle(dummyClosingFlexiIncludeBlocks, dummyFlexiIncludeBlock));
            Assert.Equal(string.Format(Strings.InvalidOperationException_FlexiIncludeBlockFactory_CycleFound,
                    $"Source: {dummyFlexiIncludeBlock2.ContainingSource}, Line Number: {dummyFlexiIncludeBlock2.Line + 1} >\n" +
                    $"Source: {dummyFlexiIncludeBlock3.ContainingSource}, Line Number: {dummyFlexiIncludeBlock3.Line + 1} >\n" +
                    $"Source: {dummyFlexiIncludeBlock.ContainingSource}, Line Number: {dummyFlexiIncludeBlock.Line + 1}"),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(ProcessContent_CreatesCodeBlockIfFlexiIncludeBlockTypeIsCode_Data))]
        public void ProcessContent_CreatesCodeBlockIfFlexiIncludeBlockTypeIsCode(FlexiIncludeBlock dummyFlexiIncludeBlock, string[] dummyContentArray, string expectedCode)
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(dummyContentArray);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            testSubject.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyBlockProcessor.Document, dummyContent);

            // Assert
            Assert.Single(dummyBlockProcessor.Document);
            var resultCodeBlock = dummyBlockProcessor.Document[0] as FencedCodeBlock;
            Assert.NotNull(resultCodeBlock);
            Assert.Equal(expectedCode, resultCodeBlock.Lines.ToString(), ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> ProcessContent_CreatesCodeBlockIfFlexiIncludeBlockTypeIsCode_Data()
        {
            var dummyContentArray = new string[] { "dummy", "content" };
            // Multiline before/after works
            const string dummyBefore = @"dummy
before";
            const string dummyAfter = @"dummy
after";

            return new object[][]
            {
                // Default clipping
                new object[]{
                    CreateFlexiIncludeBlock(type: FlexiIncludeType.Code),
                    dummyContentArray,
                    string.Join("\n", dummyContentArray)
                },
                // Clipping with non-null before and after
                new object[]
                {
                    CreateFlexiIncludeBlock(clippings: new ReadOnlyCollection<Clipping>(new List<Clipping>{ new Clipping(before: dummyBefore, after: dummyAfter) }), type: FlexiIncludeType.Code),
                    dummyContentArray,
                    $"{dummyBefore}\n{string.Join("\n", dummyContentArray)}\n{dummyAfter}"
                }
            };
        }

        [Fact]
        public void ProcessContent_CreatesBlocksIfFlexiIncludeBlockTypeIsMarkdown()
        {
            // Arrange
            const string dummyHeading = "dummyHeading";
            const string dummyParagraph = "dummyParagraph";
            const string dummyBlockquote = "dummyBlockquote";
            var dummyContent = new ReadOnlyCollection<string>(new string[]{
                "# dummyHeading",
                "dummyParagraph",
                "> dummyBlockquote"
            });
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            testSubject.ProcessContent(dummyBlockProcessor, CreateFlexiIncludeBlock(type: FlexiIncludeType.Markdown), dummyBlockProcessor.Document, dummyContent);

            // Assert
            Assert.Equal(3, dummyBlockProcessor.Document.Count);
            // HeadingBlock
            var resultHeadingBlock = dummyBlockProcessor.Document[0] as HeadingBlock;
            Assert.NotNull(resultHeadingBlock);
            Assert.Equal(dummyHeading, resultHeadingBlock.Lines.ToString());
            // ParagraphBlock
            var resultParagraphBlock = dummyBlockProcessor.Document[1] as ParagraphBlock;
            Assert.NotNull(resultParagraphBlock);
            Assert.Equal(dummyParagraph, resultParagraphBlock.Lines.ToString());
            // Blockquote
            var resultBlockquote = dummyBlockProcessor.Document[2] as QuoteBlock;
            Assert.NotNull(resultBlockquote);
            Assert.Equal(dummyBlockquote, (resultBlockquote[0] as ParagraphBlock)?.Lines.ToString());
        }

        [Fact]
        public void ProcessContent_ThrowsOptionsExceptionIfNoLineContainsStartStringOfAClipping()
        {
            // Arrange
            var dummyContent = new ReadOnlyCollection<string>(new string[] { "dummy", "content" });
            const string dummyStartString = "dummyStartString";
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(clippings: new ReadOnlyCollection<Clipping>(new List<Clipping> { new Clipping(startString: dummyStartString) }));
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, dummyContent));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(Clipping.StartString),
                    string.Format(Strings.OptionsException_FlexiIncludeBlockFactory_NoLineContainsStartString, dummyStartString)),
                result.Message);
        }

        [Fact]
        public void ProcessContent_ThrowsOptionsExceptionIfNoLineContainsEndStringOfAClipping()
        {
            var dummyContent = new ReadOnlyCollection<string>(new string[] { "dummy", "content" });
            const string dummyEndString = "dummyEndString";
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(clippings: new ReadOnlyCollection<Clipping>(new List<Clipping> { new Clipping(endString: dummyEndString) }));
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, dummyContent));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(Clipping.EndString),
                    string.Format(Strings.OptionsException_FlexiIncludeBlockFactory_NoLineContainsEndString, dummyEndString)),
                result.Message);
        }

        [Fact]
        public void ProcessContent_IndentsDedentsAndCollapsesLines()
        {
            const int dummyIndent = 6;
            const int dummyDedent = 7;
            const float dummyCollapse = 0.3f;
            const string dummyContent = "dummyContent";
            var dummyIndented = new StringSlice("dummyIndented");
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(clippings: new ReadOnlyCollection<Clipping>(new List<Clipping>
            {
                new Clipping(dedent: dummyDedent, indent: dummyIndent, collapse: dummyCollapse)
            }));
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<ILeadingWhitespaceEditorService> mockLeadingWhitespaceEditorService = _mockRepository.Create<ILeadingWhitespaceEditorService>();
            mockLeadingWhitespaceEditorService.Setup(l => l.Indent(It.Is<StringSlice>(slice => slice.ToString() == dummyContent), dummyIndent)).Returns(dummyIndented);
            mockLeadingWhitespaceEditorService.Setup(l => l.Dedent(ref dummyIndented, dummyDedent));
            mockLeadingWhitespaceEditorService.Setup(l => l.Collapse(ref dummyIndented, dummyCollapse));
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(leadingWhitespaceEditorService: mockLeadingWhitespaceEditorService.Object);

            // Act
            testSubject.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, new ReadOnlyCollection<string>(new string[] { dummyContent }));

            // Assert
            _mockRepository.VerifyAll();
            Assert.Single(dummyParentOfNewBlocks.Object);
            Assert.Equal(dummyIndented.ToString(), (dummyParentOfNewBlocks.Object[0] as LeafBlock)?.Lines.ToString());
        }

        [Theory]
        [MemberData(nameof(ProcessContent_ClipsContentAccordingToClippingStartAndEndNumbersAndStrings_Data))]
        public void ProcessContent_ClipsContentAccordingToClippingStartAndEndNumbersAndStrings(string[] dummyContent, Clipping[] dummyClippings, string[] expectedResult)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(clippings: new ReadOnlyCollection<Clipping>(dummyClippings));
            Mock<ContainerBlock> dummyParentOfNewBlocks = _mockRepository.Create<ContainerBlock>(null);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            testSubject.ProcessContent(dummyBlockProcessor, dummyFlexiIncludeBlock, dummyParentOfNewBlocks.Object, new ReadOnlyCollection<string>(dummyContent));

            // Assert
            Assert.Single(dummyParentOfNewBlocks.Object);
            Assert.Equal(string.Join("\n", expectedResult), (dummyParentOfNewBlocks.Object[0] as LeafBlock)?.Lines.ToString());
        }

        public static IEnumerable<object[]> ProcessContent_ClipsContentAccordingToClippingStartAndEndNumbersAndStrings_Data()
        {
            var defaultDummyContent = new string[] { "line1", "line2", "line3", "line4", "line5" };
            const string dummyRegion = "dummyRegion";
            var dummyContentWithRegion = new string[] { $"#region {dummyRegion}", "line2", "line3", "#endregion" };

            return new object[][]
            {
                    // Line numbers - all lines
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(endLine: 5)},
                        new string[] { "line1", "line2", "line3", "line4", "line5" }
                    },
                    // Line numbers - all lines with -1 end
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping()},
                        new string[] { "line1", "line2", "line3", "line4", "line5" }
                    },
                    // Line numbers - single line
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(3, 3)},
                        new string[] { "line3" }
                    },
                    // Line numbers - subset of lines
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(2, 4)},
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Line numbers - subset of lines using negative start and end
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(-4, -2)},
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Strings - single line
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(startString: "line2", endString: "line4")},
                        new string[] { "line3" }
                    },
                    // Strings - subset of lines
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(startString: "line1", endString: "line5")},
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Region 
                    new object[]
                    {
                        dummyContentWithRegion,
                        new Clipping[] { new Clipping(region: dummyRegion)},
                        new string[] { "line2", "line3" }
                    },
                    // Region - with start string (startString takes precedence)
                    new object[]
                    {
                        dummyContentWithRegion,
                        new Clipping[] { new Clipping(startString: "line2", region: dummyRegion)},
                        new string[] { "line3" }
                    },
                    // Region - with end string (endString takes precedence)
                    new object[]
                    {
                        dummyContentWithRegion,
                        new Clipping[] { new Clipping(endString: "line3", region: dummyRegion) },
                        new string[] { "line2" }
                    },
                    // Line numbers and strings - single line, strings take precedence (start defaults to 1)
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(endLine: 5, startString: "line4")},
                        new string[] { "line5" }
                    },
                    // line numbers and strings - subset of lines, strings take precedence (end defaults to -1)
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] { new Clipping(2, endString: "line5")},
                        new string[] { "line2", "line3", "line4" }
                    },
                    // Multiple clippings - no overlap
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] {
                            new Clipping(endLine: 2),
                            new Clipping(startString: "line2", endString: "line5"),
                            new Clipping(endLine: 5, startString: "line4")
                        },
                        new string[] { "line1", "line2", "line3", "line4", "line5" }
                    },
                    // Multiple clippings - with overlap
                    new object[]
                    {
                        defaultDummyContent,
                        new Clipping[] {
                            new Clipping(endLine: 3),
                            new Clipping(startString: "line1", endString: "line5"),
                            new Clipping(endLine: 5, startString: "line3")
                        },
                        new string[] { "line1", "line2", "line3", "line2", "line3", "line4", "line4", "line5" }
                    },
            };
        }

        [Fact]
        public void TryAddToFlexiIncludeBlockTrees_DoesNothingIfFlexiIncludeBlockHasNoParentFlexiIncludeBlock()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock(parentFlexiIncludeBlock: CreateFlexiIncludeBlock());
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory();

            // Act
            testSubject.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void TryAddToFlexiIncludeBlockTrees_AddsFlexiIncludeBlockTreeToExistingFlexiIncludeBlockTrees()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock();
            var dummyFlexiIncludeBlockTrees = new List<FlexiIncludeBlock>();
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            object dummyOut = dummyFlexiIncludeBlockTrees;
            mockContextObjectsService.Setup(c => c.TryGetContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY, dummyBlockProcessor, out dummyOut)).Returns(true);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(contextObjectsService: mockContextObjectsService.Object);

            // Act
            testSubject.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor);

            // Assert
            Assert.Single(dummyFlexiIncludeBlockTrees);
            Assert.Same(dummyFlexiIncludeBlock, dummyFlexiIncludeBlockTrees[0]);
        }

        [Fact]
        public void TryAddToFlexiIncludeBlockTrees_CreatesFlexiIncludeBlockTreesIfItDoesntAlreadyExistAndAddsItToContextObjects()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock();
            List<FlexiIncludeBlock> resultFlexiIncludeBlockTrees = null;
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            object dummyOut = null;
            mockContextObjectsService.Setup(c => c.TryGetContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY, dummyBlockProcessor, out dummyOut)).Returns(false);
            mockContextObjectsService.
                Setup(c => c.TryAddContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY,
                    It.IsAny<List<FlexiIncludeBlock>>(),
                    dummyBlockProcessor)).
                Returns(true).
                Callback<object, object, BlockProcessor>((_, flexiIncludeBlockTrees, __) => resultFlexiIncludeBlockTrees = flexiIncludeBlockTrees as List<FlexiIncludeBlock>);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(contextObjectsService: mockContextObjectsService.Object);

            // Act
            testSubject.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor);

            // Assert
            Assert.NotNull(resultFlexiIncludeBlockTrees);
            Assert.Single(resultFlexiIncludeBlockTrees);
            Assert.Same(dummyFlexiIncludeBlock, resultFlexiIncludeBlockTrees[0]);
        }

        [Fact]
        public void TryAddToFlexiIncludeBlockTrees_CreatesFlexiIncludeBlockTreesIfOutObjectIsNotAnFlexiIncludeBlockListAndAddsFlexiIncludeBlockTreesToContextObjects()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock();
            List<FlexiIncludeBlock> resultFlexiIncludeBlockTrees = null;
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            var dummyOut = new object();
            mockContextObjectsService.Setup(c => c.TryGetContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY, dummyBlockProcessor, out dummyOut)).Returns(true);
            mockContextObjectsService.
                Setup(c => c.TryAddContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY,
                    It.IsAny<List<FlexiIncludeBlock>>(),
                    dummyBlockProcessor)).
                Returns(true).
                Callback<object, object, BlockProcessor>((_, flexiIncludeBlockTrees, __) => resultFlexiIncludeBlockTrees = flexiIncludeBlockTrees as List<FlexiIncludeBlock>);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(contextObjectsService: mockContextObjectsService.Object);

            // Act
            testSubject.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor);

            // Assert
            Assert.NotNull(resultFlexiIncludeBlockTrees);
            Assert.Single(resultFlexiIncludeBlockTrees);
            Assert.Same(dummyFlexiIncludeBlock, resultFlexiIncludeBlockTrees[0]);
        }

        [Fact]
        public void TryAddToFlexiIncludeBlockTrees_DoesNothingIfUnableToAddNewFlexiIncludeBlockTreesToContextObjects()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            FlexiIncludeBlock dummyFlexiIncludeBlock = CreateFlexiIncludeBlock();
            List<FlexiIncludeBlock> resultFlexiIncludeBlockTrees = null;
            Mock<IContextObjectsService> mockContextObjectsService = _mockRepository.Create<IContextObjectsService>();
            object dummyOut = null;
            mockContextObjectsService.Setup(c => c.TryGetContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY, dummyBlockProcessor, out dummyOut)).Returns(false);
            mockContextObjectsService.
                Setup(c => c.TryAddContextObject(FlexiIncludeBlockFactory.FLEXI_INCLUDE_BLOCK_TREES_KEY,
                    It.IsAny<List<FlexiIncludeBlock>>(),
                    dummyBlockProcessor)).
                Returns(false).
                Callback<object, object, BlockProcessor>((_, flexiIncludeBlockTrees, __) => resultFlexiIncludeBlockTrees = flexiIncludeBlockTrees as List<FlexiIncludeBlock>);
            FlexiIncludeBlockFactory testSubject = CreateFlexiIncludeBlockFactory(contextObjectsService: mockContextObjectsService.Object);

            // Act
            testSubject.TryAddToFlexiIncludeBlockTrees(dummyFlexiIncludeBlock, dummyBlockProcessor);

            // Assert
            Assert.NotNull(resultFlexiIncludeBlockTrees);
            Assert.Empty(resultFlexiIncludeBlockTrees);
        }

        private static FlexiIncludeBlock CreateFlexiIncludeBlock(Uri source = default,
        ReadOnlyCollection<Clipping> clippings = default,
        FlexiIncludeType type = default,
        string cacheDirectory = default,
        FlexiIncludeBlock parentFlexiIncludeBlock = default,
        string containingSource = default,
        BlockParser parser = default)
        {
            return new FlexiIncludeBlock(source, clippings, type, cacheDirectory, parentFlexiIncludeBlock, containingSource, parser);
        }

        private Mock<FlexiIncludeBlockFactory> CreateMockFlexiIncludeBlockFactory(IContextObjectsService contextObjectsService = null,
            IDirectoryService directoryService = null,
            IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions> optionsService = null,
            IContentRetrieverService contentRetrieverService = null,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService = null)
        {
            return _mockRepository.Create<FlexiIncludeBlockFactory>(contextObjectsService ?? _mockRepository.Create<IContextObjectsService>().Object,
                directoryService ?? _mockRepository.Create<IDirectoryService>().Object,
                optionsService ?? _mockRepository.Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>().Object,
                contentRetrieverService ?? _mockRepository.Create<IContentRetrieverService>().Object,
                leadingWhitespaceEditorService ?? _mockRepository.Create<ILeadingWhitespaceEditorService>().Object);
        }

        private FlexiIncludeBlockFactory CreateFlexiIncludeBlockFactory(IContextObjectsService contextObjectsService = null,
            IDirectoryService directoryService = null,
            IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions> optionsService = null,
            IContentRetrieverService contentRetrieverService = null,
            ILeadingWhitespaceEditorService leadingWhitespaceEditorService = null)
        {
            return new FlexiIncludeBlockFactory(contextObjectsService ?? _mockRepository.Create<IContextObjectsService>().Object,
                directoryService ?? _mockRepository.Create<IDirectoryService>().Object,
                optionsService ?? _mockRepository.Create<IOptionsService<IFlexiIncludeBlockOptions, IFlexiIncludeBlocksExtensionOptions>>().Object,
                contentRetrieverService ?? _mockRepository.Create<IContentRetrieverService>().Object,
                leadingWhitespaceEditorService ?? _mockRepository.Create<ILeadingWhitespaceEditorService>().Object);
        }
    }
}
