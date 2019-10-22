using Jering.Markdig.Extensions.FlexiBlocks.FlexiPictureBlocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiPictureBlocks
{
    public class FlexiPictureBlockOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(FlexiPictureBlockOptions_CanBePopulated_Data))]
        public void FlexiPictureBlockOptions_CanBePopulated(SerializableWrapper<FlexiPictureBlockOptions> dummyInitialOptionsWrapper,
            SerializableWrapper<FlexiPictureBlockOptions> dummyExpectedOptionsWrapper,
            string dummyJson)
        {
            // Act
            JsonConvert.PopulateObject(dummyJson, dummyInitialOptionsWrapper.Value);

            // Assert
            FlexiPictureBlockOptions result = dummyInitialOptionsWrapper.Value;
            FlexiPictureBlockOptions expectedResult = dummyExpectedOptionsWrapper.Value;
            Assert.Equal(expectedResult.BlockName, result.BlockName);
            Assert.Equal(expectedResult.Src, result.Src);
            Assert.Equal(expectedResult.Alt, result.Alt);
            Assert.Equal(expectedResult.Lazy, result.Lazy);
            Assert.Equal(expectedResult.Width, result.Width);
            Assert.Equal(expectedResult.Height, result.Height);
            Assert.Equal(expectedResult.ExitFullscreenIcon, result.ExitFullscreenIcon);
            Assert.Equal(expectedResult.ErrorIcon, result.ErrorIcon);
            Assert.Equal(expectedResult.Spinner, result.Spinner);
            Assert.Equal(expectedResult.EnableFileOperations, result.EnableFileOperations);
            Assert.Equal(expectedResult.Attributes, result.Attributes);
        }

        public static IEnumerable<object[]> FlexiPictureBlockOptions_CanBePopulated_Data()
        {
            const string dummyBlockName = "dummyBlockName";
            const string dummySrc = "dummySrc";
            const string dummyAlt = "dummyAlt";
            const bool dummyLazy = false;
            const double dummyWidth = 123;
            const double dummyHeight = 321;
            const string dummyExitFullscreenIcon = "dummyExitFullscreenIcon";
            const string dummyErrorIcon = "dummyErrorIcon";
            const string dummySpinner = "dummySpinner";
            const bool dummyEnableFileOperations = false;
            const string dummyAttribute1 = "dummyAttribute1";
            const string dummyAttributeValue1 = "dummyAttributeValue1";
            var dummyAttributes1 = new Dictionary<string, string> { { dummyAttribute1, dummyAttributeValue1 } };
            const string dummyAttribute2 = "dummyAttribute2";
            const string dummyAttributeValue2 = "dummyAttributeValue2";
            var dummyAttributes2 = new Dictionary<string, string> { { dummyAttribute2, dummyAttributeValue2 } };

            return new object[][]
            {
                // Populating FlexiPictureBlockOptions containing default values
                new object[]
                {
                    new SerializableWrapper<FlexiPictureBlockOptions>(new FlexiPictureBlockOptions()),
                    new SerializableWrapper<FlexiPictureBlockOptions>(new FlexiPictureBlockOptions(dummyBlockName,
                        dummySrc,
                        dummyAlt,
                        dummyLazy,
                        dummyWidth,
                        dummyHeight,
                        dummyExitFullscreenIcon,
                        dummyErrorIcon,
                        dummySpinner,
                        dummyEnableFileOperations,
                        dummyAttributes1)),
                    $@"{{
    ""{nameof(FlexiPictureBlockOptions.BlockName)}"": ""{dummyBlockName}"",
    ""{nameof(FlexiPictureBlockOptions.Src)}"": ""{dummySrc}"",
    ""{nameof(FlexiPictureBlockOptions.Alt)}"": ""{dummyAlt}"",
    ""{nameof(FlexiPictureBlockOptions.Lazy)}"": ""{dummyLazy}"",
    ""{nameof(FlexiPictureBlockOptions.Width)}"": ""{dummyWidth}"",
    ""{nameof(FlexiPictureBlockOptions.Height)}"": ""{dummyHeight}"",
    ""{nameof(FlexiPictureBlockOptions.ExitFullscreenIcon)}"": ""{dummyExitFullscreenIcon}"",
    ""{nameof(FlexiPictureBlockOptions.ErrorIcon)}"": ""{dummyErrorIcon}"",
    ""{nameof(FlexiPictureBlockOptions.Spinner)}"": ""{dummySpinner}"",
    ""{nameof(FlexiPictureBlockOptions.EnableFileOperations)}"": ""{dummyEnableFileOperations}"",
    ""{nameof(FlexiPictureBlockOptions.Attributes)}"": {{
        ""{dummyAttribute1}"": ""{dummyAttributeValue1}""
    }}
}}"
                },

                // Existing values should not be overwritten if no corresponding new value
                new object[]
                {
                    new SerializableWrapper<FlexiPictureBlockOptions>(new FlexiPictureBlockOptions(dummyBlockName)),
                    new SerializableWrapper<FlexiPictureBlockOptions>(new FlexiPictureBlockOptions(dummyBlockName, dummySrc)), // Block name should stay the same
                    $@"{{ ""{nameof(FlexiPictureBlockOptions.Src)}"": ""{dummySrc}"" }}"
                },

                // Populating FlexiPictureBlockOptions with an existing attributes collection should replace the entire collection
                new object[]
                {
                    new SerializableWrapper<FlexiPictureBlockOptions>(new FlexiPictureBlockOptions(attributes: dummyAttributes1)),
                    new SerializableWrapper<FlexiPictureBlockOptions>(new FlexiPictureBlockOptions(attributes: dummyAttributes2)),
                    $@"{{
    ""{nameof(FlexiPictureBlockOptions.Attributes)}"": {{
        ""{dummyAttribute2}"": ""{dummyAttributeValue2}""
    }}
}}"
                }
            };
        }
    }
}
