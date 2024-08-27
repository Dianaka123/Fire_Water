using Assets.Scripts.Data;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GridManager : IGridManager
    {
        private readonly IGridBuilder _gridBuilder;
        private readonly IUIManger _canvasManger;

        private GridData _currentGrid;

        public GridData CurrentGrid => _currentGrid;

        public GridManager(IGridBuilder gridBuilder, IUIManger canvasManger)
        {
            _gridBuilder = gridBuilder;
            _canvasManger = canvasManger;
        }

        public GridData CreateGrid(Vector2Int gridSize, BoardConfig boardConfig)
            => _currentGrid = _gridBuilder.CreateGridForLevel(gridSize, boardConfig, _canvasManger.Size);

        public Vector2Int? GetCellIndexByScreenPosition(Vector3 screenPosition)
        {
            var position = TranslateToCanvasCoordinate(screenPosition);
            var halfCellSize = _currentGrid.CellSize / 2;

            Vector2Int? cellIndex = null;
            _currentGrid.Indexes.ForEach(index =>
            {
                var cellPosition = _currentGrid.Indexes[index];
                if (cellPosition.x - halfCellSize < position.x
                    && position.x < cellPosition.x + halfCellSize
                    && cellPosition.y - halfCellSize < position.y
                    && position.y < cellPosition.y + halfCellSize)
                {
                    cellIndex = index;
                }
            });

            return cellIndex;
        }

        public Vector2 GetScreenPositionByCellIndex(Vector2Int cellIndex) => _currentGrid.Indexes[cellIndex];

        private Vector2 TranslateToCanvasCoordinate(Vector3 screenPosition)
        {
            Vector2 pos = screenPosition;
            pos.x -= _canvasManger.Size.x / 2;
            pos.y -= _canvasManger.Size.y / 2;
            
            return pos;
        }
    }
}
