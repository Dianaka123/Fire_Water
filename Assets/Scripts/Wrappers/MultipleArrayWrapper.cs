using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Wrappers
{
    public class MultipleArrayWrapper<T>
    {
        public int RowCount {  get; }
        public int ColumnCount { get; }

        public T[] Array {  get; }

        public MultipleArrayWrapper(T[] array,int rowCount, int columnCount)
        {
            Array = array;
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        public MultipleArrayWrapper(int rowCount, int columnCount)
        {
            Array = new T[rowCount * columnCount];
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        public T this[int x, int y]
        {
            get => Array[GetRawIndex(x,y)];
            set => Array[GetRawIndex(x, y)] = value;
        }

        private int GetRawIndex(int x, int y)
        {
            return y * ColumnCount + x;
        }
    }
}
