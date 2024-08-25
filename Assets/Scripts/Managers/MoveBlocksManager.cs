using Assets.Scripts.Managers.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class MoveBlocksManager
    {
        private readonly ILevelManager _levelManager;
        private readonly IGridManager _gridManager;
        private readonly IBlockManager _blocksManger;

        private int _row => _levelManager.CurrentLevelSequence.GetLength(0);
        private int _column => _levelManager.CurrentLevelSequence.GetLength(1);

        public MoveBlocksManager(ILevelManager levelManager, IBlockManager blocksManger, IGridManager gridManager)
        {
            _levelManager = levelManager;
            _blocksManger = blocksManger;
            _gridManager = gridManager;
        }

        public async UniTask MoveBlockAsync(Vector2Int from, Vector2Int to)
        {
            if(to.x >= _row || to.x < 0 || to.y < 0 || to.y >= _column)
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
    }
}
