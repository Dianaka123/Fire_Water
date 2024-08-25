using Assets.Scripts.Configs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.Services
{
    public class LevelJsonConverter
    {
        public string SerializeLevel(Level level)
        {
            var levelDesc = new LevelDesc()
            {
                RowCount = level.LevelBlocksSequence.GetLength(0),
                ColumnCount = level.LevelBlocksSequence.GetLength(1),
                LevelBlocksSequence = ConvertTo1dArray(level.LevelBlocksSequence)
            };

            return JsonConvert.SerializeObject(levelDesc);
        }

        public Level DeserializeLevel(string txt)
        {
            var deserializedData = JsonConvert.DeserializeObject<LevelDesc>(txt);
            var level2d = ConvertTo2D(deserializedData.LevelBlocksSequence, deserializedData.RowCount, deserializedData.ColumnCount);

            return new Level() { LevelBlocksSequence = level2d};
        }

        public Level[] DeserializeAllLevels(string txt)
        { 
            var deserializedData = JsonConvert.DeserializeObject<Levels>(txt);
            var levelDescs = deserializedData.LevelsDesc;
            var levelCount = levelDescs.Length;

            Level[] levels = new Level[levelCount];
            for (int i = 0; i < levelCount; i++)
            {
                var levelDesc = levelDescs[i];

                levels[i] = new Level()
                {
                    LevelBlocksSequence = ConvertTo2D(levelDesc.LevelBlocksSequence, levelDesc.RowCount, levelDesc.ColumnCount),
                };
            }
            return levels;
        }

        private int[,] ConvertTo2D(int[] levelSequence, int rows, int columns)
        {
            var arr2d = new int[rows, columns];
            var index = 0;
            for (var i = rows - 1; i >= 0; i--)
            {
                for(var j = 0; j < columns; j++)
                {
                    arr2d[i,j] = levelSequence[index];
                    index++;
                }
            }

            return arr2d;
        }

        private int[] ConvertTo1dArray(int[,] arr2d)
        {
            var arr = new int[arr2d.Length];

            var rows = arr2d.GetLength(0);
            var columns = arr2d.GetLength(1);

            var index = 0;
            for (int i = rows - 1; i >= 0; i--)
            {
                for(int j = 0; j < columns; j++)
                {
                    arr[index] = arr2d[i, j];
                    index++;
                }
            }

            return arr;
        }
    }
}
