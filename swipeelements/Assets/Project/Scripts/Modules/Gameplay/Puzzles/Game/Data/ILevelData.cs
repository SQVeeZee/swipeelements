using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    public interface ILevelData
    {
        [JsonProperty("columns")]
        public int Columns { get; set; }

        [JsonProperty("rows")]
        public int Rows { get; set; }

        [JsonProperty(PropertyName = "initial")]
        public InitialGridData InitialValues { get; set; }
    }
}