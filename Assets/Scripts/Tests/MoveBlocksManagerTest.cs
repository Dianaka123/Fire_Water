using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Tests
{
    public class MoveBlocksManagerTest: ZenjectUnitTestFixture
    {
        private Array2D<int> _levelSequence = new Array2D<int>(new int[]
                {
                    1, 1, -1, -1,
                    0, 0, 0, 0
                }, 2, 4);

        private bool isLevelArraySwitchCalled;
        private bool isBlocksArraySwitchCalled;

        private static (Vector2Int, Vector2Int)[] correctIndexes = new[]
        {
            (new Vector2Int(0, 1),new Vector2Int(1, 0)),
            (new Vector2Int(0, 1), new Vector2Int(0, -1)),
            (new Vector2Int(0, 1), new Vector2Int(0, 1)),
            (new Vector2Int(1, 1), new Vector2Int(-1, -1)),
        };

        private static (Vector2Int, Vector2Int)[] incorrectIndexes = new[]
        {
            (new Vector2Int(0, 0),new Vector2Int(0, -1)),
            (new Vector2Int(0, 0), new Vector2Int(-1, 0)),
            (new Vector2Int(0, 3), new Vector2Int(0, 1)),
            (new Vector2Int(1, 0), new Vector2Int(1, 0)),
        };

        [Inject]
        private MoveBlocksManager manager;

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
            Container.Bind<MoveBlocksManager>().AsSingle();

            Container.Inject(this);
        }

        [TestCaseSource("correctIndexes")]
        public async void MoveBlockAsync_CorrectIndexes_SwitchCalled((Vector2Int, Vector2Int) value)
        {
            MockGridManager(value.Item1);
            await manager.MoveBlockAsync(Vector3.zero, value.Item2);
            Assert.IsTrue(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        [TestCaseSource("incorrectIndexes")]
        public async void MoveBlockAsync_IncorrectIndexes_SwitchCalled((Vector2Int, Vector2Int) value)
        {
            MockGridManager(value.Item1);
            await manager.MoveBlockAsync(Vector3.zero, value.Item2);
            Assert.IsFalse(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        [Test]
        public async void MoveBlockAsync_CorrectIndex_SwitchUpWithEmpty()
        {
            MockGridManager(new Vector2Int(0, 2));
            await manager.MoveBlockAsync(Vector3.zero, new Vector2Int(1, 2));
            Assert.IsFalse(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }


        private void MockGridManager(Vector2Int cellIndex)
        {
            var gridManager = new Mock<IGridManager>();
            gridManager.Setup(g => g.GetCellIndexByScreenPosition(It.IsAny<Vector3>())).Returns(cellIndex);
            Container.Bind<IGridManager>().FromInstance(gridManager.Object);
        }
    }
}
