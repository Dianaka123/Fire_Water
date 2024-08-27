using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Tests
{
    public class GridManipulatorFacadeTest: ZenjectUnitTestFixture
    {
        private Array2D<int> _levelSequence = new Array2D<int>(new int[]
                {
                    0, 0, 0, 0,
                    1, 1, -1, -1,
                }, 2, 4);
        // x - 4, y - 2

        private bool isLevelArraySwitchCalled;
        private bool isBlocksArraySwitchCalled;

        private static (Vector2Int, Vector2Int)[] correctIndexes = new[]
        {
            (new Vector2Int(1, 0), Vector2Int.left),
            (new Vector2Int(1, 0), Vector2Int.right),
            (new Vector2Int(1, 0), Vector2Int.up),
            (new Vector2Int(1, 1), Vector2Int.down),
        };

        private static (Vector2Int, Vector2Int)[] incorrectIndexes = new[]
        {
            (new Vector2Int(0, 0), Vector2Int.left),
            (new Vector2Int(0, 0), Vector2Int.down),
            (new Vector2Int(3, 0), Vector2Int.right),
            (new Vector2Int(0, 1), Vector2Int.up),
            (new Vector2Int(2, 1), Vector2Int.down),
        };

        [Inject]
        private GridManipulatorFacade facade;

        [SetUp]
        public void Init()
        {
            isLevelArraySwitchCalled = false;
            isBlocksArraySwitchCalled = false;

            var levelManagerMock = new Mock<ILevelManager>();
            levelManagerMock.Setup(m => m.CurrentLevelSequence)
                .Returns(_levelSequence);
            levelManagerMock.Setup(m => m.IsEmptyCell(It.IsAny<Vector2Int>()))
                .Returns<Vector2Int>((index) 
                => _levelSequence[index] == -1);

            levelManagerMock.Setup(m => m.SwitchBlocks(It.IsAny<Vector2Int>(), It.IsAny<Vector2Int>()))
                .Callback(() => isLevelArraySwitchCalled = true);

            var blockManagerMock = new Mock<IBlockManager>();
            blockManagerMock.Setup(b => b.SwitchBlocksAsync(It.IsAny<Vector2Int>(), It.IsAny<Vector2Int>(), It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Callback(() => isBlocksArraySwitchCalled = true);

            Container.Bind<ILevelManager>().FromInstance(levelManagerMock.Object);
            Container.Bind<IBlockManager>().FromInstance(blockManagerMock.Object);
            Container.Bind<IBoardNormalizer>().FromInstance(new Mock<IBoardNormalizer>().Object);
            Container.Bind<GridManipulatorFacade>().AsSingle();

        }

        [TestCaseSource("correctIndexes")]
        public void MoveBlockAsync_CorrectIndexes_SwitchCalled((Vector2Int from, Vector2Int direction) value)
        {
            MockGridManager(value.from);
            facade.SwitchCellsThenNormilize(Vector3.zero, value.direction).Forget();
            Assert.IsTrue(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        [TestCaseSource("incorrectIndexes")]
        public void MoveBlockAsync_IncorrectIndexes_SwitchCalled((Vector2Int from, Vector2Int direction) value)
        {
            MockGridManager(value.from);
            facade.SwitchCellsThenNormilize(Vector3.zero, value.direction).Forget();
            Assert.IsFalse(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        [Test]
        public void MoveBlockAsync_CorrectIndex_SwitchUpWithEmpty()
        {
            MockGridManager(new Vector2Int(0, 2));
            facade.SwitchCellsThenNormilize(Vector3.zero, Vector2Int.up).Forget();
            Assert.IsFalse(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        private void MockGridManager(Vector2Int cellIndex)
        {
            var gridManager = new Mock<IGridManager>();
            gridManager.Setup(g => g.GetCellIndexByScreenPosition(It.IsAny<Vector3>())).Returns(cellIndex);
            Container.Rebind<IGridManager>().FromInstance(gridManager.Object);
         
            Container.Inject(this);
        }
    }
}
