using Assets.Scripts.Managers.Interfaces;
using System.Numerics;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class MoveBlocksManager
    {
        private readonly ILevelManager _levelManager;
        private readonly GridManager _gridManager;
        private readonly BlocksManger _blocksManger;

        private int _row => _levelManager.CurrentLevel.Row;
        private int _column => _levelManager.CurrentLevel.Column;

        public MoveBlocksManager(ILevelManager levelManager, BlocksManger blocksManger, GridManager gridManager)
        {
            _levelManager = levelManager;
            _blocksManger = blocksManger;
            _gridManager = gridManager;

        }

        public void MoveBlock(Vector2Int from, Vector2Int to)
        {
            if(to.x >= _row && to.x < 0 || to.y < 0 && to.y >= _column)
            {
                return;
            }
            Debug.Log(to);

            var startPosition = _gridManager.GetScreenPositionByCellIndex(from);
            var endPosition = _gridManager.GetScreenPositionByCellIndex(to);

            _blocksManger.SwitchBlocks(from, to, startPosition, endPosition);
        }
    }
}
