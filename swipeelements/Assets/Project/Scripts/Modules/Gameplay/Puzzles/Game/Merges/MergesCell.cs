using System;
using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    public struct MergesCell : IComparable<MergesCell>
    {
        public static MergesCell Empty => new(CellType.Empty);
        public static MergesCell Void => new(CellType.Void);
        public static MergesCell None => new(CellType.None);

        [JsonProperty]
        public CellType CellType { get; private set; }

        [JsonIgnore]
        public bool IsVoid => CellType.IsVoid();
        [JsonIgnore]
        public bool IsEmpty => CellType.IsEmpty();
        [JsonIgnore]
        public bool IsRegular => CellType.IsRegular();
        [JsonIgnore]
        public bool IsTile => CellType.IsTile();

        public MergesCell(CellType cellType) => CellType = cellType;
        public MergesCell(ICellData data) => CellType = data.CellType;

        public override int GetHashCode() => CellType.GetHashCode();
        public override bool Equals(object obj) => obj is MergesCell other && this == other;

        public static bool operator ==(MergesCell c1, MergesCell c2) => c1.CellType == c2.CellType;
        public static bool operator !=(MergesCell c1, MergesCell c2) => !(c1 == c2);

        public int CompareTo(MergesCell other) => CellType.CompareTo(other.CellType);

        public override string ToString() => CellType.ToString();
    }
}