using Unity.Plastic.Newtonsoft.Json;

namespace Assets.Scripts.Configs
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
        [JsonProperty("backgroundId")]
        public int BackgroundId;
    }
}
