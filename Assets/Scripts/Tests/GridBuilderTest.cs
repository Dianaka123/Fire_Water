using Assets.Scripts.Configs;
using Assets.Scripts.Services;
using NUnit.Framework;
using System;
using UnityEngine;
using Zenject;

public class GridBuilderTest: ZenjectUnitTestFixture
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

    [TestCase(6, 4, 20, 20, -20, -30)]
    [TestCase(2, 4, 20, 40, -20, -10)]
    public void GridBuilder_CreateBoardForLevel_BottomIndexShouldBeCentralized(int rows, int columns, float sideOffset, float bottomOffset, float x, float y )
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

    [TestCase(6, 4, -20, -20)]
    [TestCase(-6, 4, 20, 20)]
    [TestCase(-6, 4, -20, 20)]
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
