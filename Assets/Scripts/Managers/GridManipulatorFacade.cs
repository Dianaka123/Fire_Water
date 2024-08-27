using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GridManipulatorFacade
    {
        private readonly Vector2Int InvalidCell = Vector2Int.one * -1;

        private readonly ILevelManager _levelManager;
        private readonly IGridManager _gridManager;
        private readonly IBlockManager _blocksManger;
        private readonly IBoardNormalizer _boardNormalizer;

        private int _row => _levelManager.CurrentLevelSequence.RowCount;
        private int _column => _levelManager.CurrentLevelSequence.ColumnCount;

        public GridManipulatorFacade(ILevelManager levelManager, IBlockManager blocksManger, IGridManager gridManager, IBoardNormalizer boardNormalizer)
        {
            _levelManager = levelManager;
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _boardNormalizer = boardNormalizer;
        }

        public async UniTask SwitchCellsThenNormilize(Vector3 startPosition, Vector2Int directon)
        {
            var startCell = GetStartCell(startPosition);
            var nextCell = startCell + directon;
            var isSwitched = IsSwitchAvailable(startCell, nextCell);

            if (isSwitched)
            {
                await SwitchAsync(startCell, nextCell);
                await CheckFallBlocksAsync();
                await ReshuffleAsync();
            }
        }

        private Vector2Int GetStartCell(Vector3 position)
        {
            var startCellNullable = _gridManager.GetCellIndexByScreenPosition(position);

            return startCellNullable == null
                ? InvalidCell
                : startCellNullable.Value;
        }

        private bool IsSwitchAvailable(Vector2Int startCell, Vector2Int nextCell)
        {
            if( IsCellsInvalid(startCell, nextCell))
                return false;

            if(_levelManager.IsEmptyCell(startCell) || IsMoveUpByEmptyCell(startCell, nextCell))
            {
                return false;
            }

             return true;
        }

        private bool IsCellsInvalid(Vector2Int from, Vector2Int to)
            => from == to
            || from == InvalidCell
            || to.x >= _column
            || to.x < 0
            || to.y < 0
            || to.y >= _row;

        private bool IsMoveUpByEmptyCell(Vector2Int from, Vector2Int to)
        {
            var isUp = (to - from).y > 0;

            return isUp && _levelManager.IsEmptyCell(to);
        }

        private async UniTask SwitchAsync(Vector2Int from, Vector2Int to)
        {
            _levelManager.SwitchBlocks(from, to);

            var startPosition = _gridManager.GetScreenPositionByCellIndex(from);
            var endPosition = _gridManager.GetScreenPositionByCellIndex(to);

            await _blocksManger.SwitchBlocksAsync(from, to, startPosition, endPosition);
        }

        private async UniTask ReshuffleAsync()
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

        private async UniTask CheckFallBlocksAsync()
        {
            var levelSequence = _levelManager.CurrentLevelSequence;
            var animations = new List<UniTask>();
            for (var x = 0; x < levelSequence.ColumnCount; x++)
            {
                var currentBottom = 0;
                for (var y = 0; y < levelSequence.RowCount; y++)
                {
                    var value = levelSequence[x, y];
                    if (value != _levelManager.EmptyCellId )
                    {
                        var isFall = currentBottom != y;
                        if (isFall)
                        {
                            animations.Add(SwitchAsync(new Vector2Int(x, y), new Vector2Int(x, currentBottom)));
                        }
                        currentBottom++;
                    }
                }
            }

            await UniTask.WhenAll(animations);
        }
    }
}
