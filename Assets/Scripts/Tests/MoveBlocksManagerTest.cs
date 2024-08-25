using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
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
        private int[,] _levelSequence = new int[2,4]
                {
                    {1, 1, -1, -1 },
                    {0, 0, 0, 0 }
                };

        private bool isLevelArraySwitchCalled;
        private bool isBlocksArraySwitchCalled;

        private static (Vector2Int, Vector2Int)[] correctIndexes = new[]
        {
            (new Vector2Int(0, 1),new Vector2Int(1, 1)),
            (new Vector2Int(0, 1), new Vector2Int(0, 0)),
            (new Vector2Int(0, 1), new Vector2Int(0, 2)),
            (new Vector2Int(1, 1), new Vector2Int(0, 0)),
        };

        private static (Vector2Int, Vector2Int)[] incorrectIndexes = new[]
        {
            (new Vector2Int(0, 0),new Vector2Int(0, -1)),
            (new Vector2Int(0, 0), new Vector2Int(-1, 0)),
            (new Vector2Int(0, 3), new Vector2Int(0, 4)),
            (new Vector2Int(1, 0), new Vector2Int(2, 0)),
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
                .Returns(new int[,]
                {
                    {1, 1, -1, -1 },
                    {0, 0, 0, 0 }
                });
            levelManagerMock.Setup(m => m.IsEmptyCell(It.IsAny<Vector2Int>()))
                .Returns<Vector2Int>((index) 
                => _levelSequence[index.x, index.y] == -1);

            levelManagerMock.Setup(m => m.SwitchBlocks(It.IsAny<Vector2Int>(), It.IsAny<Vector2Int>()))
                .Callback(() => isLevelArraySwitchCalled = true);

            var blockManagerMock = new Mock<IBlockManager>();
            blockManagerMock.Setup(b => b.SwitchBlocksAsync(It.IsAny<Vector2Int>(), It.IsAny<Vector2Int>(), It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Callback(() => isBlocksArraySwitchCalled = true);

            Container.Bind<ILevelManager>().FromInstance(levelManagerMock.Object);
            Container.Bind<IBlockManager>().FromInstance(blockManagerMock.Object);
            Container.Bind<IGridManager>().FromInstance(new Mock<IGridManager>().Object);
            Container.Bind<MoveBlocksManager>().AsSingle();

            Container.Inject(this);
        }

        [TestCaseSource("correctIndexes")]
        public async void MoveBlockAsync_CorrectIndexes_SwitchCalled((Vector2Int, Vector2Int) value)
        {
            await manager.MoveBlockAsync(value.Item1, value.Item2);
            Assert.IsTrue(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        [TestCaseSource("incorrectIndexes")]
        public async void MoveBlockAsync_IncorrectIndexes_SwitchCalled((Vector2Int, Vector2Int) value)
        {
            await manager.MoveBlockAsync(value.Item1, value.Item2);
            Assert.IsFalse(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }

        [Test]
        public async void MoveBlockAsync_CorrectIndex_SwitchUpWithEmpty()
        {
            await manager.MoveBlockAsync(new Vector2Int(0,2), new Vector2Int(1, 2));
            Assert.IsFalse(isLevelArraySwitchCalled && isBlocksArraySwitchCalled);
        }
    }
}
