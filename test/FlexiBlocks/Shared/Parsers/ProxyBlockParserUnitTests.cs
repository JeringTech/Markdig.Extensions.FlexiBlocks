using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Moq.Protected;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class ProxyBlockParserUnitTests
    {
        private static readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CloseProxy_ReturnsNull()
        {
            // Arrange
            ExposedProxyBlockParser<Block, DummyProxyBlock> testSubject = CreateExposedBlockParser();

            // Act
            Block result = testSubject.ExposedCloseProxy(null, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CloseBlock_ReturnsFalseIfThereIsNoReplacement()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            var dummyProxyBlock = new DummyProxyBlock(null);
            Mock<ExposedProxyBlockParser<Block, DummyProxyBlock>> testSubject = CreateMockExposedBlockParser();
            testSubject.CallBase = true;
            testSubject.Protected().Setup<Block>("CloseProxy", dummyBlockProcessor, dummyProxyBlock).Returns((Block)null);

            // Act
            bool result = testSubject.Object.ExposedCloseBlock(dummyBlockProcessor, dummyProxyBlock);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CloseBlock_ReturnsTrueAndReplacesProxyIfThereIsAReplacement()
        {
            // Arrange
            BlockProcessor dummyBlockProcessor = MarkdigTypesFactory.CreateBlockProcessor();
            Mock<LeafBlock> dummySiblingBlock1 = _mockRepository.Create<LeafBlock>(null);
            Mock<LeafBlock> dummySiblingBlock2 = _mockRepository.Create<LeafBlock>(null);
            var dummyProxyBlock = new DummyProxyBlock(null);
            Mock<ContainerBlock> dummyParentBlock = _mockRepository.Create<ContainerBlock>(null);
            dummyParentBlock.CallBase = true;
            dummyParentBlock.Object.Add(dummySiblingBlock1.Object); // So we can verify replacement position
            dummyParentBlock.Object.Add(dummyProxyBlock);
            dummyParentBlock.Object.Add(dummySiblingBlock2.Object); // So we can verify replacement position
            Mock<Block> dummyReplacementBlock = _mockRepository.Create<Block>(null);
            Mock<ExposedProxyBlockParser<Block, DummyProxyBlock>> testSubject = CreateMockExposedBlockParser();
            testSubject.CallBase = true;
            testSubject.Protected().Setup<Block>("CloseProxy", dummyBlockProcessor, dummyProxyBlock).Returns(dummyReplacementBlock.Object);

            // Act
            bool result = testSubject.Object.ExposedCloseBlock(dummyBlockProcessor, dummyProxyBlock);

            // Assert
            Assert.True(result);
            Assert.Equal(3, dummyParentBlock.Object.Count);
            Assert.Same(dummySiblingBlock1.Object, dummyParentBlock.Object[0]);
            Assert.Same(dummyReplacementBlock.Object, dummyParentBlock.Object[1]); // Replacement must be in position of proxy it replaced
            Assert.Same(dummySiblingBlock2.Object, dummyParentBlock.Object[2]);
        }

        private Mock<ExposedProxyBlockParser<Block, DummyProxyBlock>> CreateMockExposedBlockParser()
        {
            return _mockRepository.Create<ExposedProxyBlockParser<Block, DummyProxyBlock>>();
        }

        private ExposedProxyBlockParser<Block, DummyProxyBlock> CreateExposedBlockParser()
        {
            return new ExposedProxyBlockParser<Block, DummyProxyBlock>();
        }

        public class ExposedProxyBlockParser<TMain, TProxy> : ProxyBlockParser<TMain, TProxy>
            where TMain : Block
            where TProxy : Block, IProxyBlock
        {
            public TMain ExposedCloseProxy(BlockProcessor blockProcessor, TProxy proxyBlock)
            {
                return CloseProxy(blockProcessor, proxyBlock);
            }

            public bool ExposedCloseBlock(BlockProcessor blockProcessor, TProxy proxyBlock)
            {
                return CloseBlock(blockProcessor, proxyBlock);
            }

            protected override BlockState TryOpenBlock(BlockProcessor blockProcessor)
            {
                return BlockState.None;   // Do nothing
            }
        }

        public class DummyProxyBlock : Block, IProxyBlock
        {
            public DummyProxyBlock(BlockParser parser) : base(parser)
            {
            }

            public string MainTypeName { get; }
        }
    }
}
