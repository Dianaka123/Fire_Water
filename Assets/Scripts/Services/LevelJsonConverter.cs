using Assets.Scripts.Configs;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using Newtonsoft.Json;

namespace Assets.Scripts.Services
{
    public class LevelJsonConverter : ILevelJsonConverter
    {
        public string SerializeLevel(Level level)
        {
            var levelDesc = new LevelDesc()
            {
                RowCount = level.LevelBlocksSequence.RowCount,
                ColumnCount = level.LevelBlocksSequence.ColumnCount,
                LevelBlocksSequence = level.LevelBlocksSequence.Array1D,
            };

            return JsonConvert.SerializeObject(levelDesc);
        }

        public Level DeserializeLevel(string txt)
        {
            var deserializedData = JsonConvert.DeserializeObject<LevelDesc>(txt);
            var level2d = new Array2D<int>(deserializedData.LevelBlocksSequence, deserializedData.RowCount, deserializedData.ColumnCount);

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
                var rowCount = levelDesc.RowCount;
                var columnCount = levelDesc.ColumnCount;

                var levelSequence = ConvertArray(levelDesc.LevelBlocksSequence, rowCount, columnCount);
                levels[i] = new Level()
                {
                    LevelBlocksSequence = new Array2D<int>(levelSequence, rowCount, columnCount)
                };
            };

            return levels;
        }

        private int[] ConvertArray(int[] configLevelSequence, int rows, int columns)
        {
            var arr = new int[configLevelSequence.Length];
            var index = 0;

            for (var i = rows - 1; i >= 0; i--)
            {
                for (var j = 0; j < columns; j++)
                {
                    arr[i * columns + j] = configLevelSequence[index];
                    index++;
                }
            }

            return arr;
        }
    }
}
