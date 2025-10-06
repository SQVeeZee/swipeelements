using System;
using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    public partial struct MergesCell : IComparable<MergesCell>
    {
        public static MergesCell Empty => new(CellType.Empty, CellState.None);
        public static MergesCell Void => new(CellType.Void, CellState.None);
        public static MergesCell None => new(CellType.None, CellState.None);

        [JsonProperty]
        public CellType CellType { get; private set; }
        [JsonIgnore]
        public CellState CellState { get; }

        public MergesCell(CellType cellType)
        {
            CellType = cellType;
            CellState = CellState.None;
        }

        public MergesCell(CellType cellType, CellState cellState)
        {
            CellType = cellType;
            CellState = cellState;
        }

        public MergesCell(ICellData data, CellState cellState)
        {
            CellType = data.CellType;
            CellState = cellState;
        }

        public override int GetHashCode() => CellType.GetHashCode();
        public override bool Equals(object obj) => obj is MergesCell other && this == other;

        public static bool operator ==(MergesCell c1, MergesCell c2) => c1.CellType == c2.CellType;
        public static bool operator !=(MergesCell c1, MergesCell c2) => !(c1 == c2);

        public int CompareTo(MergesCell other) => CellType.CompareTo(other.CellType);

        public override string ToString() => $"{CellType} : {CellState}";
    }
}