using Jering.Markdig.Extensions.FlexiBlocks.FlexiVideoBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiVideoBlocks
{
    public class FlexiVideoBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiVideoBlockOptions_CanBePopulated_Data))]
        public void FlexiVideoBlockOptions_CanBePopulated(SerializableWrapper<FlexiVideoBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiVideoBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiVideoBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiVideoBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Src, result.Src);
            Assert.Equal(expectedResult.Type, result.Type);
            Assert.Equal(expectedResult.Width, result.Width);
            Assert.Equal(expectedResult.Height, result.Height);
            Assert.Equal(expectedResult.Duration, result.Duration);
            Assert.Equal(expectedResult.GeneratePoster, result.GeneratePoster);
            Assert.Equal(expectedResult.Poster, result.Poster);
            Assert.Equal(expectedResult.Spinner, result.Spinner);
            Assert.Equal(expectedResult.PlayIcon, result.PlayIcon);
            Assert.Equal(expectedResult.PauseIcon, result.PauseIcon);
            Assert.Equal(expectedResult.FullscreenIcon, result.FullscreenIcon);
            Assert.Equal(expectedResult.ExitFullscreenIcon, result.ExitFullscreenIcon);
            Assert.Equal(expectedResult.ErrorIcon, result.ErrorIcon);
            Assert.Equal(expectedResult.EnableFileOperations, result.EnableFileOperations);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiVideoBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummySrc = "dummySrc";
            const string dummyType = "dummyType";
            const double dummyWidth = 123;
            const double dummyHeight = 321;
            const double dummyDuration = 222;
            const bool dummyGeneratePoster = true;
            const string dummyPoster = "dummyPoster";
            const string dummySpinner = "dummySpinner";
            const string dummyPlayIcon = "dummyPlayIcon";
            const string dummyPauseIcon = "dummyPauseIcon";
            const string dummyFullscreenIcon = "dummyFullscreenIcon";
            const string dummyExitFullscreenIcon = "dummyExitFullscreenIcon";
            const string dummyErrorIcon = "dummyErrorIcon";
            const bool dummyEnableFileOperations = false;
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiVideoBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiVideoBlockOptions>(new FlexiVideoBlockOptions()),
                    new SerializableWrapper<FlexiVideoBlockOptions>(new FlexiVideoBlockOptions(dummyBlockName,
                        dummySrc,
                        dummyType,
                        dummyWidth,
                        dummyHeight,
                        dummyDuration,
                        dummyGeneratePoster,
                        dummyPoster,
                        dummySpinner,
                        dummyPlayIcon,
                        dummyPauseIcon,
                        dummyFullscreenIcon,
                        dummyExitFullscreenIcon,
                        dummyErrorIcon,
                        dummyEnableFileOperations,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiVideoBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiVideoBlockOptions.Src)}"": ""{dummySrc}"",
    ""{nameof(FlexiVideoBlockOptions.Type)}"": ""{dummyType}"",
    ""{nameof(FlexiVideoBlockOptions.Width)}"": ""{dummyWidth}"",
    ""{nameof(FlexiVideoBlockOptions.Height)}"": ""{dummyHeight}"",
    ""{nameof(FlexiVideoBlockOptions.Duration)}"": ""{dummyDuration}"",
    ""{nameof(FlexiVideoBlockOptions.GeneratePoster)}"": ""{dummyGeneratePoster}"",
    ""{nameof(FlexiVideoBlockOptions.Poster)}"": ""{dummyPoster}"",
    ""{nameof(FlexiVideoBlockOptions.Spinner)}"": ""{dummySpinner}"",
    ""{nameof(FlexiVideoBlockOptions.PlayIcon)}"": ""{dummyPlayIcon}"",
    ""{nameof(FlexiVideoBlockOptions.PauseIcon)}"": ""{dummyPauseIcon}"",
    ""{nameof(FlexiVideoBlockOptions.FullscreenIcon)}"": ""{dummyFullscreenIcon}"",
    ""{nameof(FlexiVideoBlockOptions.ExitFullscreenIcon)}"": ""{dummyExitFullscreenIcon}"",
    ""{nameof(FlexiVideoBlockOptions.ErrorIcon)}"": ""{dummyErrorIcon}"",
    ""{nameof(FlexiVideoBlockOptions.EnableFileOperations)}"": ""{dummyEnableFileOperations}"",
    ""{nameof(FlexiVideoBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new SerializableWrapper<FlexiVideoBlockOptions>(new FlexiVideoBlockOptions(dummyBlockName)),
                    new SerializableWrapper<FlexiVideoBlockOptions>(new FlexiVideoBlockOptions(dummyBlockName, dummySrc)), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiVideoBlockOptions.Src)}"": ""{dummySrc}"" }}"
                },

                // Populating FlexiVideoBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new SerializableWrapper<FlexiVideoBlockOptions>(new FlexiVideoBlockOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiVideoBlockOptions>(new FlexiVideoBlockOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiVideoBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
