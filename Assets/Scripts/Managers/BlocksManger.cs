using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BlocksManger : IBlockManager
    {
        private readonly GameResources _gameResources;

        private Block[,] _blocks;

        public BlocksManger(GameResources gameResources)
        {
            _gameResources = gameResources;
        }

        public void CreateBlocks(int[,] level, GridData grid, Transform parent)
        {
            var row = grid.Indexes.GetLength(0);
            var column = grid.Indexes.GetLength(1);
            _blocks = new Block[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    var blockIndex = level[i, j];
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

        public async UniTask SwitchBlocksAsync(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition)
        {
            MoveBlockAsync(cellFrom, endPosition).Forget();
            MoveBlockAsync(cellTo, startPosition).Forget();

            var block1 = _blocks[cellFrom.x, cellFrom.y];
            var block2 = _blocks[cellTo.x, cellTo.y];

            if(block2 != null && block1 != null)
            {
                var index1 = block1.SiblingIndex;
                var index2 = block2.SiblingIndex;

                block1.SiblingIndex = index2;
                block2.SiblingIndex = index1;

                Debug.Log($"{block1.SiblingIndex} + {block2.SiblingIndex}");
            }

            _blocks[cellTo.x, cellTo.y] = block1;
            _blocks[cellFrom.x, cellFrom.y] = block2;

            Debug.Log($"{_blocks[cellTo.x, cellTo.y].SiblingIndex} " +
                $"+ {_blocks[cellFrom.x, cellFrom.y].SiblingIndex}");
        }

        private async UniTask MoveBlockAsync(Vector2Int blockIndex, Vector3 to)
        {
            var block = _blocks[blockIndex.x, blockIndex.y];
            if (block != null)
            {
                await block.AnimateMovingAsync(to, 0.5f);
            }
        }
    }
}
