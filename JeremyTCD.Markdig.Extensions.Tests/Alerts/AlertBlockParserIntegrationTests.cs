using JeremyTCD.Markdig.Extensions.Alerts;
using JeremyTCD.Markdig.Extensions.JsonOptions;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JeremyTCD.Markdig.Extensions.Tests.Alerts
{
    public class AlertBlockParserIntegrationTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpen_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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
            Mock<AlertBlockParser> mockAlertBlockParser = CreateMockAlertBlockParser();
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
            var dummyAlertBlockOptions = new AlertBlockOptions();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Column = dummyInitialColumn;
            dummyBlockProcessor.Line.Start = dummyInitialStart;
            Mock<AlertBlockParser> mockAlertBlockParser = CreateMockAlertBlockParser();
            mockAlertBlockParser.CallBase = true;
            mockAlertBlockParser.Setup(a => a.TryGetAlertTypeName(It.IsAny<StringSlice>())).Returns(dummyAlertTypeName);
            mockAlertBlockParser.Setup(a => a.CreateAlertBlockOptions(dummyBlockProcessor, dummyAlertTypeName)).Returns(dummyAlertBlockOptions);

            // Act
            BlockState result = mockAlertBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyInitialStart + 1, dummyBlockProcessor.Line.Start); // Skips '!'
            var resultAlertBlock = dummyBlockProcessor.NewBlocks.Peek() as AlertBlock;
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
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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
            var dummyAlertBlock = new AlertBlock(null);
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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
            AlertsExtensionOptions dummyAlertsExtensionOptions,
            AlertBlockOptions dummyJsonAlertBlockOptions,
            AlertBlockOptions expectedResult)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<JsonOptionsService> mockJsonOptionsService = _mockRepository.Create<JsonOptionsService>();
            mockJsonOptionsService.
                Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<AlertBlockOptions>())).
                Callback<BlockProcessor, AlertBlockOptions>((_, a) =>
                {
                    if (dummyJsonAlertBlockOptions == null) { return; };
                    a.IconMarkup = dummyJsonAlertBlockOptions.IconMarkup;
                    dummyJsonAlertBlockOptions.Attributes.ToList().ForEach(x => a.Attributes[x.Key] = x.Value); // Overwrite default AlertBlockOptions with JSON AlertBlockOptions
                });
            AlertBlockParser alertBlockParser = CreateAlertBlockParser(dummyAlertsExtensionOptions, mockJsonOptionsService.Object);

            // Act
            AlertBlockOptions result = alertBlockParser.CreateAlertBlockOptions(dummyBlockProcessor, dummyAlertTypeName);

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
                    new AlertsExtensionOptions() {
                        IconMarkups = new Dictionary<string, string>() {
                            { dummyAlertTypeName, dummyIconMarkup}
                        }
                    },
                    null,
                    new AlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new Dictionary<string, string>(){
                            { "class", $"alert-{dummyAlertTypeName}" }
                        }
                    }
                },
                // Using JsonOptions
                new object[] {
                    dummyAlertTypeName,
                    new AlertsExtensionOptions(),
                    new AlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new Dictionary<string, string>(){ { "class", dummyClass } }
                    },
                    new AlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new Dictionary<string, string>(){
                            { "class", $"{dummyClass} alert-{dummyAlertTypeName}" }
                        }
                    }
                },
                // Using Default AlertBlockOptions
                new object[] {
                    dummyAlertTypeName,
                    new AlertsExtensionOptions() {
                        DefaultAlertBlockOptions = new AlertBlockOptions() {
                            IconMarkup = dummyIconMarkup,
                            Attributes = new Dictionary<string, string>(){ { "class", dummyClass } }
                        },
                    },
                    null,
                    new AlertBlockOptions() {
                        IconMarkup = dummyIconMarkup,
                        Attributes = new Dictionary<string, string>(){
                            { "class", $"{dummyClass} alert-{dummyAlertTypeName}" }
                        }
                    }
                },
            };
        }

        [Theory]
        [MemberData(nameof(TryGetAlertTypeName_ReturnsNullIfLineContainsIllegalCharacters_Data))]
        public void TryGetAlertTypeName_ReturnsNullIfLineContainsIllegalCharactersOrHasNoCharacters(string dummyString)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyString);
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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
            AlertBlockParser alertBlockParser = CreateAlertBlockParser();

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

        private AlertBlockParser CreateAlertBlockParser(AlertsExtensionOptions alertsExtensionOptions = null,
            JsonOptionsService jsonOptionsService = null)
        {
            return new AlertBlockParser(
                alertsExtensionOptions ?? new AlertsExtensionOptions(),
                jsonOptionsService ?? new JsonOptionsService());
        }

        private Mock<AlertBlockParser> CreateMockAlertBlockParser(AlertsExtensionOptions alertsExtensionOptions = null,
            JsonOptionsService jsonOptionsService = null)
        {
            return _mockRepository.Create<AlertBlockParser>(
                alertsExtensionOptions ?? new AlertsExtensionOptions(),
                jsonOptionsService ?? new JsonOptionsService());
        }
    }
}
