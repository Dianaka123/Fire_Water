using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class BoardNormalizer : IBoardNormalizer
    {
        private const int MIN_SEQUENCE_LENGTH = 3;

        private static readonly Vector2Int[] HORIZONTAL_DIRECTIONS = new[]
        {
            Vector2Int.left,
            Vector2Int.right,
        };

        private static readonly Vector2Int[] VERTICAL_DIRECTIONS = new[]
        {
            Vector2Int.up,
            Vector2Int.down,
        };

        private static readonly Vector2Int[] DIRECTIONS = HORIZONTAL_DIRECTIONS
            .Concat(VERTICAL_DIRECTIONS)
            .ToArray();

        public Vector2Int[] GetBlockSequenceForDestroying(Array2D<int> level, int emptyCellId)
        {
            var result = new HashSet<Vector2Int>();
            var visitedIndexes = new HashSet<Vector2Int>();
            level.ForEach(index =>
            {
                if (level[index] == emptyCellId || visitedIndexes.Contains(index))
                {
                    return;
                }

                visitedIndexes.Add(index);

                var maxSequenceLength = MaxSequenceLength(level, index);

                if (maxSequenceLength < MIN_SEQUENCE_LENGTH)
                {
                    return;
                }

                HashSet<Vector2Int> sequence = new HashSet<Vector2Int>();
                GetSequence(level, index, level[index], sequence);

                foreach (var it in sequence)
                {
                    visitedIndexes.Add(it);
                    result.Add(it);
                }
            });

            return result.ToArray();
        }

        private static int MaxSequenceLength(Array2D<int> level, Vector2Int index)
        {
            var value = level[index];

            int CheckSide(IEnumerable<Vector2Int> steps)
            {
                int result = 0;
                foreach (Vector2Int step in steps)
                {
                    int counter = 0;
                    Vector2Int current = index + step;
                    while (IsCellValid(level, current, value))
                    {
                        current += step;
                        counter++;
                    }
                    result += counter;
                }
                return result;
            }

            int horizontal = CheckSide(HORIZONTAL_DIRECTIONS);
            int vertical = CheckSide(VERTICAL_DIRECTIONS);
            return Math.Max(horizontal, vertical) + 1;
        }

        private static bool IsCellValid(Array2D<int> level, Vector2Int index, int value)
        {
            return index.y >= 0
                && index.x >= 0
                && index.y < level.RowCount
                && index.x < level.ColumnCount
                && level[index] == value;
        }

        private static void GetSequence(Array2D<int> level, Vector2Int index, int value, HashSet<Vector2Int> sequence)
        {
            if (!IsCellValid(level, index, value) || sequence.Contains(index))
            {
                return;
            }

            sequence.Add(index);

            foreach (var direction in DIRECTIONS)
            {
                GetSequence(level, index + direction, value, sequence);
            }
        }
    }
}
