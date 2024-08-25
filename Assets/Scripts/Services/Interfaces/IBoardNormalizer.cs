using UnityEngine;

namespace Assets.Scripts.Services.Interfaces
{
    public interface IBoardNormalizer
    {
        Vector2Int[] GetBlockSequenceForDestroying(int[,] levelSequence, int emptyCellId);
    }
}
