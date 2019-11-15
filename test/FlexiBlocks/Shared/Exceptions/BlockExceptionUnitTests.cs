using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class BlockExceptionUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void BlockException_CanBeSerialized()
        {
            // Arrange
            const int dummyLineIndex = 1;
            const int dummyColumn = 2;
            var dummyBlock = new DummyBlock(null)
            {
                Column = dummyColumn,
                Line = dummyLineIndex
            };
            const string dummyDescription = "dummyDescription";
            IFormatter dummyFormatter = new BinaryFormatter();
            var dummyStream = new MemoryStream();
            var testSubject = new BlockException(dummyBlock, dummyDescription);

            // Act
            dummyFormatter.Serialize(dummyStream, testSubject);
            dummyStream.Position = 0;
            var result = (BlockException)dummyFormatter.Deserialize(dummyStream);

            // Assert
            Assert.Equal(BlockExceptionContext.Block, result.Context);
            Assert.Equal(dummyLineIndex + 1, result.LineNumber);
            Assert.Equal(dummyColumn, result.Column);
            Assert.Equal(dummyDescription, result.Description);
            Assert.Equal(nameof(DummyBlock), result.BlockTypeName);
        }

        [Theory]
        [MemberData(nameof(Message_ReturnsInvalidBlockMessageIfContextIsBlock_Data))]
        public void Message_ReturnsInvalidBlockMessageIfContextIsBlock(string dummyDescription, string dummyExpectedDescription)
        {
            // Arrange
            const int dummyColumn = 2; // Arbitrary
            const int dummyLineIndex = 5; // Arbitrary
            var dummyBlock = new DummyBlock(null) { Column = dummyColumn, Line = dummyLineIndex };
            var testSubject = new BlockException(dummyBlock, dummyDescription);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                nameof(DummyBlock),
                dummyLineIndex + 1,
                dummyColumn,
                dummyExpectedDescription),
                result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Message_ReturnsInvalidBlockMessageIfContextIsBlock_Data()
        {
            return new object[][]
            {
                new object[]{"dummyDescription", "dummyDescription"},
                new object[]{null, Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock}
            };
        }

        [Theory]
        [MemberData(nameof(Message_IfContextIsBlockAndBlockIsProxyBlockWithMainTypeNamePrintsMainTypeName_Data))]
        public void Message_IfContextIsBlockAndBlockIsProxyBlockWithMainTypeNamePrintsMainTypeName(string dummyMainTypeName, string expectedTypeName)
        {
            // Arrange
            const string dummyDescription = "dummyDescription";
            const int dummyColumn = 2; // Arbitrary
            const int dummyLineNumber = 5; // Arbitrary
            Mock<IProxyBlock> mockProxyBlock = _mockRepository.Create<IProxyBlock>();
            mockProxyBlock.Setup(p => p.MainTypeName).Returns(dummyMainTypeName);
            var testSubject = new BlockException(mockProxyBlock.Object, dummyDescription, null, dummyLineNumber, dummyColumn);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                expectedTypeName,
                dummyLineNumber,
                dummyColumn,
                dummyDescription),
                result, ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Message_IfContextIsBlockAndBlockIsProxyBlockWithMainTypeNamePrintsMainTypeName_Data()
        {
            return new object[][]
            {
                new object[]{ "dummyMainTypeName", "dummyMainTypeName"},
                // If specified name is null, whitespace or an empty string, "block of unknown type" is printed in place of a type name
                new object[]{ null, "block of unknown type"},
                new object[]{ " ", "block of unknown type"},
                new object[]{ string.Empty, "block of unknown type"}
            };
        }

        [Theory]
        [MemberData(nameof(Message_ReturnsInvalidBlockMessageIfContextIsLine_Data))]
        public void Message_ReturnsInvalidBlockMessageIfContextIsLine(string dummyDescription, string dummyExpectedDescription)
        {
            // Arrange
            const int dummyColumn = 2; // Arbitrary
            const int dummyLineNumber = 5; // Arbitrary
            var testSubject = new BlockException(dummyLineNumber, dummyColumn, dummyDescription);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                "block of unknown type",
                dummyLineNumber,
                dummyColumn,
                dummyExpectedDescription),
                result,
                ignoreLineEndingDifferences: true);
        }

        public static IEnumerable<object[]> Message_ReturnsInvalidBlockMessageIfContextIsLine_Data()
        {
            return new object[][]
            {
                new object[]{"dummyDescription", "dummyDescription"},
                new object[]{null, Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock}
            };
        }

        [Theory]
        [MemberData(nameof(Message_ReturnsBaseMessageIfContextIsNone_Data))]
        public void Message_ReturnsBaseMessageIfContextIsNone(string dummyMessage, string expectedResult)
        {
            // Arrange
            var testSubject = new BlockException(dummyMessage);

            // Act
            string result = testSubject.Message;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Message_ReturnsBaseMessageIfContextIsNone_Data()
        {
            return new object[][]
            {
                new object[]{"dummyMessage", "dummyMessage"},
                new object[]{null, $"Exception of type '{typeof(BlockException).FullName}' was thrown."},
            };
        }

        public class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
