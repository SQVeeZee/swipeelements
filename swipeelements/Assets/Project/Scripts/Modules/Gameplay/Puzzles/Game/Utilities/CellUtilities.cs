namespace Project.Gameplay.Puzzles
{
    public static class CellUtilities
    {
        public static bool IsVoid(this CellType cell) => cell == CellType.Void;
        public static bool IsEmpty(this CellType cell) => cell == CellType.Empty;
        public static bool IsRegular(this CellType cell) => cell != CellType.Empty && cell != CellType.Void && cell != CellType.None;

        public static bool IsTile(this CellType cell) =>
            cell == CellType.Type1 ||
            cell == CellType.Type2 ||
            cell == CellType.AnyCell;

        public static MergesCell ChangeCell(this MergesCell cell, CellType type) => new(type);
    }
}