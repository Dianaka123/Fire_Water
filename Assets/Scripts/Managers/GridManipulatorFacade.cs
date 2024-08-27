using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
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
        private readonly IBlocksManager _blocksManger;
        private readonly IBoardNormalizer _boardNormalizer;
        private readonly ISaveLevelService _saveLevelService;

        private int _row => _levelManager.CurrentLevelSequence.RowCount;
        private int _column => _levelManager.CurrentLevelSequence.ColumnCount;

        public GridManipulatorFacade(ILevelManager levelManager, IBlocksManager blocksManger, IGridManager gridManager, IBoardNormalizer boardNormalizer, ISaveLevelService saveLevelService)
        {
            _levelManager = levelManager;
            _blocksManger = blocksManger;
            _gridManager = gridManager;
            _boardNormalizer = boardNormalizer;
            _saveLevelService = saveLevelService;
        }

        public async UniTask SwitchCellsThenNormalize(Vector3 startPosition, Vector2Int directon)
        {
            Vector2Int startCell = GetStartCell(startPosition);
            Vector2Int nextCell = startCell + directon;

            if (!IsSwitchAvailable(startCell, nextCell))
            {
                return;
            }

            await SwitchAsync(startCell, nextCell);
            await CheckFallBlocksAsync();
            await ReshuffleAsync();
            _saveLevelService.SaveLevelStateAsync().Forget();
        }

        private Vector2Int GetStartCell(Vector3 position)
            => _gridManager.GetCellIndexByScreenPosition(position) ?? InvalidCell;

        private bool IsSwitchAvailable(Vector2Int startCell, Vector2Int nextCell)
        {
            if (AreCellsInvalid(startCell, nextCell))
            {
                return false;
            }

            return !_levelManager.IsEmptyCell(startCell) && !IsMoveUpByEmptyCell(startCell, nextCell);
        }

        private bool AreCellsInvalid(Vector2Int from, Vector2Int to)
            => from == to
            || from == InvalidCell
            || to.x >= _column
            || to.x < 0
            || to.y < 0
            || to.y >= _row;

        private bool IsMoveUpByEmptyCell(Vector2Int from, Vector2Int to)
        {
            bool isUp = (to - from).y > 0;
            return isUp && _levelManager.IsEmptyCell(to);
        }

        private async UniTask SwitchAsync(Vector2Int from, Vector2Int to)
        {
            _levelManager.SwitchBlocks(from, to);

            Vector2 startPosition = _gridManager.GetScreenPositionByCellIndex(from);
            Vector2 endPosition = _gridManager.GetScreenPositionByCellIndex(to);

            await _blocksManger.SwitchBlocksAsync(from, to, startPosition, endPosition);
        }

        private async UniTask ReshuffleAsync()
        {
            Vector2Int[] indexesToDestroy = _boardNormalizer.GetBlockSequenceForDestroying(_levelManager.CurrentLevelSequence, _levelManager.EmptyCellId);
            if (indexesToDestroy.Length == 0)
            {
                return;
            }

            await _blocksManger.DestroyBlocksAsync(indexesToDestroy);
            foreach (var index in indexesToDestroy)
            {
                _levelManager.SetEmptyCell(index);
            }

            await CheckFallBlocksAsync();
            await ReshuffleAsync();
        }

        private async UniTask CheckFallBlocksAsync()
        {
            Array2D<int> levelSequence = _levelManager.CurrentLevelSequence;
            var animations = new List<UniTask>();
            for (var x = 0; x < levelSequence.ColumnCount; x++)
            {
                int currentBottom = 0;
                for (var y = 0; y < levelSequence.RowCount; y++)
                {
                    int value = levelSequence[x, y];
                    if (value != _levelManager.EmptyCellId)
                    {
                        var shouldFall = currentBottom != y;
                        if (shouldFall)
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
