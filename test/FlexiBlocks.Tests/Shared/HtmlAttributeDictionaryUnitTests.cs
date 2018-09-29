using System.Collections.Generic;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class HtmlAttributeDictionaryUnitTests
    {
        [Theory]
        [MemberData(nameof(Add_AppendsValueToClassValueIfItAlreadyExists_Data))]
        public void Add_AppendsValueToClassValueIfItAlreadyExists(string dummyValue, string dummyExistingValue, string dummyNewValue)
        {
            // Arrange
            var htmlAttributeDictionary = new HtmlAttributeDictionary()
            {
                { "class", dummyExistingValue }
            };

            // Act
            htmlAttributeDictionary.Add("class", dummyValue);

            // Assert
            Assert.Equal(dummyNewValue, htmlAttributeDictionary["class"]);
        }

        public static IEnumerable<object[]> Add_AppendsValueToClassValueIfItAlreadyExists_Data()
        {
            const string dummyExistingValue = "dummyExistingValue";
            const string dummyValue = "dummyValue";

            return new object[][]
            {
                new object[]{dummyValue, " ", dummyValue}, // Replaces whitespace
                new object[]{dummyValue, null, dummyValue}, // Replaces null
                new object[]{dummyValue, dummyExistingValue, $"{dummyExistingValue} {dummyValue}"}, // Gets appended to an existing value
            };
        }

        [Theory]
        [MemberData(nameof(Add_HasTheSameBehaviourAsADictionarysIndexerForKeysOtherThanClassOrWhenKeyIsClassAndTheClassKeyDoesntAlreadyExist_Data))]
        public void Add_HasTheSameBehaviourAsADictionarysIndexerForKeysOtherThanClassOrWhenKeyIsClassAndTheClassKeyDoesntAlreadyExist(
            HtmlAttributeDictionary dummyAttributes,
            List<(string key, string value)> dummyKvpsToAdd,
            HtmlAttributeDictionary dummyExpectedResult)
        {
            // Act
            foreach((string key, string value) in dummyKvpsToAdd)
            {
                dummyAttributes.Add(key, value);
            }

            // Assert
            Assert.Equal(dummyExpectedResult, dummyAttributes);
        }

        public static IEnumerable<object[]> Add_HasTheSameBehaviourAsADictionarysIndexerForKeysOtherThanClassOrWhenKeyIsClassAndTheClassKeyDoesntAlreadyExist_Data()
        {
            const string dummyKey = "dummyKey";
            const string dummyValue = "dummyValue";

            return new object[][]
            {
                // HtmlAttributeDictionary.Add overwrites existing values, default Dictionary.Add throws if key already exists
                new object[]{
                    new HtmlAttributeDictionary()
                    {
                        { dummyKey, dummyValue }
                    },
                    new List<(string key, string value)>{(dummyKey, "dummyNewValue")},
                    new HtmlAttributeDictionary()
                    {
                        { dummyKey, "dummyNewValue" }
                    },
                },
                // HtmlAttributeDictionary.Add adds new kvps as per normal
                new object[]{
                    new HtmlAttributeDictionary(),
                    new List<(string key, string value)>{(dummyKey, dummyValue), ("class", dummyValue)},
                    new HtmlAttributeDictionary()
                    {
                        { dummyKey, dummyValue },
                        { "class", dummyValue }
                    },
                }
            };
        }

        [Theory]
        [MemberData(nameof(Indexer_AppendsValueToClassValueIfItAlreadyExists_Data))]
        public void Indexer_AppendsValueToClassValueIfItAlreadyExists(string dummyValue, string dummyExistingValue, string dummyNewValue)
        {
            // Arrange
            var htmlAttributeDictionary = new HtmlAttributeDictionary()
            {
                { "class", dummyExistingValue }
            };

            // Act
            htmlAttributeDictionary["class"] = dummyValue;

            // Assert
            Assert.Equal(dummyNewValue, htmlAttributeDictionary["class"]);
        }

        public static IEnumerable<object[]> Indexer_AppendsValueToClassValueIfItAlreadyExists_Data()
        {
            const string dummyExistingValue = "dummyExistingValue";
            const string dummyValue = "dummyValue";

            return new object[][]
            {
                new object[]{dummyValue, " ", dummyValue}, // Replaces whitespace
                new object[]{dummyValue, null, dummyValue}, // Replaces null
                new object[]{dummyValue, dummyExistingValue, $"{dummyExistingValue} {dummyValue}"}, // Gets appended to an existing value
            };
        }

        [Theory]
        [MemberData(nameof(Indexer_HasTheSameBehaviourAsADictionarysIndexerForKeysOtherThanClassOrWhenKeyIsClassAndTheClassKeyDoesntAlreadyExist_Data))]
        public void Indexer_HasTheSameBehaviourAsADictionarysIndexerForKeysOtherThanClassOrWhenKeyIsClassAndTheClassKeyDoesntAlreadyExist(
            HtmlAttributeDictionary dummyAttributes,
            List<(string key, string value)> dummyKvpsToAdd,
            HtmlAttributeDictionary dummyExpectedResult)
        {
            // Act
            foreach ((string key, string value) in dummyKvpsToAdd)
            {
                dummyAttributes[key] = value;
            }

            // Assert
            Assert.Equal(dummyExpectedResult, dummyAttributes);
        }

        public static IEnumerable<object[]> Indexer_HasTheSameBehaviourAsADictionarysIndexerForKeysOtherThanClassOrWhenKeyIsClassAndTheClassKeyDoesntAlreadyExist_Data()
        {
            const string dummyKey = "dummyKey";
            const string dummyValue = "dummyValue";

            return new object[][]
            {
                // HtmlAttributeDictionary.Add overwrites existing values, default Dictionary.Add throws if key already exists
                new object[]{
                    new HtmlAttributeDictionary()
                    {
                        { dummyKey, dummyValue }
                    },
                    new List<(string key, string value)>{(dummyKey, "dummyNewValue")},
                    new HtmlAttributeDictionary()
                    {
                        { dummyKey, "dummyNewValue" }
                    },
                },
                // HtmlAttributeDictionary.Add adds new kvps as per normal
                new object[]{
                    new HtmlAttributeDictionary(),
                    new List<(string key, string value)>{(dummyKey, dummyValue), ("class", dummyValue)},
                    new HtmlAttributeDictionary()
                    {
                        { dummyKey, dummyValue },
                        { "class", dummyValue }
                    },
                }
            };
        }
    }
}
