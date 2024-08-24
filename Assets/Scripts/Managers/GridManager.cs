using Assets.Scripts.Configs;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GridManager
    {
        private readonly GameResources _gameResources;
        private readonly IGridBuilder _gridBuilder;

        public GridData CurrentGrid { get; private set; }

        public GridManager(IGridBuilder gridBuilder, GameResources gameResources)
        {
            _gridBuilder = gridBuilder;
            _gameResources = gameResources;
        }

        public GridData CreateGrid(LevelDesc level)
        {
            var boardConfig = _gameResources.Backgrounds[level.BackgroundId].BoardConfig;
            CurrentGrid = _gridBuilder.CreateGridForLevel(new Vector2Int(level.ColumnCount, level.RowCount), boardConfig);
            return CurrentGrid;
        }
    }
}
