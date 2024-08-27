using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Views;
using Assets.Scripts.Wrappers;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BlocksManger : IBlockManager
    {
        private const float MoveDuration = 0.5f;

        private readonly BlocksPool _pool;

        private Array2D<Block> _blocks;

        public BlocksManger(BlocksPool pool)
        {
            _pool = pool;
        }

        public void CreateBlocks(Array2D<int> level, GridData grid, Transform parent)
        {
            ClearBlocks();

            if (_blocks == null || _blocks?.Length != level.Length)
            {
                _blocks = new Array2D<Block>(level.RowCount, level.ColumnCount);
            }

            float blockSize = grid.CellSize;

            level.ForEach(index =>
            {
                var blockId = level[index];
                if (blockId >= 0)
                {
                    var position = grid.Indexes[index.x, index.y];
                    var block = _pool.GetBlockByID(blockId);
                    ConfigureBlock(block, position, blockSize, parent);
                    _blocks[index] = block;
                }
            });
        }

        public void RestartLevel(Array2D<int> level, GridData grid, Transform parent)
        {
            ClearBlocks();

            CreateBlocks(level, grid, parent);
        }

        public async UniTask SwitchBlocksAsync(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition)
        {
            var block1 = _blocks[cellFrom];
            var block2 = _blocks[cellTo];

            _blocks[cellTo] = block1;
            _blocks[cellFrom] = block2;

            UpdateSiblingIndices();

            await SwitchBlocksAnimationAsync(block1, block2, startPosition, endPosition);
        }

        public async UniTask DestroyAsync(Vector2Int[] indexes)
        {
            var animations = new List<UniTask>(indexes.Length);

            foreach (var index in indexes)
            {
                animations.Add(_blocks[index].DestroyAnimation());
            }

            await UniTask.WhenAll(animations);

            foreach (var index in indexes)
            {
                DestroyBlock(index);
            }
        }

        private void ClearBlocks()
        {
            if (_blocks == null)
            {
                return;
            }

            _blocks.ForEach(index =>
            {
                if (_blocks[index] != null)
                {
                    DestroyBlock(index);
                }
            });
        }

        private void DestroyBlock(Vector2Int index)
        {
            _pool.DestroyBlock(_blocks[index]);
            _blocks[index] = null;
        }

        private void ConfigureBlock(Block block, Vector2 position, float size, Transform parent)
        {
            block.transform.SetParent(parent);
            block.transform.localPosition = position;
            block.SetSize(size);
        }

        private UniTask SwitchBlocksAnimationAsync(Block from, Block to, Vector3 startPosition, Vector3 endPosition)
        {
            return UniTask.WhenAll(
                MoveBlockAnimationAsync(from, endPosition),
                MoveBlockAnimationAsync(to, startPosition)
            );
        }

        private async UniTask MoveBlockAnimationAsync(Block block, Vector3 to)
        {
            if (block != null)
            {
                await block.AnimateMovingAsync(to, MoveDuration);
            }
        }

        private void UpdateSiblingIndices()
        {
            int counter = 0;
            _blocks.ForEach(index =>
            {
                var block = _blocks[index];
                if (block != null)
                {
                    block.SiblingIndex = counter;
                    counter++;
                }
            });

        }
    }
}
