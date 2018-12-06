﻿using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using System;
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
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException(new DummyBlock(null));
            Mock<FlexiBlockRenderer<DummyBlock>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<DummyBlock>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("WriteFlexiBlock", dummyRenderer, dummyBlock).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void Write_WrapsExceptionsInFlexiBlocksExceptions()
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockRenderer<DummyBlock>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<DummyBlock>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Protected().Setup("WriteFlexiBlock", dummyRenderer, dummyBlock).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.FlexiBlocksException_FlexiBlocksException_InvalidFlexiBlock,
                    nameof(DummyBlock),
                    dummyBlock.Line + 1,
                    dummyBlock.Column,
                    Strings.FlexiBlocksException_FlexiBlocksException_ExceptionOccurredWhileProcessingABlock),
                result.Message,
                ignoreLineEndingDifferences: true);
            Assert.Same(dummyException, result.InnerException);
        }

        public class DummyBlock : Block
        {
            public DummyBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}