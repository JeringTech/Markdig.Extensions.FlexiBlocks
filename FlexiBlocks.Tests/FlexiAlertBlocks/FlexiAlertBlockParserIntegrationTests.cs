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
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryOpen(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpen_ResetsCurrentLineAndReturnsBlockStateNoneIfCurrentLineDoesNotContainAValidAlertType()
        {
            // Arrange
            const int dummyLineStart = 1;
            var dummyStringSlice = new StringSlice("dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Line.Start = dummyLineStart;
            Mock<FlexiAlertBlockParser> mockFlexiAlertBlockParser = CreateMockFlexiAlertBlockParser();
            mockFlexiAlertBlockParser.CallBase = true;
            mockFlexiAlertBlockParser.Setup(a => a.TryGetFlexiAlertBlockType(It.IsAny<StringSlice>())).Returns((string)null);

            // Act
            BlockState result = mockFlexiAlertBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
            Assert.Equal(dummyLineStart, dummyBlockProcessor.Line.Start);
        }

        [Fact]
        public void TryOpen_IfSuccessfulCreatesNewFlexiAlertBlockAndReturnsBlockStateContinueDiscard()
        {
            // Arrange
            const int dummyInitialColumn = 2;
            const int dummyInitialStart = 1;
            const string dummyAlertType = "dummyAlertType";
            var dummyStringSlice = new StringSlice("dummyString");
            var dummyFlexiAlertBlockOptions = new FlexiAlertBlockOptions();
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Column = dummyInitialColumn;
            dummyBlockProcessor.Line.Start = dummyInitialStart;
            Mock<FlexiAlertBlockParser> mockFlexiAlertBlockParser = CreateMockFlexiAlertBlockParser();
            mockFlexiAlertBlockParser.CallBase = true;
            mockFlexiAlertBlockParser.Setup(a => a.TryGetFlexiAlertBlockType(It.IsAny<StringSlice>())).Returns(dummyAlertType);
            mockFlexiAlertBlockParser.Setup(a => a.CreateFlexiAlertBlockOptions(dummyBlockProcessor, dummyAlertType)).Returns(dummyFlexiAlertBlockOptions);

            // Act
            BlockState result = mockFlexiAlertBlockParser.Object.TryOpen(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyInitialStart + 1, dummyBlockProcessor.Line.Start); // Skips '!'
            var resultFlexiAlertBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiAlertBlock;
            Assert.NotNull(resultFlexiAlertBlock);
            Assert.Same(dummyFlexiAlertBlockOptions, resultFlexiAlertBlock.FlexiAlertBlockOptions);
            Assert.Equal(dummyInitialColumn, resultFlexiAlertBlock.Column);
            Assert.Equal(dummyInitialStart, resultFlexiAlertBlock.Span.Start); // Span includes '!'
            Assert.Equal(dummyStringSlice.End, resultFlexiAlertBlock.Span.End);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinue(dummyBlockProcessor, null);

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
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinue(dummyBlockProcessor, null);

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
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinue(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.BreakDiscard, result);
        }

        [Fact]
        public void TryContinue_ReturnsBlockStateContinueIfBlockCanBeContinued()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("!dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyFlexiAlertBlock = new FlexiAlertBlock(null);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinue(dummyBlockProcessor, dummyFlexiAlertBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyStringSlice.End, dummyFlexiAlertBlock.Span.End);
        }

        [Theory]
        [MemberData(nameof(CreateFlexiAlertBlockOptions_CreatesFlexiAlertBlockOptions_Data))]
        public void CreateFlexiAlertBlockOptions_CreatesFlexiAlertBlockOptions(
            string dummyFlexiAlertType,
            SerializableWrapper<FlexiAlertBlocksExtensionOptions> dummyFlexiAlertsExtensionOptionsWrapper,
            SerializableWrapper<FlexiAlertBlockOptions> dummyFlexiOptionsWrapper,
            SerializableWrapper<FlexiAlertBlockOptions> expectedResultWrapper)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<FlexiOptionBlocksService> mockFlexiOptionsService = _mockRepository.Create<FlexiOptionBlocksService>();
            mockFlexiOptionsService.
                Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex)).
                Callback<BlockProcessor, FlexiAlertBlockOptions, int>((_, a, __) =>
                {
                    if (dummyFlexiOptionsWrapper == null) { return; }
                    a.IconMarkup = dummyFlexiOptionsWrapper.Value.IconMarkup;
                    dummyFlexiOptionsWrapper.Value.Attributes.ToList().ForEach(x => a.Attributes[x.Key] = x.Value); // Overwrite default FlexiAlertBlockOptions with FlexiOptions
                });
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser(dummyFlexiAlertsExtensionOptionsWrapper.Value, mockFlexiOptionsService.Object);

            // Act
            FlexiAlertBlockOptions result = flexiAlertBlockParser.CreateFlexiAlertBlockOptions(dummyBlockProcessor, dummyFlexiAlertType);

            // Assert
            Assert.Equal(expectedResultWrapper.Value.IconMarkup, result.IconMarkup);
            Assert.Equal(expectedResultWrapper.Value.Attributes, result.Attributes); // xunit checks KeyPairValues when determing equality of dictionaries - https://github.com/xunit/xunit/blob/master/test/test.xunit.assert/Asserts/CollectionAssertsTests.cs#L648
        }

        public static IEnumerable<object[]> CreateFlexiAlertBlockOptions_CreatesFlexiAlertBlockOptions_Data()
        {
            const string dummyFlexiAlertType = "dummyFlexiAlertType";
            const string dummyIconMarkup = "dummyIconMarkup";
            const string dummyClass = "dummyClass";

            return new object[][]
            {
                // Using FlexiAlertsExtensionOptions.IconMarkups
                new object[] {
                    dummyFlexiAlertType,
                    new SerializableWrapper<FlexiAlertBlocksExtensionOptions>(
                        new FlexiAlertBlocksExtensionOptions() {
                            IconMarkups = new Dictionary<string, string>() {
                                { dummyFlexiAlertType, dummyIconMarkup}
                            }
                        }
                    ),
                    null,
                    new SerializableWrapper<FlexiAlertBlockOptions>(
                        new FlexiAlertBlockOptions() {
                            IconMarkup = dummyIconMarkup,
                            Attributes = new HtmlAttributeDictionary(){
                                { "class", $"fab-{dummyFlexiAlertType.ToLowerInvariant()}" }
                            }
                        }
                    )
                },
                // Using JsonOptions
                new object[] {
                    dummyFlexiAlertType,
                    new SerializableWrapper<FlexiAlertBlocksExtensionOptions>(
                        new FlexiAlertBlocksExtensionOptions()
                    ),
                    new SerializableWrapper<FlexiAlertBlockOptions>(
                        new FlexiAlertBlockOptions() {
                            IconMarkup = dummyIconMarkup,
                            Attributes = new HtmlAttributeDictionary(){ { "class", dummyClass } }
                        }
                    ),
                    new SerializableWrapper<FlexiAlertBlockOptions>(
                        new FlexiAlertBlockOptions() {
                            IconMarkup = dummyIconMarkup,
                            Attributes = new HtmlAttributeDictionary(){
                                { "class", $"{dummyClass} fab-{dummyFlexiAlertType.ToLowerInvariant()}" }
                            }
                        }
                    )
                },
                // Using Default AlertBlockOptions
                new object[] {
                    dummyFlexiAlertType,
                    new SerializableWrapper<FlexiAlertBlocksExtensionOptions>(
                        new FlexiAlertBlocksExtensionOptions() {
                            DefaultFlexiAlertBlockOptions = new FlexiAlertBlockOptions() {
                                IconMarkup = dummyIconMarkup,
                                Attributes = new HtmlAttributeDictionary(){ { "class", dummyClass } }
                            },
                        }
                    ),
                    null,
                    new SerializableWrapper<FlexiAlertBlockOptions>(
                        new FlexiAlertBlockOptions() {
                            IconMarkup = dummyIconMarkup,
                            Attributes = new HtmlAttributeDictionary(){
                                { "class", $"{dummyClass} fab-{dummyFlexiAlertType.ToLowerInvariant()}" }
                            }
                        }
                    )
                },
            };
        }

        [Theory]
        [MemberData(nameof(CreateFlexiAlertBlockOptions_GeneratesValueOfClassAttribute_Data))]
        public void CreateFlexiAlertBlockOptions_GeneratesValueOfClassAttribute(
            string dummyFlexiAlertType,
            string dummyClassNameFormat,
            string expectedClassValue)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<FlexiOptionBlocksService> mockFlexiOptionsService = _mockRepository.Create<FlexiOptionBlocksService>();
            mockFlexiOptionsService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex));
            var dummyFlexiAlertsExtensionOptions = new FlexiAlertBlocksExtensionOptions()
            {
                DefaultFlexiAlertBlockOptions = new FlexiAlertBlockOptions() { ClassNameFormat = dummyClassNameFormat }
            };
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser(dummyFlexiAlertsExtensionOptions, mockFlexiOptionsService.Object);

            // Act
            FlexiAlertBlockOptions result = flexiAlertBlockParser.CreateFlexiAlertBlockOptions(dummyBlockProcessor, dummyFlexiAlertType);

            // Assert
            result.Attributes.TryGetValue("class", out string resultClassValue);
            Assert.Equal(expectedClassValue, resultClassValue);
        }

        public static IEnumerable<object[]> CreateFlexiAlertBlockOptions_GeneratesValueOfClassAttribute_Data()
        {
            const string dummyFlexiAlertType = "dummyFlexiAlertType";

            return new object[][]
            {
                new object[]{ dummyFlexiAlertType, string.Empty, null},
                new object[]{ dummyFlexiAlertType, " ", null},
                new object[]{ dummyFlexiAlertType, null, null},
                new object[]{ dummyFlexiAlertType, "dummy-format-{0}", $"dummy-format-{dummyFlexiAlertType.ToLowerInvariant()}"}
            };
        }

        [Theory]
        [MemberData(nameof(TryGetFlexiAlertType_ReturnsNullIfLineContainsIllegalCharacters_Data))]
        public void TryGetFlexiAlertType_ReturnsNullIfLineContainsIllegalCharactersOrHasNoCharacters(string dummyString)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyString);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            string result = flexiAlertBlockParser.TryGetFlexiAlertBlockType(dummyStringSlice);

            // Assert
            Assert.Null(result);
        }

        public static IEnumerable<object[]> TryGetFlexiAlertType_ReturnsNullIfLineContainsIllegalCharacters_Data()
        {
            return new object[][]
            {
                new object[]{ "dummy@String" },
                new object[]{ " dummyString" }, // Spaces are illegal, so leading spaces aren't allowed
                new object[]{ "" } // Must have at least 1 character
            };
        }

        [Theory]
        [MemberData(nameof(TryGetFlexiAlertType_ReturnsFlexiAlertTypeIfSuccessful_Data))]
        public void TryGetFlexiAlertType_ReturnsFlexiAlertTypeIfSuccessful(string dummyString, string expectedFlexiAlertType)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyString);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            string result = flexiAlertBlockParser.TryGetFlexiAlertBlockType(dummyStringSlice);

            // Assert
            Assert.Equal(expectedFlexiAlertType, result);
        }

        public static IEnumerable<object[]> TryGetFlexiAlertType_ReturnsFlexiAlertTypeIfSuccessful_Data()
        {
            return new object[][]
            {
                new object[]{ "dummyString", "dummystring" }, // both uppercase and lowercase allowed, converted to lowercase since css class names are case insensitive
                new object[]{ "dummy-string", "dummy-string" }, // - allowed
                new object[]{ "dummy_string", "dummy_string" } // _ allowed
            };
        }

        private FlexiAlertBlockParser CreateFlexiAlertBlockParser(FlexiAlertBlocksExtensionOptions flexiAlertsExtensionOptions = null,
            FlexiOptionBlocksService flexiOptionsService = null)
        {
            return new FlexiAlertBlockParser(
                flexiAlertsExtensionOptions ?? new FlexiAlertBlocksExtensionOptions(),
                flexiOptionsService ?? new FlexiOptionBlocksService());
        }

        private Mock<FlexiAlertBlockParser> CreateMockFlexiAlertBlockParser(FlexiAlertBlocksExtensionOptions flexiAlertsExtensionOptions = null,
            FlexiOptionBlocksService flexiOptionsService = null)
        {
            return _mockRepository.Create<FlexiAlertBlockParser>(
                flexiAlertsExtensionOptions ?? new FlexiAlertBlocksExtensionOptions(),
                flexiOptionsService ?? new FlexiOptionBlocksService());
        }
    }
}
