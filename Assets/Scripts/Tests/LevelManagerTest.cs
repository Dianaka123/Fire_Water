using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Managers;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using Moq;
using NUnit.Framework;
using UnityEngine;
using Zenject;
using Assets.Scripts.Data;

namespace Assets.Scripts.Tests
{
    public class LevelManagerTest : ZenjectUnitTestFixture
    {
        private int[] level = new int[]
        {
            1, 1, 0,
            1, 1, 0,
            0, 0, 1,
            -1, -1, 0,
            -1, -1, 1
        };

        [Inject]
        LevelManager manager;

        [SetUp]
        public void Init()
        {

            var converter = new Mock<ILevelJsonConverter>();

            converter.Setup(c => c.DeserializeAllLevels(It.IsAny<string>()))
                .Returns(new Level[] { new Level { LevelBlocksSequence = new Array2D<int>(level, 5, 3) } });

            Container.Bind<ILevelJsonConverter>().FromInstance(converter.Object);
            Container.Bind<LevelsConfiguration>().FromInstance(new LevelsConfiguration() { LevelsJSON = new TextAsset("")});

            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void LelvelManager_SwitchBlocks_ShouldSwitch()
        {
            manager.Initialize();
            var startIndex = new Vector2Int(2, 3);
            var endIndex = new Vector2Int(2, 2);

            var startBlock = manager.CurrentLevelSequence[startIndex];
            var endBlock = manager.CurrentLevelSequence[endIndex];

            manager.SwitchBlocks(startIndex, endIndex);
            Assert.AreEqual(endBlock, manager.CurrentLevelSequence[startIndex]);
            Assert.AreEqual(startBlock, manager.CurrentLevelSequence[endIndex]);
        }

        [Test]
        public void LelvelManager_SetEmptyCell_ShouldBeEmpty()
        {
            manager.Initialize();
            var startIndex = new Vector2Int(2, 3);

            manager.SetEmptyCell(startIndex);
            Assert.AreEqual(manager.EmptyCellId, manager.CurrentLevelSequence[startIndex]);
        }
    }
}
