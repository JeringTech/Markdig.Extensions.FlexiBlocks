using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Moq;
using System;
using System.IO;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests.Shared
{
    public class FlexiBlockRendererUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Write_DoesNotInterfereWithFlexiBlocksExceptionsThrownByWriteFlexiBlock()
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            var dummyBlock = new DummyBlock(null);
            var dummyFlexiBlocksException = new FlexiBlocksException();
            Mock<FlexiBlockRenderer<DummyBlock>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<DummyBlock>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.WriteFlexiBlock(dummyRenderer, dummyBlock)).Throws(dummyFlexiBlocksException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Same(dummyFlexiBlocksException, result);
            Assert.Null(result.InnerException);
        }

        [Fact]
        public void Write_WrapsNonFlexiBlocksExceptionsThrownByWriteFlexiBlockInFlexiBlocksExceptions()
        {
            // Arrange
            var dummyRenderer = new HtmlRenderer(new StringWriter());
            var dummyBlock = new DummyBlock(null);
            var dummyException = new ArgumentException(); // Arbitrary type
            Mock<FlexiBlockRenderer<DummyBlock>> mockTestSubject = _mockRepository.Create<FlexiBlockRenderer<DummyBlock>>();
            mockTestSubject.CallBase = true;
            mockTestSubject.Setup(f => f.WriteFlexiBlock(dummyRenderer, dummyBlock)).Throws(dummyException);

            // Act and assert
            FlexiBlocksException result = Assert.Throws<FlexiBlocksException>(() => mockTestSubject.Object.Write(dummyRenderer, dummyBlock));
            _mockRepository.VerifyAll();
            Assert.Equal(@"The FlexiDummyBlock starting at line ""1"", column ""0"", is invalid:
An unexpected exception occurred. Refer to the inner exception for more details.", 
                result.Message, ignoreLineEndingDifferences: true);
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
