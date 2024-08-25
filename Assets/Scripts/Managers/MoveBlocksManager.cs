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

        private int _row => _levelManager.CurrentLevelSequence.GetLength(0);
        private int _column => _levelManager.CurrentLevelSequence.GetLength(1);

        public MoveBlocksManager(ILevelManager levelManager, IBlockManager blocksManger, IGridManager gridManager, IBoardNormalizer boardNormalizer)
        {
            _levelManager = levelManager;
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _boardNormalizer = boardNormalizer;
        }

        public async UniTask MoveBlockAsync(Vector2Int from, Vector2Int to)
        {
            if(to.x >= _row || to.x < 0 || to.y < 0 || to.y >= _column || from == to)
            {
                return;
            }

            var isUp = (to - from).x > 0;

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

        public async UniTask Reshuffle(CancellationToken token)
        {
            var indexesToDestroy = _boardNormalizer.GetBlockSequenceForDestroying(_levelManager.CurrentLevelSequence, _levelManager.EmptyCellId);
            
            if(indexesToDestroy.Length == 0)
            {
                return;
            }

            await _blocksManger.DestroyAsync(indexesToDestroy, token);

            foreach( var index in indexesToDestroy)
            {
                _levelManager.SetEmptyCell(index);
            }

            var levelSequence = _levelManager.CurrentLevelSequence;
            var animations = new List<UniTask>();
            for ( var j = 0; j < levelSequence.GetLength(1); j++)
            {
                var currentBottom = 0;
                for (var i = 0; i < levelSequence.GetLength(0); i++)
                {
                    var value = levelSequence[i, j];
                    if(value != _levelManager.EmptyCellId)
                    {
                        animations.Add(MoveBlockAsync(new Vector2Int(i,j), new Vector2Int(currentBottom,j)));
                        currentBottom++;
                    }
                }
            }

            await UniTask.WhenAll(animations);

            await Reshuffle(token);
        }
    }
}
