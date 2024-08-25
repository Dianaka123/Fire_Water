using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class LevelBuilder
    {
        private readonly ICanvasManger _canvasManager;
        private readonly IGridManager _gridManager;
        private readonly IBlockManager _blocksManger;
        private readonly GameResources _gameResources;

        public LevelBuilder(IBlockManager blocksManger, IGridManager gridManager,
            ICanvasManger canvasManager, GameResources gameResources)
        {
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _canvasManager = canvasManager;
            _gameResources = gameResources;
        }

        public void BuildLevel(int[,] level)
        {
            var background = _gameResources.Backgrounds[0];
            _canvasManager.SetBackground(background.Sprite);
            
            var gridSize = new Vector2Int(level.GetLength(0), level.GetLength(1));
            var grid = _gridManager.CreateGrid(gridSize, background.BoardConfig);

            _blocksManger.CreateBlocks(level, grid, _canvasManager.DynamicCanvasTransform);
        }
    }
}
