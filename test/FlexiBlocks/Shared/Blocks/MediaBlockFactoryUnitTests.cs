using Jering.IocServices.System.IO;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class MediaBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfDirectoryServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ExposedMediaBlockFactory(null));
        }

        [Fact]
        public void CreateProxyJsonBlock_ThrowsArgumentNullExceptionIfBlockProcessorIsNull()
        {
            // Arrange
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

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
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act
            ProxyJsonBlock result = testSubject.CreateProxyJsonBlock(dummyBlockProcessor, dummyBlockParser.Object);

            // Assert
            Assert.Equal(result.Column, dummyColumn);
            Assert.Equal(result.Span.Start, dummyLineStart);
            Assert.Equal(nameof(Block), result.MainTypeName);
            Assert.Same(dummyBlockParser.Object, result.Parser);
        }

        // TODO ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcContainsInvalidCharacters.
        // For some reason, contrary to documented behaviour, Path.GetFileName doesn't throw an ArgumentException even if path contains invalid characters like \0.

        [Theory]
        [MemberData(nameof(ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcIsNullWhitespaceOrAnEmptyString_Data))]
        public void ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcIsNullWhitespaceOrAnEmptyString(string dummySrc)
        {
            // Arrange
            Mock<IDummyMediaBlockOptions> mockMediaBlockOptions = _mockRepository.Create<IDummyMediaBlockOptions>();
            mockMediaBlockOptions.Setup(m => m.Src).Returns(dummySrc);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedValidateSrcAndResolveFileName(mockMediaBlockOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockOptions.Src),
                    Strings.OptionsException_Shared_ValueMustNotBeNullWhitespaceOrAnEmptyString),
                result.Message);
        }

        public static IEnumerable<object[]> ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcIsNullWhitespaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{" "},
                new object[]{string.Empty}
            };
        }

        [Theory]
        [MemberData(nameof(ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcDoesNotPointToAFile_Data))]
        public void ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcDoesNotPointToAFile(string dummySrc)
        {
            // Arrange
            Mock<IDummyMediaBlockOptions> mockMediaBlockOptions = _mockRepository.Create<IDummyMediaBlockOptions>();
            mockMediaBlockOptions.Setup(m => m.Src).Returns(dummySrc);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedValidateSrcAndResolveFileName(mockMediaBlockOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockOptions.Src),
                    string.Format(Strings.OptionsException_Shared_ValueMustPointToAFile, dummySrc)),
                result.Message);
        }

        public static IEnumerable<object[]> ValidateSrcAndResolveFileName_ThrowsOptionsExceptionIfSrcDoesNotPointToAFile_Data()
        {
            return new object[][]
            {
                new object[]{"./relative/uri/"}, // FILE scheme directory
                new object[]{"https://absolute.uri"}, // HTTPS scheme domain
                new object[]{"https://absolute.uri/test/"} // HTTPS scheme directory
            };
        }

        [Theory]
        [MemberData(nameof(ValidateSrcAndResolveFileName_ResolvesFileName_Data))]
        public void ValidateSrcAndResolveFileName_ResolvesFileName(string dummySrc, string expectedFileName)
        {
            // Arrange
            Mock<IDummyMediaBlockOptions> mockMediaBlockOptions = _mockRepository.Create<IDummyMediaBlockOptions>();
            mockMediaBlockOptions.Setup(m => m.Src).Returns(dummySrc);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act
            string result = testSubject.ExposedValidateSrcAndResolveFileName(mockMediaBlockOptions.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(expectedFileName, result);
        }

        public static IEnumerable<object[]> ValidateSrcAndResolveFileName_ResolvesFileName_Data()
        {
            const string dummyFileNameWithExtension = "file.ext";
            const string dummyFileNameWithoutExtension = "file";

            return new object[][]
            {
                // Common relative (non absolute) URIs, see http://www.ietf.org/rfc/rfc3986.txt, section 5.4.1
                // Note: "/relative/uri" is considered a relative URI on Windows but it is considered an absolute URI on Linux/macOS
                new object[]{ $"/relative/uri/{dummyFileNameWithExtension}", dummyFileNameWithExtension },
                new object[]{ $"./relative/uri/{dummyFileNameWithExtension}", dummyFileNameWithExtension },
                new object[]{ $"../../../../relative/uri/{dummyFileNameWithExtension}", dummyFileNameWithExtension },
                new object[]{ $"relative/uri/{dummyFileNameWithExtension}" , dummyFileNameWithExtension },
                // Absolute URIs
                new object[]{ $"file:///host/absolute/uri/{dummyFileNameWithExtension}", dummyFileNameWithExtension },
                new object[]{ $"http://absolute/uri/{dummyFileNameWithExtension}", dummyFileNameWithExtension },
                new object[]{ $"https://absolute/uri/{dummyFileNameWithExtension}", dummyFileNameWithExtension },
                // File name with no extension
                new object[]{ $"./relative/uri/{dummyFileNameWithoutExtension}", dummyFileNameWithoutExtension },
                new object[]{ $"https://absolute/uri/{dummyFileNameWithoutExtension}", dummyFileNameWithoutExtension },
                // URI with query paramters
                new object[]{ $"./relative/uri/{dummyFileNameWithExtension}?a=b&c=d", dummyFileNameWithExtension },
                new object[]{ $"https://absolute/uri/{dummyFileNameWithoutExtension}?a=b&c=d", dummyFileNameWithoutExtension },
            };
        }

        [Fact]
        public void ResolveLocalAbsolutePath_ReturnsNullIfEnableFileOperationsIsFalse()
        {
            // Arrange
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act
            string result = testSubject.ExposedResolveLocalAbsolutePath(false, null, null);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfLocalMediaDirectoryIsNotAnAbsoluteUri_Data))]
        public void ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfLocalMediaDirectoryIsNotAnAbsoluteUri(string dummyLocalMediaDirectory)
        {
            // Arrange
            Mock<IDummyMediaBlockExtensionOptions> mockMediaBlockExtensionOptions = _mockRepository.Create<IDummyMediaBlockExtensionOptions>();
            mockMediaBlockExtensionOptions.Setup(m => m.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedResolveLocalAbsolutePath(true,
                null,
                mockMediaBlockExtensionOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockExtensionOptions.LocalMediaDirectory),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAnAbsoluteUri, dummyLocalMediaDirectory)),
                result.Message);
        }

        public static IEnumerable<object[]> ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfLocalMediaDirectoryIsNotAnAbsoluteUri_Data()
        {
            return new object[][]
            {
                new object[]{ "./relative/uri" },
                new object[]{ "../relative/uri" },
                new object[]{ "relative/uri"  }
            };
        }

        [Theory]
        [MemberData(nameof(ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfLocalMediaDirectorySchemeIsNotFile_Data))]
        public void ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfLocalMediaDirectorySchemeIsNotFile(string dummyLocalMediaDirectory, string dummyScheme)
        {
            // Arrange
            Mock<IDummyMediaBlockExtensionOptions> mockMediaBlockExtensionOptions = _mockRepository.Create<IDummyMediaBlockExtensionOptions>();
            mockMediaBlockExtensionOptions.Setup(m => m.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory();

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedResolveLocalAbsolutePath(true,
                null,
                mockMediaBlockExtensionOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockExtensionOptions.LocalMediaDirectory),
                    string.Format(Strings.OptionsException_Shared_ValueMustBeAUriWithASupportedScheme, dummyLocalMediaDirectory, dummyScheme, "FILE")),
                result.Message);
        }

        public static IEnumerable<object[]> ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfLocalMediaDirectorySchemeIsNotFile_Data()
        {
            return new object[][]
            {
                new object[]{ "ftp://unsupported/scheme", "ftp" },
                new object[]{ "mailto:unsupported@scheme.com", "mailto" },
                new object[]{ "gopher://unsupported.scheme.com/", "gopher" },
                new object[]{ "https://unsupported.scheme.com/", "https" },
                new object[]{ "http://unsupported.scheme.com/", "http" },
            };
        }

        [Fact]
        public void ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfUnableToRetrieveFilesFromLocalMediaDirectory()
        {
            // Arrange
            const string dummyLocalMediaDirectory = "file:///host/dummy/local/images"; // /dummy/local/images isn't considered absolute on windows and c:/dummy/local/images isn't considered absolute on linux
            const string dummyFileName = "dummyFileName";
            Mock<IDummyMediaBlockExtensionOptions> mockMediaBlockExtensionOptions = _mockRepository.Create<IDummyMediaBlockExtensionOptions>();
            mockMediaBlockExtensionOptions.Setup(m => m.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            var dummyException = new Exception();
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.
                Setup(d => d.GetFiles(new Uri(dummyLocalMediaDirectory).AbsolutePath, dummyFileName, SearchOption.AllDirectories)).
                Throws(dummyException);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory(directoryService: mockDirectoryService.Object);

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedResolveLocalAbsolutePath(true,
                dummyFileName,
                mockMediaBlockExtensionOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyException, result.InnerException);
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockExtensionOptions.LocalMediaDirectory),
                    string.Format(Strings.OptionsException_Shared_UnableToRetrieveFilesFromDirectory, dummyLocalMediaDirectory)),
                result.Message);
        }

        [Fact]
        public void ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfFileIsNotFoundInLocalMediaDirectory()
        {
            // Arrange
            const string dummyLocalMediaDirectory = "file:///host/dummy/local/images"; // /dummy/local/images isn't considered absolute on windows and c:/dummy/local/images isn't considered absolute on linux
            const string dummyFileName = "dummyFileName";
            Mock<IDummyMediaBlockExtensionOptions> mockMediaBlockExtensionOptions = _mockRepository.Create<IDummyMediaBlockExtensionOptions>();
            mockMediaBlockExtensionOptions.Setup(m => m.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.
                Setup(d => d.GetFiles(new Uri(dummyLocalMediaDirectory).AbsolutePath, dummyFileName, SearchOption.AllDirectories)).
                Returns(new string[0]);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory(directoryService: mockDirectoryService.Object);

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedResolveLocalAbsolutePath(true,
                dummyFileName,
                mockMediaBlockExtensionOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockExtensionOptions.LocalMediaDirectory),
                    string.Format(Strings.OptionsException_Shared_FileNotFoundInDirectory, dummyFileName, dummyLocalMediaDirectory)),
                result.Message);
        }

        [Fact]
        public void ResolveLocalAbsolutePath_ThrowsOptionsExceptionIfMultipleFilesAreFoundInLocalMediaDirectory()
        {
            // Arrange
            const string dummyLocalMediaDirectory = "file:///host/dummy/local/images"; // /dummy/local/images isn't considered absolute on windows and c:/dummy/local/images isn't considered absolute on linux
            const string dummyFileName = "dummyFileName";
            var dummyRetrievedFiles = new string[] { "dummyFile1", "dummyFile2" };
            Mock<IDummyMediaBlockExtensionOptions> mockMediaBlockExtensionOptions = _mockRepository.Create<IDummyMediaBlockExtensionOptions>();
            mockMediaBlockExtensionOptions.Setup(m => m.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.
                Setup(d => d.GetFiles(new Uri(dummyLocalMediaDirectory).AbsolutePath, dummyFileName, SearchOption.AllDirectories)).
                Returns(dummyRetrievedFiles);
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory(directoryService: mockDirectoryService.Object);

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.ExposedResolveLocalAbsolutePath(true,
                dummyFileName,
                mockMediaBlockExtensionOptions.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                    nameof(IDummyMediaBlockExtensionOptions.LocalMediaDirectory),
                    string.Format(Strings.OptionsException_Shared_MultipleFilesFoundInDirectory,
                        dummyRetrievedFiles.Length,
                        dummyFileName,
                        dummyLocalMediaDirectory,
                        string.Join("\n", dummyRetrievedFiles))),
                result.Message,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void ResolveLocalAbsolutePath_ReturnsLocalAbsolutePath()
        {
            // Arrange
            const string dummyLocalMediaDirectory = "file:///host/dummy/local/images"; // /dummy/local/images isn't considered absolute on windows and c:/dummy/local/images isn't considered absolute on linux
            const string dummyFileName = "dummyFileName";
            const string dummyRetrievedFile = "dummyRetrievedFile";
            Mock<IDummyMediaBlockExtensionOptions> mockMediaBlockExtensionOptions = _mockRepository.Create<IDummyMediaBlockExtensionOptions>();
            mockMediaBlockExtensionOptions.Setup(m => m.LocalMediaDirectory).Returns(dummyLocalMediaDirectory);
            Mock<IDirectoryService> mockDirectoryService = _mockRepository.Create<IDirectoryService>();
            mockDirectoryService.
                Setup(d => d.GetFiles(new Uri(dummyLocalMediaDirectory).AbsolutePath, dummyFileName, SearchOption.AllDirectories)).
                Returns(new string[] { dummyRetrievedFile });
            ExposedMediaBlockFactory testSubject = CreateExposedMediaBlockFactory(directoryService: mockDirectoryService.Object);

            // Act
            string result = testSubject.ExposedResolveLocalAbsolutePath(true,
                dummyFileName,
                mockMediaBlockExtensionOptions.Object);
            _mockRepository.VerifyAll();
            Assert.Equal(dummyRetrievedFile, result);
        }

        public ExposedMediaBlockFactory CreateExposedMediaBlockFactory(IDirectoryService directoryService = null)
        {
            return new ExposedMediaBlockFactory(directoryService ?? _mockRepository.Create<IDirectoryService>().Object);
        }

        public class ExposedMediaBlockFactory : MediaBlockFactory<Block, IDummyMediaBlockOptions, IDummyMediaBlockExtensionOptions>
        {
            public ExposedMediaBlockFactory(IDirectoryService directoryService) : base(directoryService)
            {
            }

            public string ExposedValidateSrcAndResolveFileName(IDummyMediaBlockOptions dummyMediaBlockOptions)
            {
                return ValidateSrcAndResolveFileName(dummyMediaBlockOptions);
            }

            public string ExposedResolveLocalAbsolutePath(bool enableFileOperations, string fileName, IDummyMediaBlockExtensionOptions dummyMediaBlockExtensionOptions)
            {
                return ResolveLocalAbsolutePath(enableFileOperations, fileName, dummyMediaBlockExtensionOptions);
            }

            public override Block Create(ProxyJsonBlock proxyJsonBlock, BlockProcessor blockProcessor)
            {
                // Do nothing
                return null;
            }
        }

        public interface IDummyMediaBlockOptions : IMediaBlockOptions<IDummyMediaBlockOptions>
        {
        }

        public interface IDummyMediaBlockExtensionOptions : IMediaBlocksExtensionOptions<IDummyMediaBlockOptions>
        {
        }
    }
}
