using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Services.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class BoardService : IBoardService
    {
        private readonly ICanvasManger _canvasManager;
        
        public BoardService(ICanvasManger canvasManger)
        {
            _canvasManager = canvasManger;
        }

        private Vector2[,] _indexes;
        public Vector2[,] Indexes => _indexes;
        
        private float _cellSize;
        public float CellSize => _cellSize;

        public void CreateBoardForLevel(Vector2Int gridSize, BoardConfig boardConfig)
        {
            var rows = gridSize.y;
            var columns = gridSize.x;

            if(!IsGridSizeValid(gridSize) || !IsBoardConfigValid(boardConfig))
            {
                _indexes = null;
                throw new ArgumentException("Invalid parameters.");
            }

            //optimization
            if(_indexes != null && _indexes.Length == rows * columns)
            {
                return;
            }

            _indexes = new Vector2[rows, columns];

            var boardSize = CalculateBoardSize(boardConfig);

            _cellSize = CalculateCellSize(boardSize, rows, columns);
            var offset = OffsetForCentralizeBlocks(_cellSize, boardSize.x, columns);

            var bottomCoordinate = new Vector3(-boardSize.x / 2 + offset / 2, -boardSize.y / 2);

            var x = bottomCoordinate.x;
            var y = bottomCoordinate.y;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _indexes[i, j] = new Vector2(x, y);
                    x += _cellSize;
                }

                x = bottomCoordinate.x;
                y += _cellSize;
            }
        }

        private Vector2 CalculateBoardSize(BoardConfig config)
        {
            var canvasSize = _canvasManager.Size;
            var canvasScale = _canvasManager.LocalScale;

            var width = (canvasSize.x - 2 * config.SideOffset) * canvasScale.x;
            var height = (canvasSize.y - 2 * config.BottomOffset) * canvasScale.y;

            return new Vector2(width, height);
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
            return boardConfig.SideOffset > 0 && boardConfig.BottomOffset > 0;
        }

        private bool IsGridSizeValid(Vector2Int gridSize)
        {
            return gridSize.x > 0 && gridSize.y > 0;
        }
    }
}
