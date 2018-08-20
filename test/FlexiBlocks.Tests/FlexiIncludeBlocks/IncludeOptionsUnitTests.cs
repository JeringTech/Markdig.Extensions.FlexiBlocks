using Jering.Markdig.Extensions.FlexiBlocks.FlexiIncludeBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiIncludeBlocks
{
    public class IncludeOptionsUnitTests
    {
        [Theory]
        [MemberData(nameof(Constructor_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString_Data))]
        public void Constructor_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString(string dummySource)
        {
            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => new IncludeOptions(dummySource));
            Assert.Equal(string.Format(Strings.ArgumentException_MustBeDefined, "source"), result.Message);
        }

        public static IEnumerable<object[]> Constructor_ThrowsArgumentExceptionIfSourceIsNullWhiteSpaceOrAnEmptyString_Data()
        {
            return new object[][]
            {
                new object[]{null},
                new object[]{string.Empty},
                new object[]{" "}
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfContentTypeIsInvalid()
        {
            // Arrange
            ArgumentException result = Assert.Throws<ArgumentException>(() => new IncludeOptions("dummySource", (ContentType)3)); // Arbitrary invalid content type
            Assert.Equal(string.Format(Strings.ArgumentException_InvalidEnumArgument, "contentType", 3, "ContentType"), result.Message);
        }
    }
}
