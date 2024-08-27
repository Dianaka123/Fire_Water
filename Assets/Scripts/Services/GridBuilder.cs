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
            if(!IsGridSizeValid(gridSize) || !IsBoardConfigValid(boardConfig))
            {
                throw new ArgumentException("Invalid parameters.");
            }

            if(_currentGrid.Indexes?.Length == gridSize.x * gridSize.y)
            {
                return _currentGrid;
            }

            var indexes = new Array2D<Vector2>(gridSize);
            var rowCount = indexes.RowCount;
            var columnCount = indexes.ColumnCount;

            var boardSize = CalculateBoardSize(boardConfig, windowSize);

            var cellSize = CalculateCellSize(boardSize, rowCount, columnCount);

            var offset = OffsetForCentralizeBlocks(cellSize, boardSize.x, columnCount);

            var bottomLeftCenterCoordinate = new Vector3(-boardSize.x / 2 + offset / 2 + cellSize / 2, -boardSize.y / 2 + cellSize / 2);

            var x = bottomLeftCenterCoordinate.x;
            var y = bottomLeftCenterCoordinate.y;

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

        private Vector2 CalculateBoardSize(BoardConfig config, Vector2 windowSize)
        {
            var sideOffset = CalculateOffset(config.RelativeSideOffset, windowSize.x);
            var bottomOffset = CalculateOffset(config.RelativeBottomOffset, windowSize.y);

            var width = windowSize.x - sideOffset * 2;
            var height = windowSize.y - bottomOffset * 2;

            return new Vector2(width, height);
        }

        private float CalculateOffset(float offset, float sideSize)
        {
            return sideSize * offset;
        }
        
        private float CalculateCellSize(Vector2 boardSize, int rowCount, int columnCount)
        {
            var cellSizeX = boardSize.x / columnCount;
            var cellSizeY = boardSize.y / rowCount;

            return Mathf.Min(cellSizeX, cellSizeY);
        }
    
        private float OffsetForCentralizeBlocks(float cellSize, float width, int columnCount)
        {
            var length = cellSize * columnCount;

            return (width - length);
        }

        private bool IsBoardConfigValid(BoardConfig boardConfig)
        {
            return boardConfig.RelativeSideOffset >= 0 && boardConfig.RelativeBottomOffset >= 0;
        }

        private bool IsGridSizeValid(Vector2Int gridSize)
        {
            return gridSize.x > 0 && gridSize.y > 0;
        }
    }
}
