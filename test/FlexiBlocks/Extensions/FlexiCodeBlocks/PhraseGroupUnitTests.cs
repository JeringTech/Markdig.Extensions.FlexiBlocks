using Jering.Markdig.Extensions.FlexiBlocks.FlexiCodeBlocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiCodeBlocks
{
    public class PhraseGroupUnitTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfRegexIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new PhraseGroup(null, null));
        }

        [Theory]
        [MemberData(nameof(GetPhrases_GetsPhrases_Data))]
        public void GetPhrases_GetsPhrases(string dummyRegex, int[] dummyIncludedMatches, string dummyText, List<Phrase> expectedResult)
        {
            // Arrange
            var testSubject = new PhraseGroup(dummyRegex, dummyIncludedMatches);
            var result = new List<Phrase>();

            // Act
            testSubject.GetPhrases(dummyText, result);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> GetPhrases_GetsPhrases_Data()
        {
            return new object[][]
            {
                // No regex matches
                new object[]{"1", null, "2", new List<Phrase>()},
                // Regex matches are empty strings - handled by AddMatch
                new object[]{"1(.*)", null, "1", new List<Phrase>()},
                // Regex without groups - handled by AddMatch
                new object[]{"[0-9]+", null, "12345 abcde 12345 abcde", new List<Phrase>() { new Phrase(0, 4), new Phrase(12, 16) } },
                new object[]{"[a-z]+", null, "12345 abcde 12345 abcde", new List<Phrase>() { new Phrase(6, 10), new Phrase(18, 22) } },
                // Regex with groups - main match is ignored - handled by AddMatch
                new object[]{"(.*)3(.*) ", null, "12345 abcde", new List<Phrase>(){ new Phrase(0, 1), new Phrase(3, 4) } },
                new object[]{" (.*)c(.*)", null, "12345 abcde", new List<Phrase>(){ new Phrase(6, 7), new Phrase(9, 10) } },
                // Included specified (negative indices work)
                new object[]{"1", new int[] { 0, 2, -3, -1}, "111111", new List<Phrase>(){ new Phrase(0, 0), new Phrase(2, 2), new Phrase(3, 3), new Phrase(5, 5) } }
            };
        }

        [Fact]
        public void GetPhrases_ThrowsOptionsExceptionIfIncludedMatchIndexIsOutOfRange()
        {
            const string dummyText = "1 1 1";
            var dummyPhrases = new List<Phrase>();
            var testSubject = new PhraseGroup("1", new int[] { 3 });

            // Act and assert
            OptionsException result = Assert.Throws<OptionsException>(() => testSubject.GetPhrases(dummyText, dummyPhrases));
            Assert.Equal(string.Format(Strings.OptionsException_OptionsException_InvalidOption,
                            nameof(PhraseGroup.IncludedMatches),
                            string.Format(Strings.OptionsException_PhraseGroup_IncludedMatchIndexOutOfRange, testSubject, 3, 3)),
                        result.Message);
        }

        [Theory]
        [MemberData(nameof(ToString_ReturnsPhraseGroupAsString_Data))]
        public void ToString_ReturnsPhraseGroupAsString(string dummyRegex, int[] dummyIncludedMatches, string expectedResult)
        {
            // Arrange
            var lineRange = new PhraseGroup(dummyRegex, dummyIncludedMatches);

            // Act
            string result = lineRange.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ToString_ReturnsPhraseGroupAsString_Data()
        {
            const string dummyRegex = "dummyRegex";
            var dummyIncludedMatches = new int[] { 1, 2, 3 };

            return new object[][]
            {
                new object[]{ dummyRegex, null, $"{nameof(PhraseGroup.Regex)}: {dummyRegex}, {nameof(PhraseGroup.IncludedMatches)}: null"},
                new object[]{ dummyRegex, dummyIncludedMatches, $"{nameof(PhraseGroup.Regex)}: {dummyRegex}, {nameof(PhraseGroup.IncludedMatches)}: [{string.Join(",", dummyIncludedMatches)}]"}
            };
        }

        [Theory]
        [MemberData(nameof(Equals_ReturnsTrueIfObjIsAnIdenticalPhraseGroupOtherwiseReturnsFalse_Data))]
        public void Equals_ReturnsTrueIfObjIsAnIdenticalPhraseGroupOtherwiseReturnsFalse(PhraseGroup dummyPhraseGroup,
            object dummyObj,
            bool expectedResult)
        {
            // Act
            bool result = dummyPhraseGroup.Equals(dummyObj);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Equals_ReturnsTrueIfObjIsAnIdenticalPhraseGroupOtherwiseReturnsFalse_Data()
        {
            const string dummyRegex = "dummyRegex";
            var dummyIncludedMatches = new int[] { 1, 2, 3 };

            return new object[][]
            {
                // Same
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    true },
                // Different types
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    "not a line range",
                    false },
                // Same included, different regex
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    new PhraseGroup(string.Empty, dummyIncludedMatches),
                    false },
                // Same regex, different included
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    new PhraseGroup(dummyRegex, new int[] {4, 5, 6 }),
                    false }
            };
        }

        [Theory]
        [MemberData(nameof(GetHashCode_ReturnsSameHashCodeForIdenticalPhraseGroups_Data))]
        public void GetHashCode_ReturnsSameHashCodeForIdenticalPhraseGroups(PhraseGroup dummyPhraseGroup1,
            PhraseGroup dummyPhraseGroup2,
            bool identical)
        {
            // Act and assert
            Assert.Equal(identical, dummyPhraseGroup1.GetHashCode() == dummyPhraseGroup2.GetHashCode());
        }

        public static IEnumerable<object[]> GetHashCode_ReturnsSameHashCodeForIdenticalPhraseGroups_Data()
        {
            const string dummyRegex = "dummyRegex";
            var dummyIncludedMatches = new int[] { 1, 2, 3 };

            return new object[][]
            {
                // Same
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    true },
                // Same included, different regex
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    new PhraseGroup(string.Empty, dummyIncludedMatches),
                    false },
                // Same regex, different included
                new object[]{new PhraseGroup(dummyRegex, dummyIncludedMatches),
                    new PhraseGroup(dummyRegex, new int[] {4, 5, 6 }),
                    false }
            };
        }
    }
}
