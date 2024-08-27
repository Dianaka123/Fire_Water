using Assets.Scripts.Data;
using Assets.Scripts.Services;
using NUnit.Framework;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Tests
{
    public class GridBuilderTest : ZenjectUnitTestFixture
    {
        [Inject]
        GridBuilder gridBuilder;

        private Vector2 windowSize = new Vector2(100, 100);

        [SetUp]
        public void Init()
        {
            Container.BindInterfacesAndSelfTo<GridBuilder>().AsSingle();

            Container.Inject(this);
        }

        [TestCase(6, 4, 0.2f, 0.2f, -15, -25)]
        [TestCase(2, 4, 0.2f, 0.4f, -15, -5)]
        public void GridBuilder_CreateBoardForLevel_BottomIndexShouldBeCentralized(int rows, int columns, float sideOffset, float bottomOffset, float x, float y)
        {
            var gridSize = new Vector2Int(columns, rows);
            var boardConfig = new BoardConfig()
            {
                RelativeSideOffset = sideOffset,
                RelativeBottomOffset = bottomOffset,
            };

            var gridConfig = gridBuilder.CreateGridForLevel(gridSize, boardConfig, windowSize);

            Assert.NotNull(gridConfig.Indexes);
            Assert.That(gridConfig.Indexes[0, 0], Is.EqualTo(new Vector2(x, y)));
        }

        [TestCase(6, 4, -0.2f, -0.2f)]
        [TestCase(-6, 4, 0.2f, 0.2f)]
        [TestCase(-6, 4, -0.2f, 0.2f)]
        public void GridBuilder_CreateBoardForLevel_ShouldProduceError(int rows, int columns, float sideOffset, float bottomOffset)
        {
            var gridSize = new Vector2Int(columns, rows);
            var boardConfig = new BoardConfig()
            {
                RelativeSideOffset = sideOffset,
                RelativeBottomOffset = bottomOffset,
            };

            Assert.Throws<ArgumentException>(() => gridBuilder.CreateGridForLevel(gridSize, boardConfig, windowSize));
        }
    }
}
