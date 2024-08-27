using Assets.Scripts.Data;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class GridBuilder : IGridBuilder
    {
        private GridData _currentGrid;

        public GridData CreateGridForLevel(Vector2Int gridSize, BoardConfig boardConfig, Vector2 windowSize)
        {
            if (!IsGridSizeValid(gridSize) || !IsBoardConfigValid(boardConfig))
            {
                throw new ArgumentException("Invalid parameters.");
            }

            if (_currentGrid.Indexes?.Length == gridSize.x * gridSize.y)
            {
                return _currentGrid;
            }

            var indexes = new Array2D<Vector2>(gridSize);
            int rowCount = indexes.RowCount;
            int columnCount = indexes.ColumnCount;

            Vector2 boardSize = CalculateBoardSize(boardConfig, windowSize);

            float cellSize = CalculateCellSize(boardSize, rowCount, columnCount);

            float offset = OffsetForCentralizeBlocks(cellSize, boardSize.x, columnCount);

            var bottomLeftCenterCoordinate = new Vector3(-boardSize.x / 2 + offset / 2 + cellSize / 2, -boardSize.y / 2 + cellSize / 2);

            float x = bottomLeftCenterCoordinate.x;
            float y = bottomLeftCenterCoordinate.y;

            for (int y1 = 0; y1 < rowCount; y1++)
            {
                for (int x1 = 0; x1 < columnCount; x1++)
                {
                    indexes[x1, y1] = new Vector2(x, y);
                    x += cellSize;
                }

                x = bottomLeftCenterCoordinate.x;
                y += cellSize;
            }

            return new GridData() { Indexes = indexes, CellSize = cellSize };
        }

        private static Vector2 CalculateBoardSize(BoardConfig config, Vector2 windowSize)
        {
            float sideOffset = CalculateOffset(config.RelativeSideOffset, windowSize.x);
            float bottomOffset = CalculateOffset(config.RelativeBottomOffset, windowSize.y);

            float width = windowSize.x - sideOffset * 2;
            float height = windowSize.y - bottomOffset * 2;

            return new Vector2(width, height);
        }

        private static float CalculateOffset(float offset, float sideSize) => sideSize * offset;

        private static float CalculateCellSize(Vector2 boardSize, int rowCount, int columnCount)
        {
            float cellSizeX = boardSize.x / columnCount;
            float cellSizeY = boardSize.y / rowCount;

            return Mathf.Min(cellSizeX, cellSizeY);
        }

        private static float OffsetForCentralizeBlocks(float cellSize, float width, int columnCount) => width - cellSize * columnCount;

        private static bool IsBoardConfigValid(BoardConfig boardConfig) => boardConfig.RelativeSideOffset >= 0 && boardConfig.RelativeBottomOffset >= 0;

        private static bool IsGridSizeValid(Vector2Int gridSize) => gridSize.x > 0 && gridSize.y > 0;
    }
}
