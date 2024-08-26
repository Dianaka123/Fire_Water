using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    public class Array2D<T>
    {
        public int RowCount {  get; }
        public int ColumnCount { get; }

        public int Length => Array1D.Length;
        public Vector2Int Size { get; }
        public T[] Array1D {  get; }

        public Array2D(T[] array,int rowCount, int columnCount)
        {
            Array1D = array;
            RowCount = rowCount;
            ColumnCount = columnCount;
            Size = new Vector2Int(columnCount, rowCount);
        }

        public Array2D(int rowCount, int columnCount)
        {
            Array1D = new T[rowCount * columnCount];
            RowCount = rowCount;
            ColumnCount = columnCount;
            Size = new Vector2Int(columnCount, rowCount);
        }

        public Array2D(Vector2Int size)
        {
            Array1D = new T[size.x * size.y];
            RowCount = size.y;
            ColumnCount = size.x;
        }

        public T this[int x, int y]
        {
            get => Array1D[GetRawIndex(x,y)];
            set => Array1D[GetRawIndex(x, y)] = value;
        }

        public T this[Vector2Int index]
        {
            get => this[index.x, index.y];
            set => this[index.x, index.y] = value;
        }

        public void ForEach(Action<Vector2Int> action)
        {
            for (int y = 0; y < RowCount; y++)
            {
                for(int x = 0; x < ColumnCount; x++)
                {
                    action(new Vector2Int(x, y));
                }
            }
        }

        public void ForEach(Action<T> action)
        {
            for (int y = 0; y < RowCount; y++)
            {
                for (int x = 0; x < ColumnCount; x++)
                {
                    action(this[x, y]);
                }
            }
        }

        public Array2D<T> Clone()
        {
            return new Array2D<T>(Array1D.ToArray(), RowCount, ColumnCount);
        }

        private int GetRawIndex(int x, int y)
        {
            return y * ColumnCount + x;
        }
    }
}
