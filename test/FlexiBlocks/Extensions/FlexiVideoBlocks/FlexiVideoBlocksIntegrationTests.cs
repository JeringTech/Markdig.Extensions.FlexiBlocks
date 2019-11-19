using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Markdig;
using System;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    // Integration tests that do not fit in amongst specs for this extension
    public class FlexiVideoBlocksIntegrationTests : IClassFixture<FlexiVideoBlocksIntegrationTestsFixture>
    {
        private readonly FlexiVideoBlocksIntegrationTestsFixture _fixture;
        private const string _dummyVideoFileName = "exampleVideo.mp4";

        public FlexiVideoBlocksIntegrationTests(FlexiVideoBlocksIntegrationTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.RecreateDirectory();
            string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), _dummyVideoFileName);
            string targetPath = Path.Combine(_fixture.TempDirectory, _dummyVideoFileName);
            File.Copy(sourcePath, targetPath);
        }

        [Fact]
        public void FlexiVideoBlocks_RetrievesMetadataAndGeneratesPoster()
        {
            // Arrange
            string dummySrc = $"/url/{_dummyVideoFileName}";
            var flexiVideoBlocksExtensionOptions = new FlexiVideoBlocksExtensionOptions(localMediaDirectory: _fixture.TempDirectory);
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                    UseFlexiVideoBlocks(flexiVideoBlocksExtensionOptions);
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            // Act
            string result = Markdown.ToHtml($@"v{{
    ""src"": ""{dummySrc}"",
    ""generatePoster"": true
}}", markdownPipeline);

            // Assert
            const double expectedWidth = 270;
            string expectedPoster = dummySrc.Replace(".mp4", "_poster.png");
            string expectedPosterFileName = _dummyVideoFileName.Replace(".mp4", "_poster.png");
            Assert.True(File.Exists(Path.Combine(_fixture.TempDirectory, expectedPosterFileName)));
            Assert.Equal($@"<div class=""flexi-video flexi-video_has-poster flexi-video_has-width flexi-video_has-aspect-ratio flexi-video_has-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon"">
<div class=""flexi-video__container"" tabindex=""-1"" style=""width:{expectedWidth}px"">
<div class=""flexi-video__video-outer-container"" style=""width:{expectedWidth}px"">
<div class=""flexi-video__video-inner-container"" style=""padding-bottom:{480 / expectedWidth * 100}%"">
<video class=""flexi-video__video"" preload=""auto"" poster=""{expectedPoster}"" muted playsInline disablePictureInPicture loop>
<source class=""flexi-video__source"" data-src=""/url/exampleVideo.mp4"" type=""video/mp4"">
</video>
</div>
</div>
<div class=""flexi-video__controls"">
<button class=""flexi-video__play-pause-button"" aria-label=""Pause/play"">
<svg class=""flexi-video__play-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M8 5v14l11-7z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
<svg class=""flexi-video__pause-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M6 19h4V5H6v14zm8-14v14h4V5h-4z""/></svg>
</button>
<div class=""flexi-video__elapsed-time"">
<span class=""flexi-video__current-time"">0:00</span>
/<span class=""flexi-video__duration"">{TimeSpan.FromSeconds(Math.Round(5.8)).ToString("m\\:ss")}</span>
</div>
<div class=""flexi-video__progress"">
<div class=""flexi-video__progress-track"">
<div class=""flexi-video__progress-played""></div>
<div class=""flexi-video__progress-buffered""></div>
</div>
</div>
<button class=""flexi-video__fullscreen-button"" aria-label=""Toggle fullscreen"">
<svg class=""flexi-video__fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z""/></svg>
<svg class=""flexi-video__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z""/></svg>
</button>
</div>
<div class=""flexi-video__error-notice"">
<svg class=""flexi-video__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-video__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
</div>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void FlexiVideoBlocks_SpecifiedMetadataAndPosterTakePrecedence()
        {
            // Arrange
            const int dummyWidth = 300;
            const int dummyHeight = 200;
            const double dummyDuration = 1234.34;
            const string dummyPoster = "dummyPoster";
            string dummySrc = $"/url/{_dummyVideoFileName}";
            var flexiVideoBlocksExtensionOptions = new FlexiVideoBlocksExtensionOptions(localMediaDirectory: _fixture.TempDirectory);
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                    UseFlexiVideoBlocks(flexiVideoBlocksExtensionOptions);
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            // Act
            string result = Markdown.ToHtml($@"v{{
    ""src"": ""{dummySrc}"",
    ""width"": {dummyWidth},
    ""height"": {dummyHeight},
    ""duration"": {dummyDuration},
    ""poster"": ""{dummyPoster}"",
    ""generatePoster"": true
}}", markdownPipeline);

            // Assert
            Assert.Equal($@"<div class=""flexi-video flexi-video_has-poster flexi-video_has-width flexi-video_has-aspect-ratio flexi-video_has-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon"">
<div class=""flexi-video__container"" tabindex=""-1"" style=""width:{dummyWidth}px"">
<div class=""flexi-video__video-outer-container"" style=""width:{dummyWidth}px"">
<div class=""flexi-video__video-inner-container"" style=""padding-bottom:{dummyHeight / (double)dummyWidth * 100}%"">
<video class=""flexi-video__video"" preload=""auto"" poster=""{dummyPoster}"" muted playsInline disablePictureInPicture loop>
<source class=""flexi-video__source"" data-src=""{dummySrc}"" type=""video/mp4"">
</video>
</div>
</div>
<div class=""flexi-video__controls"">
<button class=""flexi-video__play-pause-button"" aria-label=""Pause/play"">
<svg class=""flexi-video__play-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M8 5v14l11-7z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
<svg class=""flexi-video__pause-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M6 19h4V5H6v14zm8-14v14h4V5h-4z""/></svg>
</button>
<div class=""flexi-video__elapsed-time"">
<span class=""flexi-video__current-time"">0:00</span>
/<span class=""flexi-video__duration"">{TimeSpan.FromSeconds(Math.Round(dummyDuration)).ToString("m\\:ss")}</span>
</div>
<div class=""flexi-video__progress"">
<div class=""flexi-video__progress-track"">
<div class=""flexi-video__progress-played""></div>
<div class=""flexi-video__progress-buffered""></div>
</div>
</div>
<button class=""flexi-video__fullscreen-button"" aria-label=""Toggle fullscreen"">
<svg class=""flexi-video__fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z""/></svg>
<svg class=""flexi-video__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z""/></svg>
</button>
</div>
<div class=""flexi-video__error-notice"">
<svg class=""flexi-video__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-video__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
</div>
</div>
", result, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void FlexiVideoBlocks_MetadataArentRetrievedAndPosterIsNotGeneratedIfEnableFileOperationsIsFalse()
        {
            // Arrange
            string dummySrc = $"/url/{_dummyVideoFileName}";
            var flexiVideoBlocksExtensionOptions = new FlexiVideoBlocksExtensionOptions(localMediaDirectory: _fixture.TempDirectory);
            var markdownPipelineBuilder = new MarkdownPipelineBuilder();
            markdownPipelineBuilder.
                UseFlexiOptionsBlocks().
                    UseFlexiVideoBlocks(flexiVideoBlocksExtensionOptions);
            MarkdownPipeline markdownPipeline = markdownPipelineBuilder.Build();

            // Act
            string result = Markdown.ToHtml($@"v{{
    ""src"": ""{dummySrc}"",
    ""generatePoster"": true,
    ""enableFileOperations"": false
}}", markdownPipeline);

            // Assert
            Assert.Equal($@"<div class=""flexi-video flexi-video_no-poster flexi-video_no-width flexi-video_no-aspect-ratio flexi-video_no-duration flexi-video_has-type flexi-video_has-spinner flexi-video_has-play-icon flexi-video_has-pause-icon flexi-video_has-fullscreen-icon flexi-video_has-exit-fullscreen-icon flexi-video_has-error-icon"">
<div class=""flexi-video__container"" tabindex=""-1"">
<div class=""flexi-video__video-outer-container"">
<div class=""flexi-video__video-inner-container"">
<video class=""flexi-video__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""flexi-video__source"" data-src=""{dummySrc}"" type=""video/mp4"">
</video>
</div>
</div>
<div class=""flexi-video__controls"">
<button class=""flexi-video__play-pause-button"" aria-label=""Pause/play"">
<svg class=""flexi-video__play-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M8 5v14l11-7z""/><path d=""M0 0h24v24H0z"" fill=""none""/></svg>
<svg class=""flexi-video__pause-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M6 19h4V5H6v14zm8-14v14h4V5h-4z""/></svg>
</button>
<div class=""flexi-video__elapsed-time"">
<span class=""flexi-video__current-time"">0:00</span>
/<span class=""flexi-video__duration"">0:00</span>
</div>
<div class=""flexi-video__progress"">
<div class=""flexi-video__progress-track"">
<div class=""flexi-video__progress-played""></div>
<div class=""flexi-video__progress-buffered""></div>
</div>
</div>
<button class=""flexi-video__fullscreen-button"" aria-label=""Toggle fullscreen"">
<svg class=""flexi-video__fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M7 14H5v5h5v-2H7v-3zm-2-4h2V7h3V5H5v5zm12 7h-3v2h5v-5h-2v3zM14 5v2h3v3h2V5h-5z""/></svg>
<svg class=""flexi-video__exit-fullscreen-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path shape-rendering=""crispEdges"" d=""M5 16h3v3h2v-5H5v2zm3-8H5v2h5V5H8v3zm6 11h2v-3h3v-2h-5v5zm2-11V5h-2v5h5V8h-3z""/></svg>
</button>
</div>
<div class=""flexi-video__error-notice"">
<svg class=""flexi-video__error-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/></svg>
</div>
<div class=""flexi-video__spinner spinner"">
    <div class=""spinner__rects"">
        <div class=""spinner__rect-1""></div>
        <div class=""spinner__rect-2""></div>
        <div class=""spinner__rect-3""></div>
    </div>
</div>
</div>
</div>
", result, ignoreLineEndingDifferences: true);
        }
    }

    public class FlexiVideoBlocksIntegrationTestsFixture : IDisposable
    {
        public string TempDirectory { get; } = Path.Combine(Path.GetTempPath(), nameof(FlexiVideoBlocksIntegrationTests)); // Dummy directory for containing videos

        public void RecreateDirectory()
        {
            TryDeleteDirectory();
            Directory.CreateDirectory(TempDirectory);
        }

        public void TryDeleteDirectory()
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
