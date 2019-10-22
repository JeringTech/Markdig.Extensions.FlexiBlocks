using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    public class FlexiPictureBlockRendererUnitTests
    {
        [Fact]
        public void WriteBlock_OnlyWritesAltIfEnableHtmlForBlockIsFalse()
        {
            // Arrange
            const string dummyAlt = "dummyAlt";
            FlexiPictureBlock dummyFlexiPictureBlock = CreateFlexiPictureBlock(alt: dummyAlt);
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter)
            {
                EnableHtmlForBlock = false
            };
            ExposedFlexiPictureBlockRenderer testSubject = CreateExposedFlexiPictureBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiPictureBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal($"{dummyAlt}\n", result, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [MemberData(nameof(WriteBlock_WritesBlock_Data))]
        public void WriteBlock_WritesBlock(FlexiPictureBlock dummyFlexiPictureBlock,
            string expectedResult)
        {
            // Arrange
            var dummyStringWriter = new StringWriter();
            var dummyHtmlRenderer = new HtmlRenderer(dummyStringWriter);
            ExposedFlexiPictureBlockRenderer testSubject = CreateExposedFlexiPictureBlockRenderer();

            // Act
            testSubject.ExposedWriteBlock(dummyHtmlRenderer, dummyFlexiPictureBlock);
            string result = dummyStringWriter.ToString();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> WriteBlock_WritesBlock_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummySrc = "dummySrc";
            const string dummyAlt = "dummyAlt";
            const double dummyWidth = 123;
            const double dummyAspectRatio = 33.333;
            const string dummyIcon = "<dummyIcon></dummyIcon>";
            const string dummyIconWithExitFullscreenClass = "<dummyIcon class=\"__exit-fullscreen-icon\"></dummyIcon>";
            const string dummyIconWithErrorClass = "<dummyIcon class=\"__error-icon\"></dummyIcon>";
            const string dummySpinner = "<dummySpinner></dummySpinner>";
            const string dummySpinnerWithClass = "<dummySpinner class=\"__spinner\"></dummySpinner>";
            const string dummyAttributeKey1 = "dummyAttributeKey1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            const string dummyAttributeKey2 = "dummyAttributeKey2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            const string dummyClass = "dummyClass";

            return new object[][]
            {
                // BlockName is assigned as a class of the root element and all default classes are prepended with it
                new object[]{
                    CreateFlexiPictureBlock(dummyBlockName),
                    $@"<div class=""{dummyBlockName} {dummyBlockName}_no-alt {dummyBlockName}_is-lazy {dummyBlockName}_no-width {dummyBlockName}_no-aspect-ratio {dummyBlockName}_no-exit-fullscreen-icon {dummyBlockName}_no-error-icon {dummyBlockName}_no-spinner"">
<button class=""{dummyBlockName}__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""{dummyBlockName}__container"">
<div class=""{dummyBlockName}__error-notice"">
</div>
<div class=""{dummyBlockName}__picture-container"">
<picture class=""{dummyBlockName}__picture"">
<img class=""{dummyBlockName}__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If exit fullscreen icon is valid HTML, it is rendered with a default class and a _has-exit-fullscreen-icon class is rendered
                new object[]{
                    CreateFlexiPictureBlock(exitFullscreenIcon: dummyIcon),
                    $@"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _has-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
{dummyIconWithExitFullscreenClass}
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If exit fullscreen icon is null, whitespace or an empty string, no exit fullscreen icon is rendered and a _no-exit-fullscreen-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiPictureBlock(exitFullscreenIcon: " "),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiPictureBlock(exitFullscreenIcon: string.Empty),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If width is larger than 0, it is assigned to width style properties and a _has-width class is rendered
                new object[]{
                    CreateFlexiPictureBlock(width: dummyWidth),
                    $@"<div class="" _no-alt _is-lazy _has-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"" style=""width:{dummyWidth}px"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"" style=""width:{dummyWidth}px"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If width is less than or equal to 0, no style attributes are rendered and a _no-width class is rendered (0 case already verified in other tests)
                new object[]{
                    CreateFlexiPictureBlock(width: -1),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If error icon is valid HTML, it is rendered with a default class and a _has-error-icon class is rendered
                new object[]{
                    CreateFlexiPictureBlock(errorIcon: dummyIcon),
                    $@"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _has-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
{dummyIconWithErrorClass}
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If error icon is null, whitespace or an empty string, no error icon is rendered and a _no-error-icon class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiPictureBlock(errorIcon: " "),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiPictureBlock(errorIcon: string.Empty),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If spinner is valid HTML, it is rendered with a default class and a _has-spinner class is rendered
                new object[]{
                    CreateFlexiPictureBlock(spinner: dummySpinner),
                    $@"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _has-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
{dummySpinnerWithClass}
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If spinner is null, whitespace or an empty string, no spinner is rendered and a _no-spinner class is rendered (null case already verified in other tests)
                new object[]{
                    CreateFlexiPictureBlock(spinner: " "),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiPictureBlock(spinner: string.Empty),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If aspect ratio is larger than 0, it is assigned to the padding-bottom style property and a _has-aspect-ratio is rendered
                new object[]{
                    CreateFlexiPictureBlock(aspectRatio: dummyAspectRatio),
                    $@"<div class="" _no-alt _is-lazy _no-width _has-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"" style=""padding-bottom:{dummyAspectRatio}%"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If aspect ratio is less than or equal to 0, no style attribute is rendered and a _no-aspect-ratio class is rendered (0 case already verified in other tests)
                new object[]{
                    CreateFlexiPictureBlock(aspectRatio: -1),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If lazy is true, src is assigned to the data-src attribute and an _is-lazy class is rendered
                new object[]{
                    CreateFlexiPictureBlock(src: dummySrc),
                    $@"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src=""{dummySrc}"" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If lazy is false, src is assigned to the src attribute and a _not-lazy class is rendered
                new object[]{
                    CreateFlexiPictureBlock(lazy: false, src: dummySrc),
                    $@"<div class="" _no-alt _not-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" src=""{dummySrc}"" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If alt is not null, whitespace or an empty string, it is assigned to the alt attribute and a _has-alt class is rendered
                new object[]{
                    CreateFlexiPictureBlock(alt: dummyAlt),
                    $@"<div class="" _has-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" alt=""{dummyAlt}"" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If alt is null, whitespace or an empty string, no alt attribute is rendered and a _no-alt class is rendered (null case alrady verified in other tests)
                new object[]{
                    CreateFlexiPictureBlock(alt: " "),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                new object[]{
                    CreateFlexiPictureBlock(alt: string.Empty),
                    @"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If attributes specified, they're written
                new object[]{
                    CreateFlexiPictureBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { dummyAttributeKey1, dummyAttributeValue1 }, { dummyAttributeKey2, dummyAttributeValue2 } })),
                    $@"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner"" {dummyAttributeKey1}=""{dummyAttributeValue1}"" {dummyAttributeKey2}=""{dummyAttributeValue2}"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                },
                // If classes are specified, they're appended to default classes
                new object[]{
                    CreateFlexiPictureBlock(attributes: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{ { "class", dummyClass } })),
                    $@"<div class="" _no-alt _is-lazy _no-width _no-aspect-ratio _no-exit-fullscreen-icon _no-error-icon _no-spinner {dummyClass}"">
<button class=""__exit-fullscreen-button"" title=""Exit fullscreen"" aria-label=""Exit fullscreen"">
</button>
<div class=""__container"">
<div class=""__error-notice"">
</div>
<div class=""__picture-container"">
<picture class=""__picture"">
<img class=""__image"" data-src="""" tabindex=""-1"">
</picture>
</div>
</div>
</div>
"
                }
            };
        }

        public class ExposedFlexiPictureBlockRenderer : FlexiPictureBlockRenderer
        {
            public void ExposedWriteBlock(HtmlRenderer htmlRenderer, FlexiPictureBlock flexiPictureBlock)
            {
                WriteBlock(htmlRenderer, flexiPictureBlock);
            }
        }

        private ExposedFlexiPictureBlockRenderer CreateExposedFlexiPictureBlockRenderer()
        {
            return new ExposedFlexiPictureBlockRenderer();
        }

        private static FlexiPictureBlock CreateFlexiPictureBlock(string blockName = default,
            string src = default,
            string alt = default,
            bool lazy = true,
            double width = default,
            double height = default,
            double aspectRatio = default,
            string exitFullscreenIcon = default,
            string errorIcon = default,
            string spinner = default,
            ReadOnlyDictionary<string, string> attributes = default)
        {
            return new FlexiPictureBlock(blockName, src, alt, lazy, width, height, aspectRatio, exitFullscreenIcon, errorIcon, spinner, attributes, null);
        }
    }
}
