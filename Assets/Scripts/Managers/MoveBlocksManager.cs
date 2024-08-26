using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class MoveBlocksManager
    {
        private readonly ILevelManager _levelManager;
        private readonly IGridManager _gridManager;
        private readonly IBlockManager _blocksManger;
        private readonly IBoardNormalizer _boardNormalizer;

        private int _row => _levelManager.CurrentLevelSequence.RowCount;
        private int _column => _levelManager.CurrentLevelSequence.ColumnCount;

        public MoveBlocksManager(ILevelManager levelManager, IBlockManager blocksManger, IGridManager gridManager, IBoardNormalizer boardNormalizer)
        {
            _levelManager = levelManager;
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _boardNormalizer = boardNormalizer;
        }

        public async UniTask MoveBlockAsync(Vector2Int from, Vector2Int to)
        {
            if(to.x >= _column || to.x < 0 || to.y < 0 || to.y >= _row || from == to)
            {
                return;
            }

            var isUp = (to - from).y > 0;

            if (_levelManager.IsEmptyCell(from) 
                || (isUp && _levelManager.IsEmptyCell(to)))
            {
                return;
            }

            _levelManager.SwitchBlocks(from, to);

            var startPosition = _gridManager.GetScreenPositionByCellIndex(from);
            var endPosition = _gridManager.GetScreenPositionByCellIndex(to);

            await _blocksManger.SwitchBlocksAsync(from, to, startPosition, endPosition);
        }

        public async UniTask ReshuffleAsync()
        {
            var indexesToDestroy = _boardNormalizer.GetBlockSequenceForDestroying(_levelManager.CurrentLevelSequence, _levelManager.EmptyCellId);
            
            if(indexesToDestroy.Length == 0)
            {
                return;
            }

            await _blocksManger.DestroyAsync(indexesToDestroy);

            foreach( var index in indexesToDestroy)
            {
                _levelManager.SetEmptyCell(index);
            }

            await CheckFallBlocksAsync();

            await ReshuffleAsync();
        }

        public async UniTask CheckFallBlocksAsync()
        {
            var levelSequence = _levelManager.CurrentLevelSequence;
            var animations = new List<UniTask>();
            for (var x = 0; x < levelSequence.ColumnCount; x++)
            {
                var currentBottom = 0;
                for (var y = 0; y < levelSequence.RowCount; y++)
                {
                    var value = levelSequence[x, y];
                    if (value != _levelManager.EmptyCellId)
                    {
                        animations.Add(MoveBlockAsync(new Vector2Int(x, y), new Vector2Int(x, currentBottom)));
                        currentBottom++;
                    }
                }
            }

            await UniTask.WhenAll(animations);
        }
    }
}
