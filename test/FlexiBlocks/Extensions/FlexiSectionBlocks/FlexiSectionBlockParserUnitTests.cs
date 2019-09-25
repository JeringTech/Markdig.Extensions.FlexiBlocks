using Jering.Markdig.Extensions.FlexiBlocks.FlexiSectionBlocks;
using Markdig.Helpers;
using Markdig.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.FlexiSectionBlocks
{
    public class FlexiSectionBlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Constructor_SetsOpeningCharacters()
        {
            // Act
            var testSubject = new FlexiSectionBlockParser(_mockRepository.Create<IFlexiSectionBlockFactory>().Object);

            // Assert
            Assert.Single(testSubject.OpeningCharacters);
            Assert.Equal('#', testSubject.OpeningCharacters[0]);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionIfFlexiSectionBlockFactoryIsNull()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new FlexiSectionBlockParser(null));
        }

        [Fact]
        public void TryOpenBlock_ReturnsBlockStateNoneIfCurrentLineHasCodeIndent()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Column = 4; // IsCodeIndent is an expression bodied member that derives its value from Column
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        [Theory]
        [MemberData(nameof(TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotStartWithTheExpectedCharacters_Data))]
        public void TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotStartWithTheExpectedCharacters(string dummyLineText)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            Assert.Equal(BlockState.None, result);
        }

        public static IEnumerable<object[]> TryOpenBlock_ReturnsBlockStateNoneIfLineDoesNotStartWithTheExpectedCharacters_Data()
        {
            return new object[][]
            {
                // Too many (> 6) hashes
                new object[]
                {
                    "####### Dummy"
                },
                // Character after hashes is not a space/tab/eol
                new object[]
                {
                    "###Dummy"
                },
            };
        }

        [Fact]
        public void TryOpenBlock_ThrowsBlockExceptionIfAnExceptionIsThrownWhileCreatingTheBlock()
        {
            // Arrange
            const int dummyLineIndex = 6;
            const int dummyColumn = 3;
            const int dummyLevel = 1;
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice("# Dummy");
            dummyBlockProcessor.LineIndex = dummyLineIndex;
            dummyBlockProcessor.Column = dummyColumn;
            Mock<IFlexiSectionBlockFactory> mockFlexiSectionBlockFactory = _mockRepository.Create<IFlexiSectionBlockFactory>();
            var dummyOptionsException = new OptionsException();
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser(mockFlexiSectionBlockFactory.Object);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.DiscardRedundantCharacters(dummyLevel, dummyBlockProcessor));
            mockFlexiSectionBlockFactory.Setup(f => f.Create(dummyLevel, dummyBlockProcessor, mockTestSubject.Object)).Throws(dummyOptionsException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    nameof(FlexiSectionBlock),
                    dummyLineIndex + 1,
                    dummyColumn,
                    Strings.BlockException_Shared_ExceptionOccurredWhileCreatingBlock),
                result.Message);
            Assert.Same(dummyOptionsException, result.InnerException);
        }

        [Theory]
        [MemberData(nameof(TryOpenBlock_CreatesFlexiSectionBlockAndReturnsBlockStateContinueDiscardIfSuccessful_Data))]
        public void TryOpenBlock_CreatesFlexiSectionBlockAndReturnsBlockStateContinueDiscardIfSuccessful(string dummyLineText, int expectedLevel)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock();
            Mock<IFlexiSectionBlockFactory> mockFlexiSectionBlockFactory = _mockRepository.Create<IFlexiSectionBlockFactory>();
            Mock<ExposedFlexiSectionBlockParser> mockTestSubject = CreateMockExposedFlexiSectionBlockParser(mockFlexiSectionBlockFactory.Object);
            mockFlexiSectionBlockFactory.Setup(f => f.Create(expectedLevel, dummyBlockProcessor, mockTestSubject.Object)).Returns(dummyFlexiSectionBlock);
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(t => t.DiscardRedundantCharacters(expectedLevel, dummyBlockProcessor));

            // Act
            BlockState result = mockTestSubject.Object.ExposedTryOpenBlock(dummyBlockProcessor);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(BlockState.ContinueDiscard, result);
            Assert.Single(dummyBlockProcessor.NewBlocks);
            Assert.Same(dummyFlexiSectionBlock, dummyBlockProcessor.NewBlocks.Peek());
        }

        public static IEnumerable<object[]> TryOpenBlock_CreatesFlexiSectionBlockAndReturnsBlockStateContinueDiscardIfSuccessful_Data()
        {
            return new object[][]
            {
                // Level 1
                new object[] { "# Dummy", 1 },
                // Level 2
                new object[] { "## Dummy", 2 },
                // Level 3
                new object[] { "### Dummy", 3 },
                // Level 4
                new object[] { "#### Dummy", 4 },
                // Level 5
                new object[] { "##### Dummy", 5 },
                // Level 6
                new object[] { "###### Dummy", 6 },
                // Eol after hashes
                new object[] { "####", 4 },
                // Tab after hashes
                new object[] { "#####\tDummy", 5 },
            };
        }

        [Fact]
        public void TryContinueBlock_ContinuesBlock()
        {
            // Arrange
            FlexiSectionBlock dummyFlexiSectionBlock = CreateFlexiSectionBlock();
            dummyFlexiSectionBlock.IsOpen = false; // Set to false so we can verify that it gets set to true
            ExposedFlexiSectionBlockParser testSubject = CreateExposedFlexiSectionBlockParser();

            // Act
            BlockState result = testSubject.ExposedTryContinueBlock(null, dummyFlexiSectionBlock);

            // Assert
            Assert.Equal(BlockState.Skip, result);
            Assert.True(dummyFlexiSectionBlock.IsOpen);
        }

        [Theory]
        [MemberData(nameof(DiscardRedundantCharacters_DiscardsRedundantCharacters_Data))]
        public void DiscardRedundantCharacters_DiscardsRedundantCharacters(string dummyLineText,
            int dummyLevel,
            int expectedStart,
            int expectedEnd,
            int expectedColumn)
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            dummyBlockProcessor.Line = new StringSlice(dummyLineText);
            FlexiSectionBlockParser testSubject = CreateFlexiSectionBlockParser();

            // Act
            testSubject.DiscardRedundantCharacters(dummyLevel, dummyBlockProcessor);

            // Assert
            Assert.Equal(expectedStart, dummyBlockProcessor.Line.Start);
            Assert.Equal(expectedEnd, dummyBlockProcessor.Line.End);
            Assert.Equal(expectedColumn, dummyBlockProcessor.Column);
        }

        public static IEnumerable<object[]> DiscardRedundantCharacters_DiscardsRedundantCharacters_Data()
        {
            return new object[][]
            {
                // Standard
                new object[]{"# Dummy", 1, 2, 6, 2},
                new object[]{"## Dummy", 2, 3, 7, 3},
                new object[]{"### Dummy", 3, 4, 8, 4},
                new object[]{"#### Dummy", 4, 5, 9, 5},
                new object[]{"##### Dummy", 5, 6, 10, 6},
                new object[]{"###### Dummy", 6, 7, 11, 7},
                // Eol after hash
                new object[]{"#", 1, 1, 0, 1}, // Moved past end of line
                // Multiple spaces between hashes and content
                new object[]{"##\t  Dummy", 2, 5, 9, 5},
                // Spaces after content
                new object[]{"##\t  Dummy  ", 2, 5, 9, 5},
                // End hashes
                new object[]{"### Dummy ###", 3, 4, 8, 4},
                // End hashes with trailing spaces
                new object[]{"#### Dummy ####  ", 4, 5, 9, 5},
                // End hashes without leading space
                new object[]{"##### Dummy#####", 5, 6, 15, 6},
                // End hashes with multiple leading spaces
                new object[]{"###### Dummy  ######", 6, 7, 11, 7}
            };
        }

        public class ExposedFlexiSectionBlockParser : FlexiSectionBlockParser
        {
            public ExposedFlexiSectionBlockParser(IFlexiSectionBlockFactory flexiSectionBlockFactory) :
                base(flexiSectionBlockFactory)
            {
            }

            public BlockState ExposedTryOpenBlock(BlockProcessor processor)
            {
                return TryOpenBlock(processor);
            }

            public BlockState ExposedTryContinueBlock(BlockProcessor processor, FlexiSectionBlock flexiSectionBlock)
            {
                return TryContinueBlock(processor, flexiSectionBlock);
            }
        }

        private FlexiSectionBlock CreateFlexiSectionBlock(string blockName = default,
            SectioningContentElement element = default,
            string linkIcon = default,
            FlexiSectionBlockRenderingMode renderingMode = default,
            int level = default,
            ReadOnlyDictionary<string, string> attributes = default,
            BlockParser blockParser = default)
        {
            return new FlexiSectionBlock(blockName, element, linkIcon, renderingMode, level, attributes, blockParser);
        }

        private FlexiSectionBlockParser CreateFlexiSectionBlockParser(IFlexiSectionBlockFactory flexiSectionBlockFactory = null)
        {
            return new FlexiSectionBlockParser(flexiSectionBlockFactory ?? _mockRepository.Create<IFlexiSectionBlockFactory>().Object);
        }

        private ExposedFlexiSectionBlockParser CreateExposedFlexiSectionBlockParser(IFlexiSectionBlockFactory flexiSectionBlockFactory = null)
        {
            return new ExposedFlexiSectionBlockParser(flexiSectionBlockFactory ?? _mockRepository.Create<IFlexiSectionBlockFactory>().Object);
        }

        private Mock<ExposedFlexiSectionBlockParser> CreateMockExposedFlexiSectionBlockParser(IFlexiSectionBlockFactory flexiSectionBlockFactory = null)
        {
            return _mockRepository.Create<ExposedFlexiSectionBlockParser>(flexiSectionBlockFactory ?? _mockRepository.Create<IFlexiSectionBlockFactory>().Object);
        }
    }
}
