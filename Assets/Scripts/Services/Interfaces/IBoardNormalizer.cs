using Assets.Scripts.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Services.Interfaces
{
    public interface IBoardNormalizer
    {
        Vector2Int[] GetBlockSequenceForDestroying(Array2D<int> levelSequence, int emptyCellId);
    }
}
