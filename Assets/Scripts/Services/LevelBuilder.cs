using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;

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
            _canvasManager.SetBackground(_gameResources.Backgrounds[level.BackgroundId].Sprite);
            var grid = _gridManager.CreateGrid(level);
            _blocksManger.CreateBlocks(level, grid);
        }
    }
}
