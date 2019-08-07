using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class BlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Write_ThrowsArgumentNullExceptionIfRendererIsNull()
        {
            // Arrange
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<BlockRenderer<Block>> mockTestSubject = _mockRepository.Create<BlockRenderer<Block>>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Write(null, dummyBlock.Object));
        }

        [Fact]
        public void Write_ThrowsArgumentNullExceptionIfObjIsNull()
        {
            // Arrange
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<BlockRenderer<Block>> mockTestSubject = _mockRepository.Create<BlockRenderer<Block>>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Write(new HtmlRenderer(new StringWriter()), null));
        }

        [Fact]
        public void Write_DoesNotInterfereWithBlockExceptionsWithBlockContext()
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            var dummyBlockException = new BlockException(dummyBlock.Object);
            Mock<BlockRenderer<Block>> mockTestSubject = _mockRepository.Create<BlockRenderer<Block>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("WriteBlock", dummyRenderer, dummyBlock.Object).Throws(dummyBlockException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyBlockException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(Write_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data))]
        public void Write_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext(Exception dummyException)
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<BlockRenderer<Block>> mockTestSubject = _mockRepository.Create<BlockRenderer<Block>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("WriteBlock", dummyRenderer, dummyBlock.Object).Throws(dummyException);

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    dummyBlock.Object.GetType().Name,
                    dummyBlock.Object.Line + 1,
                    dummyBlock.Object.Column,
                    Strings.BlockException_BlockException_ExceptionOccurredWhileProcessingBlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> Write_WrapsNonBlockExceptionsAndBlockExceptionsWithoutBlockContextInBlockExceptionsWithBlockContext_Data()
        {
            return new object[][]
            {
                // Non BlockException exception
                new object[]{new ArgumentException()},
                // BlockException without block context
                new object[]{new BlockException()}
            };
        }
    }
}
