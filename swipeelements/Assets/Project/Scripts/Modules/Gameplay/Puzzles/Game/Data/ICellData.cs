using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    public interface ICellData
    {
        [JsonProperty("cell_type")]
        public CellType CellType { get; set; }
    }
}