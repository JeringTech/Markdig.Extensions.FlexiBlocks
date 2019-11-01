using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    // Integration tests that do not fit in amongst specs for this extension
    public class VideoServiceIntegrationTests : IClassFixture<VideoServiceIntegrationTestsFixture>
    {
        private readonly VideoServiceIntegrationTestsFixture _fixture;
        private const string _dummyVideoFileName = "exampleVideo.mp4";

        public VideoServiceIntegrationTests(VideoServiceIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.RecreateDirectory();
            string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), _dummyVideoFileName);
            string targetPath = Path.Combine(_fixture.TempDirectory, _dummyVideoFileName);
            File.Copy(sourcePath, targetPath);
        }

        [Fact]
        public void GetVideoDimensionsAndDuration_RetrievesVideoDimensions()
        {
            // Arrange
            string dummyPath = Path.Combine(_fixture.TempDirectory, _dummyVideoFileName);
            IServiceCollection dummyServiceCollection = new ServiceCollection().AddFlexiVideoBlocks();
            using (ServiceProvider serviceProvider = dummyServiceCollection.BuildServiceProvider())
            {
                IVideoService testSubject = serviceProvider.GetRequiredService<IVideoService>();

                // Act
                (double width, double height, double duration) = testSubject.GetVideoDimensionsAndDuration(dummyPath);

                // Assert
                Assert.Equal(270, width); // TODO would be better if we generated the video as part of this test so we can verify that the right dimensions and duration are retrieved
                Assert.Equal(480, height);
                Assert.Equal(5.8, duration);
            }
        }

        [Fact]
        public void GeneratePoster_GeneratesPoster()
        {
            // Arrange
            string dummyVideoPath = Path.Combine(_fixture.TempDirectory, _dummyVideoFileName);
            string dummyPosterPath = dummyVideoPath.Replace(".mp4", "_poster.png");
            IServiceCollection dummyServiceCollection = new ServiceCollection().AddFlexiVideoBlocks();
            using (ServiceProvider serviceProvider = dummyServiceCollection.BuildServiceProvider())
            {
                IVideoService testSubject = serviceProvider.GetRequiredService<IVideoService>();

                // Act
                testSubject.GeneratePoster(dummyVideoPath, dummyPosterPath);

                // Assert
                Assert.True(File.Exists(dummyPosterPath)); // TODO we could verify its dimensions using ImageSharp
            }
        }
    }

    public class VideoServiceIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(VideoServiceIntegrationTests)); // Dummy directory for containing videos

        public void RecreateDirectory()
        {
            TryDeleteDirectory();
            Directory.CreateDirectory(TempDirectory);
        }

        private void TryDeleteDirectory()
        {
            try
            {
                Directory.Delete(TempDirectory, true);
            }
            catch
            {
                // Do nothing
            }
        }

        // Called after all tests are complete
        public void Dispose()
        {
            TryDeleteDirectory();
        }
    }
}
