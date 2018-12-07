using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Write_ThrowsArgumentNullExceptionIfRendererIsNull()
        {
            // Arrange
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<FlexiBlockRenderer<Block>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<Block>>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Write(null, dummyBlock.Object));
        }

        [Fact]
        public void Write_ThrowsArgumentNullExceptionIfObjIsNull()
        {
            // Arrange
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<FlexiBlockRenderer<Block>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<Block>>();
            mockTestSubject.CallBase = true;

            // Act and assert
            Assert.Throws<ArgumentNullException>(() => mockTestSubject.Object.Write(new HtmlRenderer(new StringWriter()), null));
        }

        [Fact]
        public void Write_DoesNotInterfereWithFlexiBlocksExceptionsWithBlockContext()
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(dummyBlock.Object);
            Mock<FlexiBlockRenderer<Block>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<Block>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("WriteFlexiBlock", dummyRenderer, dummyBlock.Object).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Theory]
        [MemberData(nameof(Write_WrapsNonFlexiBlocksExceptionsAndFlexiBlocksExceptionsWithoutBlockContextInFlexiBlocksExceptionsWithBlockContext_Data))]
        public void Write_WrapsNonFlexiBlocksExceptionsAndFlexiBlocksExceptionsWithoutBlockContextInFlexiBlocksExceptionsWithBlockContext(Exception dummyException)
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            Mock<Block> dummyBlock = _mockRepository.Create<Block>(null);
            Mock<FlexiBlockRenderer<Block>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<Block>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("WriteFlexiBlock", dummyRenderer, dummyBlock.Object).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiBlocksException_InvalidFlexiBlock,
                    dummyBlock.Object.GetType().Name,
                    dummyBlock.Object.Line + 1,
                    dummyBlock.Object.Column,
                    Strings.FlexiBlocksException_FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public static IEnumerable<object[]> Write_WrapsNonFlexiBlocksExceptionsAndFlexiBlocksExceptionsWithoutBlockContextInFlexiBlocksExceptionsWithBlockContext_Data()
        {
            return new object[][]
            {
                // Non FlexiBlocksException exception
                new object[]{new ArgumentException()},
                // FlexiBlocksException without block context
                new object[]{new FlexiBlocksException()}
            };
        }
    }
}
