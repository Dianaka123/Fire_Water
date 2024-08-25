using Assets.Scripts.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ILevelManager
    {
        public int[,] CurrentLevelSequence { get; }
        HashSet<int> LevelBlocksType { get; }

        void UpdateLevelBySavingData(Level levelSavingData);
        void SwitchBlocks(Vector2Int from, Vector2Int to);
        bool IsEmptyCell(Vector2Int cellIndex);
    }
}
