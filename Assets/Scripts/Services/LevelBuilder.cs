using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Wrappers;

namespace Assets.Scripts.Services
{
    public class LevelBuilder
    {
        private readonly IUIManger _uiManager;
        private readonly IGridManager _gridManager;
        private readonly IBlockManager _blocksManger;
        private readonly IBackgroundManager _backgroundManager;

        public LevelBuilder(IBlockManager blocksManger, IGridManager gridManager,
            IUIManger canvasManager, IBackgroundManager backgroundManager)
        {
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _uiManager = canvasManager;
            _backgroundManager = backgroundManager;
        }

        public void BuildLevel(Array2D<int> level, int levelIndex)
        {
            var background = _backgroundManager.GetBackgroundByLevelIndex(levelIndex);
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
