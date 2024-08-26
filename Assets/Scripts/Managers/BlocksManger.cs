using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Views;
using Assets.Scripts.Wrappers;
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

        private Array2D<Block> _blocks;

        public BlocksManger(GameResources gameResources)
        {
            _gameResources = gameResources;
        }

        public void CreateBlocks(Array2D<int> level, GridData grid, Transform parent)
        {
            _blocks = new Array2D<Block>(level.RowCount, level.ColumnCount);
            float blockSize = grid.CellSize;

            level.ForEach(index =>
            {
                var blockValue = level[index];
                if (blockValue >= 0)
                {
                    var position = grid.Indexes[index.x, index.y];
                    var blockPrefab = _gameResources.Bloks[blockValue];
                    _blocks[index] = CreateBlock(index, blockPrefab, position, blockSize, parent);
                }
            });
        }

        public async UniTask SwitchBlocksAsync(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition)
        {
            await SwitchBlocksAnimationAsync(cellFrom, cellTo, startPosition, endPosition);

            var block1 = _blocks[cellFrom];
            var block2 = _blocks[cellTo];

            _blocks[cellTo] = block1;
            _blocks[cellFrom] = block2;

            UpdateSiblingIndices();
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
                _blocks[index].DestroyBlock();
                _blocks[index] = null;
            }
        }

        private Block CreateBlock(Vector2Int index, Block blockPrefab, Vector2 position, float size, Transform parent)
        {
            var block = GameObject.Instantiate(blockPrefab, parent);
            block.transform.localPosition = position;
            block.SetSize(size);

            return block;
        }

        private UniTask SwitchBlocksAnimationAsync(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition)
        {
            return UniTask.WhenAll(
                MoveBlockAnimationAsync(cellFrom, endPosition),
                MoveBlockAnimationAsync(cellTo, startPosition)
            );
        }

        private async UniTask MoveBlockAnimationAsync(Vector2Int blockIndex, Vector3 to)
        {
            var block = _blocks[blockIndex];
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
