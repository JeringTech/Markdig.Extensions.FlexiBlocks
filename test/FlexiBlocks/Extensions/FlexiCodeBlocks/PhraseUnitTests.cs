using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System;
using System.Collections.Generic;
using Xunit;
namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class PhraseUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfStartIsLessThan0()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Phrase(-1, 0));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeExceptionIfEndIsLessThan0()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Phrase(0, -1));
        }

        [Fact]
        public void Constructor_ThrowsArgumentExceptionIfEndIsLessThanStart()
        {
            // Act and assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Phrase(3, 2));
        }

        [Theory]
        [MemberData(nameof(CompareTo_ComparesPhrases_Data))]
        public void CompareTo_ComparesPhrases(int dummyStart, int dummyEnd,
            int dummyOtherStart, int dummyOtherEnd,
            int expectedResult)
        {
            // Arrange
            var dummyOtherPhrase = new Phrase(dummyOtherStart, dummyOtherEnd);
            var testSubject = new Phrase(dummyStart, dummyEnd);

            // Act
            int result = testSubject.CompareTo(dummyOtherPhrase);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> CompareTo_ComparesPhrases_Data()
        {
            return new object[][]
            {
                // Start < other start
                new object[]{1, 12, 2, 25, -1},
                // Start > other start
                new object[]{2, 25, 1, 12, 1},
                // Start == other start && end < other end
                new object[]{4, 6, 4, 7, 1},
                // Start == other start && end > other end
                new object[]{4, 7, 4, 6, -1},
                // Same
                new object[]{34, 67, 34, 67, 0}
            };
        }

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalPhraseOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalPhraseOtherwiseReturnsFalse(Phrase dummyPhrase,
            object dummyObj,
            bool expectedResult)
        {
            // Act
            bool result = dummyPhrase.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalPhraseOtherwiseReturnsFalse_Data()
        {
            const int dummyStart = 4; // Arbitrary
            const int dummyEnd = 25; // Arbitrary

            return new object[][]
            {
                new object[]{new Phrase(0, 0),
                    "not a line range",
                    false },
                new object[]{new Phrase(dummyStart, dummyEnd),
                    new Phrase(dummyStart, dummyEnd),
                    true },
                // False if the Phrases differ in any way
                new object[]{new Phrase(dummyStart, 5),
                    new Phrase(dummyStart, dummyEnd),
                    false },
                new object[]{new Phrase(5, dummyEnd),
                    new Phrase(dummyStart, dummyEnd),
                    false }
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalPhrases_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalPhrases(Phrase dummyPhrase1,
            Phrase dummyPhrase2,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyPhrase1.GetHashCode() == dummyPhrase2.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalPhrases_Data()
        {
            const int dummyStart = 4; // Arbitrary
            const int dummyEnd = 25; // Arbitrary

            return new object[][]
            {
                new object[]{new Phrase(dummyStart, dummyEnd),
                    new Phrase(dummyStart, dummyEnd),
                    true },
                new object[]{new Phrase(dummyStart, 5),
                    new Phrase(dummyStart, dummyEnd),
                    false },
                new object[]{new Phrase(5, dummyEnd),
                    new Phrase(dummyStart, dummyEnd),
                    false }
            };
        }
    }
}
