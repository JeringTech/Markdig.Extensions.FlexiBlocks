using Markdig.Parsers;
using Markdig.Syntax;
using Moq;
using Xunit;

namespace Jering.Markdig.Extensions.FlexiBlocks.Tests
{
    public class CollectionBlockFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void MoveChildren_MovesChildren()
        {
            // Arrange
            var dummyChildBlock1 = new DummyChildBlock(null);
            var dummyChildBlock2 = new DummyChildBlock(null);
            var dummyChildBlock3 = new DummyChildBlock(null);
            var dummyProxyFencedContainerBlock = new ProxyFencedContainerBlock(0, 0, null, null)
            {
                dummyChildBlock1, dummyChildBlock2, dummyChildBlock3
            };
            ContainerBlock dummyTarget = _mockRepository.Create<ContainerBlock>(null).Object;
            ExposedCollectionBlockFactory testSubject = CreateExposedCollectionBlockFactory();

            // Act
            testSubject.ExposedMoveChildren(dummyProxyFencedContainerBlock, dummyTarget);

            // Assert
            Assert.Equal(3, dummyTarget.Count);
            // Correct order
            Assert.Same(dummyChildBlock1, dummyTarget[0]);
            Assert.Same(dummyChildBlock2, dummyTarget[1]);
            Assert.Same(dummyChildBlock3, dummyTarget[2]);
            Assert.Empty(dummyProxyFencedContainerBlock);
        }

        [Fact]
        public void MoveChildren_ThrowsBlockExceptionIfAChildIsNotOfTheExpectedType()
        {
            // Arrange
            const int dummyLineIndex = 6;
            const int dummyColumn = 2;
            var dummyChildBlock1 = new DummyChildBlock(null);
            Block dummyChildBlock2 = _mockRepository.Create<Block>(null).Object;
            var dummyChildBlock3 = new DummyChildBlock(null);
            var dummyProxyFencedContainerBlock = new ProxyFencedContainerBlock(0, 0, null, null)
            {
                dummyChildBlock1, dummyChildBlock2, dummyChildBlock3
            };
            ContainerBlock dummyTarget = _mockRepository.Create<ContainerBlock>(null).Object;
            dummyTarget.Column = dummyColumn;
            dummyTarget.Line = dummyLineIndex;
            ExposedCollectionBlockFactory testSubject = CreateExposedCollectionBlockFactory();

            // Act and assert
            BlockException result = Assert.Throws<BlockException>(() => testSubject.ExposedMoveChildren(dummyProxyFencedContainerBlock, dummyTarget));
            Assert.Equal(string.Format(Strings.BlockException_BlockException_InvalidBlock,
                    dummyTarget.GetType().Name,
                    dummyLineIndex + 1,
                    dummyColumn,
                    string.Format(Strings.BlockException_Shared_BlockMustOnlyContainASpecificTypeOfBlock,
                        nameof(ContainerBlock),
                        nameof(DummyChildBlock),
                        dummyChildBlock2.GetType().Name)),
                result.Message);
        }

        private ExposedCollectionBlockFactory CreateExposedCollectionBlockFactory()
        {
            return new ExposedCollectionBlockFactory();
        }

        private class ExposedCollectionBlockFactory : CollectionBlockFactory<ContainerBlock, ProxyFencedContainerBlock, DummyChildBlock>
        {
            public override ContainerBlock Create(ProxyFencedContainerBlock proxyFencedBlock, BlockProcessor blockProcessor)
            {
                // Do nothing
                return null;
            }

            public override ProxyFencedContainerBlock CreateProxyFencedBlock(int openingFenceIndent, int openingFenceCharCount, BlockProcessor blockProcessor, BlockParser blockParser)
            {
                // Do nothing
                return null;
            }

            public void ExposedMoveChildren(ProxyFencedContainerBlock proxyFencedContainerBlock, ContainerBlock target)
            {
                MoveChildren(proxyFencedContainerBlock, target);
            }
        }

        private class DummyChildBlock : Block
        {
            public DummyChildBlock(BlockParser parser) : base(parser)
            {
            }
        }
    }
}
