using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Wrappers;

namespace Assets.Scripts.Services
{
    public class LevelBuilder
    {
        private readonly IUIManger _uiManager;
        private readonly IGridManager _gridManager;
        private readonly IBlockManager _blocksManger;
        private readonly GameResources _gameResources;

        public LevelBuilder(IBlockManager blocksManger, IGridManager gridManager,
            IUIManger canvasManager, GameResources gameResources)
        {
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _uiManager = canvasManager;
            _gameResources = gameResources;
        }

        public void BuildLevel(Array2D<int> level)
        {
            var background = _gameResources.Backgrounds[0];
            _uiManager.SetBackground(background.Sprite);
            
            var grid = _gridManager.CreateGrid(level.Size, background.BoardConfig);

            _blocksManger.CreateBlocks(level, grid, _uiManager.BlocksRoot);
        }

        public void RestartLevel(Array2D<int> level)
        {
            _blocksManger.RestartLevel(level, _gridManager.CurrentGrid, _uiManager.BlocksRoot);
        }
    }
}
