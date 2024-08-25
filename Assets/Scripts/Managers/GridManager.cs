using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GridManager : IGridManager
    {
        private readonly IGridBuilder _gridBuilder;
        private readonly ICanvasManger _canvasManger;

        private GridData _currentGrid;

        public GridManager(IGridBuilder gridBuilder, ICanvasManger canvasManger)
        {
            _gridBuilder = gridBuilder;
            _canvasManger = canvasManger;
        }

        public GridData CreateGrid(Vector2Int gridSize, BoardConfig boardConfig)
        {
            _currentGrid = _gridBuilder.CreateGridForLevel(gridSize, boardConfig, _canvasManger.Size);
            return _currentGrid;
        }

        public Vector2Int GetCellIndexByScreenPosition(Vector3 screenPosition)
        {
            var position = TranslateToCanvasCoordinate(screenPosition);
            var column = _currentGrid.Indexes.GetLength(1);
            var row = _currentGrid.Indexes.GetLength(0);
            var halfCellSize = _currentGrid.CellSize / 2;

            for (int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    var cellPosition = _currentGrid.Indexes[i, j];
                    if ((cellPosition.x - halfCellSize < position.x && position.x < cellPosition.x + halfCellSize)
                        && (cellPosition.y - halfCellSize < position.y && position.y < cellPosition.y + halfCellSize))
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }
            return Vector2Int.one * -1;
        }

        public Vector2 GetScreenPositionByCellIndex(Vector2Int cellIndex)
        {
            return _currentGrid.Indexes[cellIndex.x, cellIndex.y];
        }

        private Vector2 TranslateToCanvasCoordinate(Vector3 screenPosition)
        {
            Vector2 pos = screenPosition;
            var width = _canvasManger.Size.x / 2;
            var height = _canvasManger.Size.y / 2;

            pos.x = pos.x - width;
            pos.y = pos.y - height;

            return pos;
        }
    }
}
