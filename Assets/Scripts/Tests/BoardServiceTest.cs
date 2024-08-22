using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Moq;
using NUnit.Framework;
using System;
using UnityEngine;
using Zenject;

public class BoardServiceTest: ZenjectUnitTestFixture
{
    [SetUp]
    public void Init()
    {
        var mockCanvasManager = new Mock<ICanvasManger>();
        mockCanvasManager.Setup(m => m.Size).Returns(new Vector2(100, 100));
        mockCanvasManager.Setup(m => m.LocalScale).Returns(Vector3.one);

        Container.Bind<ICanvasManger>().FromInstance(mockCanvasManager.Object);
        Container.BindInterfacesAndSelfTo<BoardService>().AsSingle();
        

        Container.Inject(this);
    }

    [Inject]
    BoardService boardService;

    [TestCase(6, 4, 20, 20, -20, -30)]
    [TestCase(2, 4, 20, 40, -20, -10)]
    public void BoardService_CreateBoardForLevel_BottomIndexShouldBeCentralized(int rows, int columns, float sideOffset, float bottomOffset, float x, float y )
    {
        var gridSize = new Vector2Int(columns, rows);
        var boardConfig = new BoardConfig()
        {
            SideOffset = sideOffset,
            BottomOffset = bottomOffset,
        };

        boardService.CreateBoardForLevel(gridSize, boardConfig);

        Assert.NotNull(boardService.Indexes);
        Assert.That(boardService.Indexes[0, 0], Is.EqualTo(new Vector2(x, y)));
    }

    [TestCase(6, 4, -20, -20)]
    [TestCase(-6, 4, 20, 20)]
    [TestCase(-6, 4, -20, 20)]
    public void BoardService_CreateBoardForLevel_ShouldProduceError(int rows, int columns, float sideOffset, float bottomOffset)
    {
        var gridSize = new Vector2Int(columns, rows);
        var boardConfig = new BoardConfig()
        {
            SideOffset = sideOffset,
            BottomOffset = bottomOffset,
        };

        Assert.Throws<ArgumentException>(() => boardService.CreateBoardForLevel(gridSize, boardConfig));
        Assert.Null(boardService.Indexes);
    }
}
