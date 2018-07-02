using FlexiBlocks.Alerts;
using FlexiBlocks.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlexiBlocks.Tests.Alerts
{
    public class FlexiAlertBlockParserIntegrationTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            BlockState result = alertBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpen_ResetsCurrentLineAndReturnsBlockStateNoneIfCurrentLineDoesNotHaveAValidAlertTypeName()
        {
            // Arrange
            const int dummyLineStart = 1;
            var dummyStringSlice = new StringSlice("dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Line.Start = dummyLineStart;
            Mock<FlexiAlertBlockParser> mockAlertBlockParser = CreateMockAlertBlockParser();
            mockAlertBlockParser.CallBase = true;
            mockAlertBlockParser.Setup(a => a.TryGetAlertTypeName(It.IsAny<StringSlice>())).Returns((string)null);

            // Act
            BlockState result = mockAlertBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
            Assert.Equal(dummyLineStart, dummyBlockProcessor.Line.Start);
        }

        [Fact]
        public void TryOpen_IfSuccessfulCreatesNewAlertBlockAndReturnsBlockStateContinueDiscard()
        {
            // Arrange
            const int dummyInitialColumn = 2;
            const int dummyInitialStart = 1;
            const string dummyAlertTypeName = "dummyAlertTypeName";
            var dummyStringSlice = new StringSlice("dummyString");
            var dummyAlertBlockOptions = new FlexiAlertBlockOptions();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Column = dummyInitialColumn;
            dummyBlockProcessor.Line.Start = dummyInitialStart;
            Mock<FlexiAlertBlockParser> mockAlertBlockParser = CreateMockAlertBlockParser();
            mockAlertBlockParser.CallBase = true;
            mockAlertBlockParser.Setup(a => a.TryGetAlertTypeName(It.IsAny<StringSlice>())).Returns(dummyAlertTypeName);
            mockAlertBlockParser.Setup(a => a.CreateAlertBlockOptions(dummyBlockProcessor, dummyAlertTypeName)).Returns(dummyAlertBlockOptions);

            // Act
            BlockState result = mockAlertBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyInitialStart + 1, dummyBlockProcessor.Line.Start); // Skips '!'
            var resultAlertBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiAlertBlock;
            Assert.NotNull(resultAlertBlock);
            Assert.Same(dummyAlertBlockOptions, resultAlertBlock.AlertBlockOptions);
            Assert.Equal(dummyInitialColumn, resultAlertBlock.Column);
            Assert.Equal(dummyInitialStart, resultAlertBlock.Span.Start); // Span includes '!'
            Assert.Equal(dummyStringSlice.End, resultAlertBlock.Span.End);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            BlockState result = alertBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateNoneIfCurrentLineDoesNotBeginWithExclamationMark()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            BlockState result = alertBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateBreakDiscardIfCurrentLineIsBlank()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            BlockState result = alertBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.BreakDiscard, result);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueIfSuccessful()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("!dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyAlertBlock = new FlexiAlertBlock(null);
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            BlockState result = alertBlockParser.TryContinue(dummyBlockProcessor, dummyAlertBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyStringSlice.End, dummyAlertBlock.Span.End);
        }

        [Theory]
        [MemberData(nameof(CreateAlertBlockOptions_CreatesAlertBlockOptions_Data))]
        public void CreateAlertBlockOptions_CreatesAlertBlockOptions(
            string dummyAlertTypeName,
            FlexiAlertsExtensionOptions dummyAlertsExtensionOptions,
            FlexiAlertBlockOptions dummyJsonAlertBlockOptions,
            FlexiAlertBlockOptions expectedResult)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.
                Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex)).
                Callback<BlockProcessor, FlexiAlertBlockOptions, int>((_, a, __) =>
                {
                    if (dummyJsonAlertBlockOptions == null) { return; };
                    a.IconMarkup = dummyJsonAlertBlockOptions.IconMarkup;
                    dummyJsonAlertBlockOptions.Attributes.ToList().ForEach(x => a.Attributes[x.Key] = x.Value); // Overwrite default AlertBlockOptions with JSON AlertBlockOptions
                });
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser(dummyAlertsExtensionOptions, mockJsonOptionsService.Object);

            // Act
            FlexiAlertBlockOptions result = alertBlockParser.CreateAlertBlockOptions(dummyBlockProcessor, dummyAlertTypeName);

            // Assert
            Assert.Equal(expectedResult.IconMarkup, result.IconMarkup);
            Assert.Equal(expectedResult.Attributes, result.Attributes); // xunit checks KeyPairValues when determing equality of dictionaries - https://github.com/xunit/xunit/blob/master/test/test.xunit.assert/Asserts/CollectionAssertsTests.cs#L648
        }

        public static IEnumerable<object[]> CreateAlertBlockOptions_CreatesAlertBlockOptions_Data()
        {
            const string dummyAlertTypeName = "dummyAlertTypeName";
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyClass = "dummyClass";

            return new object[][]
            {
                // Using AlertsExtensionOptions.IconMarkups
                new object[] {
                    dummyAlertTypeName,
                    new FlexiAlertsExtensionOptions() {
                        IconMarkups = new Dictionary<string, string>() {
                            { dummyAlertTypeName, dummyIconMarkup}
                        }
                    },
                    null,
                    new FlexiAlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new HtmlAttributeDictionary(){
                            { "class", $"alert-{dummyAlertTypeName.ToLowerInvariant()}" }
                        }
                    }
                },
                // Using JsonOptions
                new object[] {
                    dummyAlertTypeName,
                    new FlexiAlertsExtensionOptions(),
                    new FlexiAlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new HtmlAttributeDictionary(){ { "class", dummyClass } }
                    },
                    new FlexiAlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new HtmlAttributeDictionary(){
                            { "class", $"{dummyClass} alert-{dummyAlertTypeName.ToLowerInvariant()}" }
                        }
                    }
                },
                // Using Default AlertBlockOptions
                new object[] {
                    dummyAlertTypeName,
                    new FlexiAlertsExtensionOptions() {
                        DefaultAlertBlockOptions = new FlexiAlertBlockOptions() {
                            IconMarkup = dummyIconMarkup,
                            Attributes = new HtmlAttributeDictionary(){ { "class", dummyClass } }
                        },
                    },
                    null,
                    new FlexiAlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new HtmlAttributeDictionary(){
                            { "class", $"{dummyClass} alert-{dummyAlertTypeName.ToLowerInvariant()}" }
                        }
                    }
                },
            };
        }

        [Theory]
        [MemberData(nameof(CreateAlertBlockOptions_GeneratesValueOfClassAttribute_Data))]
        public void CreateAlertBlockOptions_GeneratesValueOfClassAttribute(
            string dummyAlertTypeName,
            string dummyClassNameFormat,
            string expectedClassValue)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<FlexiOptionsService> mockJsonOptionsService = _mockRepository.Create<FlexiOptionsService>();
            mockJsonOptionsService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex));
            var dummyAlertsExtensionOptions = new FlexiAlertsExtensionOptions()
            {
                DefaultAlertBlockOptions = new FlexiAlertBlockOptions() { ClassNameFormat = dummyClassNameFormat }
            };
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser(dummyAlertsExtensionOptions, mockJsonOptionsService.Object);

            // Act
            FlexiAlertBlockOptions result = alertBlockParser.CreateAlertBlockOptions(dummyBlockProcessor, dummyAlertTypeName);

            // Assert
            result.Attributes.TryGetValue("class", out string resultClassValue);
            Assert.Equal(expectedClassValue, resultClassValue);
        }

        public static IEnumerable<object[]> CreateAlertBlockOptions_GeneratesValueOfClassAttribute_Data()
        {
            const string dummyAlertTypeName = "dummyAlertTypeName";

            return new object[][]
            {
                new object[]{ dummyAlertTypeName, string.Empty, null},
                new object[]{ dummyAlertTypeName, " ", null},
                new object[]{ dummyAlertTypeName, null, null},
                new object[]{ dummyAlertTypeName, "dummy-format-{0}", $"dummy-format-{dummyAlertTypeName.ToLowerInvariant()}"}
            };
        }

        [Theory]
        [MemberData(nameof(TryGetAlertTypeName_ReturnsNullIfLineContainsIllegalCharacters_Data))]
        public void TryGetAlertTypeName_ReturnsNullIfLineContainsIllegalCharactersOrHasNoCharacters(string dummyString)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyString);
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            string result = alertBlockParser.TryGetAlertTypeName(dummyStringSlice);

            // Assert
            Assert.Null(result);
        }

        public static IEnumerable<object[]> TryGetAlertTypeName_ReturnsNullIfLineContainsIllegalCharacters_Data()
        {
            return new object[][]
            {
                new object[]{ "dummy@String" },
                new object[]{ " dummyString" }, // Spaces are illegal, so leading spaces aren't allowed
                new object[]{ "" } // Must have at least 1 character
            };
        }

        [Theory]
        [MemberData(nameof(TryGetAlertTypeName_ReturnsAlertTypeNameIfSuccessful_Data))]
        public void TryGetAlertTypeName_ReturnsAlertTypeNameIfSuccessful(string dummyString, string expectedAlertTypeName)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyString);
            FlexiAlertBlockParser alertBlockParser = CreateAlertBlockParser();

            // Act
            string result = alertBlockParser.TryGetAlertTypeName(dummyStringSlice);

            // Assert
            Assert.Equal(expectedAlertTypeName, result);
        }

        public static IEnumerable<object[]> TryGetAlertTypeName_ReturnsAlertTypeNameIfSuccessful_Data()
        {
            return new object[][]
            {
                new object[]{ "dummyString", "dummystring" }, // both uppercase and lowercase allowed, converted to lowercase since css class names are case insensitive
                new object[]{ "dummy-string", "dummy-string" }, // - allowed
                new object[]{ "dummy_string", "dummy_string" } // _ allowed
            };
        }

        private FlexiAlertBlockParser CreateAlertBlockParser(FlexiAlertsExtensionOptions alertsExtensionOptions = null,
            FlexiOptionsService jsonOptionsService = null)
        {
            return new FlexiAlertBlockParser(
                alertsExtensionOptions ?? new FlexiAlertsExtensionOptions(),
                jsonOptionsService ?? new FlexiOptionsService());
        }

        private Mock<FlexiAlertBlockParser> CreateMockAlertBlockParser(FlexiAlertsExtensionOptions alertsExtensionOptions = null,
            FlexiOptionsService jsonOptionsService = null)
        {
            return _mockRepository.Create<FlexiAlertBlockParser>(
                alertsExtensionOptions ?? new FlexiAlertsExtensionOptions(),
                jsonOptionsService ?? new FlexiOptionsService());
        }
    }
}
