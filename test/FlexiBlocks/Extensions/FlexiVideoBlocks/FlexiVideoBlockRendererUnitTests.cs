using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Markdig.Renderers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    public class FlexiVideoBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_WritesNothingIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            FlexiVideoBlock dummyFlexiVideoBlock = CreateFlexiVideoBlock();
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiVideoBlockRenderer testSubject = CreateExposedFlexiVideoBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiVideoBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(string.Empty, result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiVideoBlock dummyFlexiVideoBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiVideoBlockRenderer testSubject = CreateExposedFlexiVideoBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiVideoBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummySrc = "dummySrc";
            const string dummyType = "dummyType";
            const string dummyPoster = "dummyPoster";
            const double dummyWidth = 123;
            const double dummyAspectRatio = 33.333;
            const double dummyDuration = 1.234;
            const string dummySpinner = "<dummySpinner></dummySpinner>";
            const string dummySpinnerWithClass = "<dummySpinner class=\"__spinner\"></dummySpinner>";
            const string dummyIcon = "<dummyIcon></dummyIcon>";
            const string dummyIconWithPlayClass = "<dummyIcon class=\"__play-icon\"></dummyIcon>";
            const string dummyIconWithPauseClass = "<dummyIcon class=\"__pause-icon\"></dummyIcon>";
            const string dummyIconWithFullscreenClass = "<dummyIcon class=\"__fullscreen-icon\"></dummyIcon>";
            const string dummyIconWithExitFullscreenClass = "<dummyIcon class=\"__exit-fullscreen-icon\"></dummyIcon>";
            const string dummyIconWithErrorClass = "<dummyIcon class=\"__error-icon\"></dummyIcon>";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";
            const string expectedHasNothingResult = @"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
";

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiVideoBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_no-poster {dummyBlockName}_no-width {dummyBlockName}_no-aspect-ratio {dummyBlockName}_no-duration {dummyBlockName}_no-type {dummyBlockName}_no-spinner {dummyBlockName}_no-play-icon {dummyBlockName}_no-pause-icon {dummyBlockName}_no-fullscreen-icon {dummyBlockName}_no-exit-fullscreen-icon {dummyBlockName}_no-error-icon"">
<div class=""{dummyBlockName}__container"" tabindex=""-1"">
<div class=""{dummyBlockName}__video-outer-container"">
<div class=""{dummyBlockName}__video-inner-container"">
<video class=""{dummyBlockName}__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""{dummyBlockName}__source"" data-src="""">
</video>
</div>
</div>
<div class=""{dummyBlockName}__controls"">
<button class=""{dummyBlockName}__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""{dummyBlockName}__elapsed-time"">
<span class=""{dummyBlockName}__current-time"">0:00</span>
/<span class=""{dummyBlockName}__duration"">0:00</span>
</div>
<div class=""{dummyBlockName}__progress"">
<div class=""{dummyBlockName}__progress-track"">
<div class=""{dummyBlockName}__progress-played""></div>
<div class=""{dummyBlockName}__progress-buffered""></div>
</div>
</div>
<button class=""{dummyBlockName}__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""{dummyBlockName}__error-notice"">
</div>
</div>
</div>
"
                },
                // If width is larger than 0, it is assigned to width style properties and a _has-width class is rendered
                new object[]{
                    CreateFlexiVideoBlock(width: dummyWidth),
                    $@"<div class="" _no-poster _has-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"" style=""width:{dummyWidth}px"">
<div class=""__video-outer-container"" style=""width:{dummyWidth}px"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If width is less than or equal to 0, no style attributes are rendered and a _no-width class is rendered (0 case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(width: -1), expectedHasNothingResult
                },
                // If aspect ratio is larger than 0, it is assigned to the padding-bottom style property and a _has-aspect-ratio is rendered
                new object[]{
                    CreateFlexiVideoBlock(aspectRatio: dummyAspectRatio),
                    $@"<div class="" _no-poster _no-width _has-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"" style=""padding-bottom:{dummyAspectRatio}%"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If aspect ratio is less than or equal to 0, no style attribute is rendered and a _no-aspect-ratio class is rendered (0 case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(aspectRatio: -1), expectedHasNothingResult
                },
                // If poster is not null, whitespace or an empty string, it is assigned to the poster attribute and a _has-poster class is rendered
                new object[]{
                    CreateFlexiVideoBlock(poster: dummyPoster),
                    $@"<div class="" _has-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" poster=""{dummyPoster}"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If poster is null, whitespace or an empty string, no poster attribute is rendered and a _no-poster class is rendered (null case alrady verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(poster: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(poster: string.Empty), expectedHasNothingResult
                },
                // Src is assigned to data-src
                new object[]{
                    CreateFlexiVideoBlock(src: dummySrc),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src=""{dummySrc}"">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If type is not null, whitespace or an empty string, it is assigned to the type attribute and a _has-type class is rendered
                new object[]{
                    CreateFlexiVideoBlock(type: dummyType),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _has-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""" type=""{dummyType}"">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If type is null, whitespace or an empty string, no type attribute is rendered and a _no-type class is rendered (null case alrady verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(type: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(type: string.Empty), expectedHasNothingResult
                },
                // If play icon is valid HTML, it is rendered with a default class and a _has-play-icon class is rendered
                new object[]{
                    CreateFlexiVideoBlock(playIcon: dummyIcon),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _has-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
{dummyIconWithPlayClass}
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If play icon is null, whitespace or an empty string, no play icon is rendered and a _no-play-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(playIcon: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(playIcon: string.Empty), expectedHasNothingResult
                },
                // If pause icon is valid HTML, it is rendered with a default class and a _has-pause-icon class is rendered
                new object[]{
                    CreateFlexiVideoBlock(pauseIcon: dummyIcon),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _has-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
{dummyIconWithPauseClass}
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If pause icon is null, whitespace or an empty string, no pause icon is rendered and a _no-pause-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(pauseIcon: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(pauseIcon: string.Empty), expectedHasNothingResult
                },
                // If duration is larger than 0, it is formatted and set as the __duration element's content
                new object[]{
                    CreateFlexiVideoBlock(duration: dummyDuration),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _has-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">{TimeSpan.FromSeconds(Math.Round(dummyDuration)).ToString("m\\:ss")}</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If duration is less than or equal to 0, the __duration element's content is "0:00" (0 case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(duration: -1), expectedHasNothingResult
                },
                // If fullscreen icon is valid HTML, it is rendered with a default class and a _has-fullscreen-icon class is rendered
                new object[]{
                    CreateFlexiVideoBlock(fullscreenIcon: dummyIcon),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _has-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
{dummyIconWithFullscreenClass}
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If fullscreen icon is null, whitespace or an empty string, no fullscreen icon is rendered and a _no-fullscreen-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(fullscreenIcon: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(fullscreenIcon: string.Empty), expectedHasNothingResult
                },
                // If exit fullscreen icon is valid HTML, it is rendered with a default class and a _has-exit-fullscreen-icon class is rendered
                new object[]{
                    CreateFlexiVideoBlock(exitFullscreenIcon: dummyIcon),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _has-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
{dummyIconWithExitFullscreenClass}
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If exit fullscreen icon is null, whitespace or an empty string, no exit fullscreen icon is rendered and a _no-exit-fullscreen-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(exitFullscreenIcon: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(exitFullscreenIcon: string.Empty), expectedHasNothingResult
                },
                // If error icon is valid HTML, it is rendered with a default class and a _has-error-icon class is rendered
                new object[]{
                    CreateFlexiVideoBlock(errorIcon: dummyIcon),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _has-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
{dummyIconWithErrorClass}
</div>
</div>
</div>
"
                },
                // If error icon is null, whitespace or an empty string, no error icon is rendered and a _no-error-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(errorIcon: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(errorIcon: string.Empty), expectedHasNothingResult
                },
                // If spinner is valid HTML, it is rendered with a default class and a _has-spinner class is rendered
                new object[]{
                    CreateFlexiVideoBlock(spinner: dummySpinner),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _has-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
{dummySpinnerWithClass}
</div>
</div>
"
                },
                // If spinner is null, whitespace or an empty string, no spinner is rendered and a _no-spinner class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiVideoBlock(spinner: " "), expectedHasNothingResult
                },
                new object[]{
                    CreateFlexiVideoBlock(spinner: string.Empty), expectedHasNothingResult
                },
                // If attributes specified, they're written
                new object[]{
                    CreateFlexiVideoBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiVideoBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _no-poster _no-width _no-aspect-ratio _no-duration _no-type _no-spinner _no-play-icon _no-pause-icon _no-fullscreen-icon _no-exit-fullscreen-icon _no-error-icon {dummyClass}"">
<div class=""__container"" tabindex=""-1"">
<div class=""__video-outer-container"">
<div class=""__video-inner-container"">
<video class=""__video"" preload=""auto"" muted playsInline disablePictureInPicture loop>
<source class=""__source"" data-src="""">
</video>
</div>
</div>
<div class=""__controls"">
<button class=""__play-pause-button"" aria-label=""Pause/play"">
</button>
<div class=""__elapsed-time"">
<span class=""__current-time"">0:00</span>
/<span class=""__duration"">0:00</span>
</div>
<div class=""__progress"">
<div class=""__progress-track"">
<div class=""__progress-played""></div>
<div class=""__progress-buffered""></div>
</div>
</div>
<button class=""__fullscreen-button"" aria-label=""Toggle fullscreen"">
</button>
</div>
<div class=""__error-notice"">
</div>
</div>
</div>
"
                }
            };
        }

        public class ExposedFlexiVideoBlockRenderer : FlexiVideoBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiVideoBlock flexiVideoBlock)
            {
                WriteBlock(htmlRenderer, flexiVideoBlock);
            }
        }

        private ExposedFlexiVideoBlockRenderer CreateExposedFlexiVideoBlockRenderer()
        {
            return new ExposedFlexiVideoBlockRenderer();
        }

        private static FlexiVideoBlock CreateFlexiVideoBlock(string blockName = default,
            string src = default,
            string type = default,
            string poster = default,
            double width = default,
            double height = default,
            double aspectRatio = default,
            double duration = default,
            string spinner = default,
            string playIcon = default,
            string pauseIcon = default,
            string fullscreenIcon = default,
            string exitFullscreenIcon = default,
            string errorIcon = default,
            ReadOnlyDictionary<string, string> attributes = default)
        {
            return new FlexiVideoBlock(blockName, src, type, poster, width, height, aspectRatio, duration, spinner, playIcon, pauseIcon, fullscreenIcon, exitFullscreenIcon, errorIcon, attributes, null);
        }
    }
}
