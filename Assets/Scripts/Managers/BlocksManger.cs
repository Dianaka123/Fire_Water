using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Views;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BlocksManger
    {
        private readonly GameResources _gameResources;

        private Block[,] _blocks;

        public BlocksManger(GameResources gameResources)
        {
            _gameResources = gameResources;
        }

        public void CreateBlocks(Level level, GridData grid, Transform parent)
        {
            var row = grid.Indexes.GetLength(0);
            var column = grid.Indexes.GetLength(1);
            _blocks = new Block[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    var blockIndex = level.LevelBlocksSequence[i, j];
                    if (blockIndex >= 0)
                    {
                        var position = grid.Indexes[i, j];
                        var block = GameObject.Instantiate(_gameResources.Bloks[blockIndex], parent);
                        block.transform.localPosition = position;
                        block.SetSize(grid.CellSize);

                        _blocks[i, j] = block;
                    }
                }
            }
        }

        public void MoveBlock(Vector2Int blockIndex, Vector3 to)
        {
            var block = _blocks[blockIndex.x, blockIndex.y];
            if ( block != null)
            {
                block.MoveTo(to);
            }
        }

        public void SwitchBlocks(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition)
        {
            MoveBlock(cellFrom, endPosition);
            MoveBlock(cellTo, startPosition);

            var block1 = _blocks[cellFrom.x, cellFrom.y];
            var block2 = _blocks[cellTo.x, cellTo.y];

            _blocks[cellTo.x, cellTo.y] = block1;
            _blocks[cellFrom.x, cellFrom.y] = block2;

            Debug.Log(_blocks[cellTo.x, cellTo.y]);
            Debug.Log(_blocks[cellFrom.x, cellFrom.y]);
        }

    }
}
