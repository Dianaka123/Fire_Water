using Newtonsoft.Json;

namespace Assets.Scripts.ScriptableObjects
{
    public class Levels
    {
        [JsonProperty("levels")]
        public LevelDesc[] LevelsDesc;
    }

    public struct LevelDesc
    {
        [JsonProperty("columnCount")]
        public int ColumnCount;
        [JsonProperty("rowCount")]
        public int RowCount;
        [JsonProperty("levelBlocksSequence")]
        public int[] LevelBlocksSequence;
    }
}
