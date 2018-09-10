using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Newtonsoft.Json;
using System;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockOptionsUnitTests
    {
        [Fact]
        public void ValidateAndPopulate_ThrowsFlexiBlocksExceptionIfClassFormatIsAnInvalidFormat()
        {
            // Arrange
            const string dummyClassFormat = "dummy-{0}-{1}";

            // Act and assert
            FlexiBlocksException result = Assert.
                Throws<FlexiBlocksException>(() => new FlexiAlertBlockOptions(classFormat: dummyClassFormat, alertType: "dummyAlertType"));
            Assert.Equal(string.Format(Strings.FlexiBlocksException_InvalidFormat,
                    nameof(FlexiAlertBlockOptions.ClassFormat),
                    dummyClassFormat),
                result.Message);
            Assert.IsType<FormatException>(result.InnerException);
        }

        [Fact]
        public void ValidateAndPopulate_PopulatesClassifClassFormatAndAlertTypeAreDefined()
        {
            // Arrange
            const string dummyClassFormat = "dummy-{0}";
            const string dummyAlertType = "dummyAlertType";

            // Act
            var testSubject = new FlexiAlertBlockOptions(classFormat: dummyClassFormat, alertType: dummyAlertType);

            // Assert
            Assert.Equal(string.Format(dummyClassFormat, dummyAlertType), testSubject.Class);
        }

        [Fact]
        public void ValidateAndPopulate_SetsClassToNullIfClassFormatOrAlertTypeIsUndefined()
        {
            // Arrange
            var testSubject = new FlexiAlertBlockOptions();
            string initialClass = testSubject.Class;

            // Act
            JsonConvert.PopulateObject($"{{\"classFormat\": null}}", testSubject);

            // Assert
            Assert.NotNull(initialClass);
            Assert.Null(testSubject.Class);
        }

    }
}
