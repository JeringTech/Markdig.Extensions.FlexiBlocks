using Jering.Markdig.Extensions.FlexiBlocks.FlexiAlertBlocks;
using Jering.Markdig.Extensions.FlexiBlocks.FlexiOptionsBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiAlertBlocks
{
    public class FlexiAlertBlockParserUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryOpenFlexiBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryOpenFlexiBlock_ResetsColumnAndLineAndReturnsBlockStateNoneIfCurrentLineDoesNotContainAValidAlertType()
        {
            // Arrange
            const int dummyInitialColumn = 3; // Arbitrary
            const int dummyInitialLineStart = 4; // Arbitrary
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyString") { Start = dummyInitialLineStart };
            dummyBlockProcessor.Column = dummyInitialColumn;
            Mock<FlexiAlertBlockParser> mockFlexiAlertBlockParser = CreateMockFlexiAlertBlockParser();
            mockFlexiAlertBlockParser.CallBase = true;
            mockFlexiAlertBlockParser.Setup(a => a.TryGetFlexiAlertType(It.IsAny<StringSlice>())).Returns((string)null);

            // Act
            BlockState result = mockFlexiAlertBlockParser.Object.TryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.None, result);
            Assert.Equal(dummyInitialLineStart, dummyBlockProcessor.Line.Start);
            Assert.Equal(dummyInitialColumn, dummyBlockProcessor.Column);
        }

        [Fact]
        public void TryOpenFlexiBlock_IfSuccessfulCreatesNewFlexiAlertBlockAndReturnsBlockStateContinueDiscard()
        {
            // Arrange
            const int dummyInitialColumn = 2;
            const int dummyInitialStart = 1;
            const string dummyAlertType = "dummyAlertType";
            var dummyStringSlice = new StringSlice("dummyString") { Start = dummyInitialStart };
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            dummyBlockProcessor.Column = dummyInitialColumn;
            var dummyFlexiAlertBlockOptions = new FlexiAlertBlockOptions();
            Mock<FlexiAlertBlockParser> mockFlexiAlertBlockParser = CreateMockFlexiAlertBlockParser();
            mockFlexiAlertBlockParser.CallBase = true;
            mockFlexiAlertBlockParser.Setup(a => a.TryGetFlexiAlertType(It.IsAny<StringSlice>())).Returns(dummyAlertType);
            mockFlexiAlertBlockParser.Setup(a => a.CreateFlexiAlertBlockOptions(dummyBlockProcessor, dummyAlertType)).Returns(dummyFlexiAlertBlockOptions);

            // Act
            BlockState result = mockFlexiAlertBlockParser.Object.TryOpenFlexiBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Equal(dummyInitialStart + 1, dummyBlockProcessor.Line.Start); // Skips '!'
            Assert.Equal(dummyInitialColumn + 1, dummyBlockProcessor.Column); // Skips '!'
            var resultFlexiAlertBlock = dummyBlockProcessor.NewBlocks.Peek() as FlexiAlertBlock;
            Assert.NotNull(resultFlexiAlertBlock);
            Assert.Same(dummyFlexiAlertBlockOptions, resultFlexiAlertBlock.FlexiAlertBlockOptions);
            Assert.Equal(dummyInitialColumn, resultFlexiAlertBlock.Column); // Includes '!'
            Assert.Equal(dummyInitialStart, resultFlexiAlertBlock.Span.Start); // Includes '!'
            Assert.Equal(dummyStringSlice.End, resultFlexiAlertBlock.Span.End);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateNoneIfCurrentLineDoesNotBeginWithExclamationMarkAndIsNotBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("dummyString");
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateBreakDiscardIfCurrentLineIsBlank()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("");
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, null);

            // Assert
            Assert.Equal(BlockState.BreakDiscard, result);
        }

        [Fact]
        public void TryContinueFlexiBlock_ReturnsBlockStateContinueIfBlockCanBeContinued()
        {
            // Arrange
            var dummyStringSlice = new StringSlice("!dummyString");
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = dummyStringSlice;
            var dummyFlexiAlertBlock = new FlexiAlertBlock(null);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            BlockState result = flexiAlertBlockParser.TryContinueFlexiBlock(dummyBlockProcessor, dummyFlexiAlertBlock);

            // Assert
            Assert.Equal(BlockState.Continue, result);
            Assert.Equal(dummyStringSlice.End, dummyFlexiAlertBlock.Span.End);
            Assert.Equal(dummyStringSlice.Start + 1, dummyBlockProcessor.Start); // Skips !
        }

        [Theory]
        [MemberData(nameof(CreateFlexiAlertBlockOptions_CreatesFlexiAlertBlockOptions_Data))]
        public void CreateFlexiAlertBlockOptions_CreatesFlexiAlertBlockOptions(
            string dummyFlexiAlertType,
            SerializableWrapper<FlexiAlertBlocksExtensionOptions> dummyExtensionOptionsWrapper,
            SerializableWrapper<FlexiAlertBlockOptions> dummyFlexiOptionsWrapper,
            SerializableWrapper<FlexiAlertBlockOptions> expectedResultWrapper)
        {
            // Arrange
            const int dummyLineIndex = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            Mock<IFlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<IFlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.
                Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex)).
                Callback<BlockProcessor, FlexiAlertBlockOptions, int>((_, a, __) =>
                {
                    if (dummyFlexiOptionsWrapper == null) { return; }
                    a.IconMarkup = dummyFlexiOptionsWrapper.Value.IconMarkup;
                    dummyFlexiOptionsWrapper.Value.Attributes.ToList().ForEach(x => a.Attributes[x.Key] = x.Value); // Overwrite default FlexiAlertBlockOptions with FlexiOptions
                });
            Mock<IOptions<FlexiAlertBlocksExtensionOptions>> mockExtensionOptionsAccessor = _mockRepository.Create<IOptions<FlexiAlertBlocksExtensionOptions>>();
            mockExtensionOptionsAccessor.Setup(e => e.Value).Returns(dummyExtensionOptionsWrapper.Value);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser(mockExtensionOptionsAccessor.Object, mockFlexiOptionsBlockService.Object);

            // Act
            FlexiAlertBlockOptions result = flexiAlertBlockParser.CreateFlexiAlertBlockOptions(dummyBlockProcessor, dummyFlexiAlertType);

            // Assert
            _mockRepository.VerifyAll();
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
                // Using FlexiAlertBlocksExtensionOptions.IconMarkups
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
                            DefaultBlockOptions = new FlexiAlertBlockOptions() {
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
            Mock<IFlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<IFlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.Setup(j => j.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyLineIndex));
            var dummyExtensionOptions = new FlexiAlertBlocksExtensionOptions()
            {
                DefaultBlockOptions = new FlexiAlertBlockOptions() { ClassNameFormat = dummyClassNameFormat }
            };
            Mock<IOptions<FlexiAlertBlocksExtensionOptions>> mockExtensionOptionsAccessor = _mockRepository.Create<IOptions<FlexiAlertBlocksExtensionOptions>>();
            mockExtensionOptionsAccessor.Setup(e => e.Value).Returns(dummyExtensionOptions);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser(mockExtensionOptionsAccessor.Object, mockFlexiOptionsBlockService.Object);

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

        [Fact]
        public void CreateFlexiAlertBlockOptions_ThrowsFlexiBlocksExceptionIfClassNameFormatIsInvalid()
        {
            // Arrange
            const string dummyClassNameFormat = "alert-{0}-{1}"; // Too many format items
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.NewBlocks.Push(new FlexiAlertBlock(null));
            var dummyExtensionOptions = new FlexiAlertBlocksExtensionOptions();
            dummyExtensionOptions.DefaultBlockOptions.ClassNameFormat = dummyClassNameFormat;
            Mock<IOptions<FlexiAlertBlocksExtensionOptions>> mockExtensionOptionsAccessor = _mockRepository.Create<IOptions<FlexiAlertBlocksExtensionOptions>>();
            mockExtensionOptionsAccessor.Setup(o => o.Value).Returns(dummyExtensionOptions);
            Mock<IFlexiOptionsBlockService> mockFlexiOptionsBlockService = _mockRepository.Create<IFlexiOptionsBlockService>();
            mockFlexiOptionsBlockService.Setup(f => f.TryPopulateOptions(dummyBlockProcessor, It.IsAny<FlexiAlertBlockOptions>(), dummyBlockProcessor.LineIndex));
            FlexiAlertBlockParser testSubject = CreateFlexiAlertBlockParser(mockExtensionOptionsAccessor.Object, mockFlexiOptionsBlockService.Object);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => testSubject.CreateFlexiAlertBlockOptions(dummyBlockProcessor, "dummyAlertType"));
            Assert.Equal(
                string.Format(
                    Strings.FlexiBlocksException_InvalidFlexiBlock,
                    nameof(FlexiAlertBlock),
                    1,
                    0,
                    string.Format(Strings.FlexiBlocksException_InvalidFormat, nameof(FlexiAlertBlockOptions.ClassNameFormat), dummyClassNameFormat)),
                result.Message);
        }

        [Theory]
        [MemberData(nameof(TryGetFlexiAlertType_ReturnsNullIfLineContainsIllegalCharacters_Data))]
        public void TryGetFlexiAlertType_ReturnsNullIfLineContainsIllegalCharactersOrHasNoCharacters(string dummyString)
        {
            // Arrange
            var dummyStringSlice = new StringSlice(dummyString);
            FlexiAlertBlockParser flexiAlertBlockParser = CreateFlexiAlertBlockParser();

            // Act
            string result = flexiAlertBlockParser.TryGetFlexiAlertType(dummyStringSlice);

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
            string result = flexiAlertBlockParser.TryGetFlexiAlertType(dummyStringSlice);

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

        private FlexiAlertBlockParser CreateFlexiAlertBlockParser(IOptions<FlexiAlertBlocksExtensionOptions> extensionOptionsAccessor = null,
            IFlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return new FlexiAlertBlockParser(
                extensionOptionsAccessor,
                flexiOptionsBlockService ?? new FlexiOptionsBlockService(null));
        }

        private Mock<FlexiAlertBlockParser> CreateMockFlexiAlertBlockParser(IOptions<FlexiAlertBlocksExtensionOptions> extensionOptionsAccessor = null,
            IFlexiOptionsBlockService flexiOptionsBlockService = null)
        {
            return _mockRepository.Create<FlexiAlertBlockParser>(
                extensionOptionsAccessor,
                flexiOptionsBlockService ?? new FlexiOptionsBlockService(null));
        }
    }
}
