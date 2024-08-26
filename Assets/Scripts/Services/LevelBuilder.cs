using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Wrappers;
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

        public void BuildLevel(Array2D<int> level)
        {
            var background = _gameResources.Backgrounds[0];
            _canvasManager.SetBackground(background.Sprite);
            
            var grid = _gridManager.CreateGrid(level.Size, background.BoardConfig);

            _blocksManger.CreateBlocks(level, grid, _canvasManager.DynamicCanvasTransform);
        }
    }
}
