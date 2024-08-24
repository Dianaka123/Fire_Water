using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Managers
{
    public class GridManager
    {
        private readonly IGridBuilder _gridBuilder;
        private readonly ICanvasManger _canvasManger;

        public GridData CurrentGrid { get; private set; }

        public GridManager(IGridBuilder gridBuilder, ICanvasManger canvasManger)
        {
            _gridBuilder = gridBuilder;
            _canvasManger = canvasManger;
        }

        public GridData CreateGrid(Vector2Int gridSize, BoardConfig boardConfig)
        {
            CurrentGrid = _gridBuilder.CreateGridForLevel(gridSize, boardConfig, _canvasManger.Size);
            return CurrentGrid;
        }

        public Vector2Int GetCellIndexByScreenPosition(Vector3 screenPosition)
        {
            var position = TranslateToCanvasCoordinate(screenPosition);
            var column = CurrentGrid.Indexes.GetLength(1);
            var row = CurrentGrid.Indexes.GetLength(0);
            var halfCellSize = CurrentGrid.CellSize / 2;

            for (int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    var cellPosition = CurrentGrid.Indexes[i, j];
                    if ((cellPosition.x - halfCellSize < position.x && position.x < cellPosition.x + halfCellSize)
                        && (cellPosition.y - halfCellSize < position.y && position.y < cellPosition.y + halfCellSize))
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }
            return Vector2Int.one * -1;
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
