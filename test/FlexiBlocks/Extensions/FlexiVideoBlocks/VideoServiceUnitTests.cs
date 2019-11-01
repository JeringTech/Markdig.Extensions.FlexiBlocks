using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    public class VideoServiceUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfProcessServiceIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new VideoService(null));
        }

        [Theory]
        [MemberData(nameof(GetVideoDimensionsAndDuration_ThrowsInvalidOperationExceptionIfTheRetrievedMetadataIsInvalid_Data))]
        public void GetVideoDimensionsAndDuration_ThrowsInvalidOperationExceptionIfTheRetrievedMetadataIsInvalid(string dummyMetadata)
        {
            // Arrange
            const string dummyPath = "dummyPath";
            Mock<VideoService> mockTestSubject = CreateMockVideoService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.RunFfmpeg("ffprobe", string.Format(VideoService.GET_METADATA_ARGUMENTS_FORMAT, dummyPath), 1000)).Returns(dummyMetadata);


            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => mockTestSubject.Object.GetVideoDimensionsAndDuration(dummyPath));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.InvalidOperationException_VideoService_InvalidVideoMetadata, dummyPath, dummyMetadata), result.Message);
        }

        public static IEnumerable<object[]> GetVideoDimensionsAndDuration_ThrowsInvalidOperationExceptionIfTheRetrievedMetadataIsInvalid_Data()
        {
            return new object[][]
            {
                // Empty string value
                new object[]{ @"123

123" },
                // Non double value
                new object[]{ @"123
test
123"},
                // Missing line
                new object[]{ @"123
123"}
            };
        }

        [Fact]
        public void GetVideoDimensionsAndDuration_GetsVideoDimensionsAndDuration()
        {
            // Arrange
            const string dummyPath = "dummyPath";
            const double dummyWidth = 123.12;
            const double dummyHeight = 321.3;
            const double dummyDuration = 222;
            string dummyMetadata = $@"{dummyWidth}
{dummyHeight}
{dummyDuration}";
            Mock<VideoService> mockTestSubject = CreateMockVideoService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.RunFfmpeg("ffprobe", string.Format(VideoService.GET_METADATA_ARGUMENTS_FORMAT, dummyPath), 1000)).Returns(dummyMetadata);

            // Act
            (double resultWidth, double resultHeight, double resultDuration) = mockTestSubject.Object.GetVideoDimensionsAndDuration(dummyPath);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyWidth, resultWidth);
            Assert.Equal(dummyHeight, resultHeight);
            Assert.Equal(dummyDuration, resultDuration);
        }

        [Fact]
        public void GeneratePoster_GeneratesPoster()
        {
            // Arrange
            const string dummyVideoPath = "dummyVideoPath";
            const string dummyPosterPath = "dummyPosterPath";
            Mock<VideoService> mockTestSubject = CreateMockVideoService();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.RunFfmpeg("ffmpeg", string.Format(VideoService.GENERATE_POSTER_ARGUMENTS_FORMAT, dummyVideoPath, dummyPosterPath), 1000));

            // Act
            mockTestSubject.Object.GeneratePoster(dummyVideoPath, dummyPosterPath);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void RunFfmpeg_ThrowsInvalidOperationsExceptionIfAWin32ExceptionIsThrown()
        {
            // Arrange
            const string dummyExecutable = "dummyExecutable";
            const string dummyArguments = "dummyArguments";
            const int dummyTimeoutMS = 1234;
            var dummyWin32Exception = new Win32Exception();
            Mock<IProcessService> mockProcessService = _mockRepository.Create<IProcessService>();
            mockProcessService.Setup(p => p.Run(dummyExecutable, dummyArguments, dummyTimeoutMS)).Throws(dummyWin32Exception);
            VideoService testSubject = CreateVideoService(mockProcessService.Object);

            // Act and assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.RunFfmpeg(dummyExecutable, dummyArguments, dummyTimeoutMS));
            _mockRepository.VerifyAll();
            Assert.Same(dummyWin32Exception, result.InnerException);
            Assert.Equal(Strings.InvalidOperationException_VideoService_FfmpegRequired, result.Message);
        }

        [Fact]
        public void RunFfmpeg_RunsFfmpeg()
        {
            // Arrange
            const string dummyOutput = "dummyOutput";
            const string dummyExecutable = "dummyExecutable";
            const string dummyArguments = "dummyArguments";
            const int dummyTimeoutMS = 1234;
            Mock<IProcessService> mockProcessService = _mockRepository.Create<IProcessService>();
            mockProcessService.Setup(p => p.Run(dummyExecutable, dummyArguments, dummyTimeoutMS)).Returns(dummyOutput);
            VideoService testSubject = CreateVideoService(mockProcessService.Object);

            // Act
            string result = testSubject.RunFfmpeg(dummyExecutable, dummyArguments, dummyTimeoutMS);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyOutput, result);
        }

        private static VideoService CreateVideoService(IProcessService processService = null)
        {
            return new VideoService(processService ?? _mockRepository.Create<IProcessService>().Object);
        }

        private static Mock<VideoService> CreateMockVideoService(IProcessService processService = null)
        {
            return _mockRepository.Create<VideoService>(processService ?? _mockRepository.Create<IProcessService>().Object);
        }
    }
}
