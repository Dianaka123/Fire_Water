using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class LevelBuilder
    {
        private readonly ICanvasManger _canvasManager;
        private readonly GridManager _gridManager;
        private readonly BlocksManger _blocksManger;
        private readonly GameResources _gameResources;

        public LevelBuilder(BlocksManger blocksManger, GridManager gridManager,
            ICanvasManger canvasManager, GameResources gameResources)
        {
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _canvasManager = canvasManager;
            _gameResources = gameResources;
        }

        public void BuildLevel(LevelDesc level)
        {
            var background = _gameResources.Backgrounds[level.BackgroundId];
            _canvasManager.SetBackground(background.Sprite);
            
            var gridSize = new Vector2Int(level.ColumnCount, level.RowCount);
            var grid = _gridManager.CreateGrid(gridSize, background.BoardConfig);

            _blocksManger.CreateBlocks(level, grid);
        }
    }
}
