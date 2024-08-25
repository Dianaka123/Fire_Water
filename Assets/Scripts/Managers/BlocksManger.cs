using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Views;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BlocksManger : IBlockManager
    {
        private const float MoveDuration = 0.5f;

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
            await UniTask.WhenAll(
                MoveBlockAsync(cellFrom, endPosition),
                MoveBlockAsync(cellTo, startPosition)
            );

            var block1 = _blocks[cellFrom.x, cellFrom.y];
            var block2 = _blocks[cellTo.x, cellTo.y];

            _blocks[cellTo.x, cellTo.y] = block1;
            _blocks[cellFrom.x, cellFrom.y] = block2;

            UpdateSiblingIndices();
        }

        public async UniTask DestroyAsync(Vector2Int[] indexes, CancellationToken token)
        {
            var animations = new List<UniTask>(indexes.Length);

            foreach (var index in indexes)
            {
                animations.Add(_blocks[index.x, index.y].DestroyAnimation(token));
            }

            await UniTask.WhenAll(animations);

            foreach (var index in indexes)
            {
                _blocks[index.x, index.y].DestroyBlock();
                _blocks[index.x, index.y] = null;
            }
        }


        private async UniTask MoveBlockAsync(Vector2Int blockIndex, Vector3 to)
        {
            var block = _blocks[blockIndex.x, blockIndex.y];
            if (block != null)
            {
                await block.AnimateMovingAsync(to, MoveDuration);
            }
        }

        private void UpdateSiblingIndices()
        {
            int counter = 0;
            for (int i = 0; i < _blocks.GetLength(0); i++)
            {
                for (int j = 0; j < _blocks.GetLength(1); j++)
                {
                    var block = _blocks[i, j];
                    if (block != null)
                    {
                        block.SiblingIndex = counter;
                        counter++;
                    }
                }
            }
        }
    }
}
