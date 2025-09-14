namespace Project.Gameplay.Puzzles
{
    public static class CellUtilities
    {
        public static bool IsVoid(this CellType cell) => cell == CellType.Void;
        public static bool IsEmpty(this CellType cell) => cell == CellType.Empty;
        public static bool IsRegular(this CellType cell) => cell != CellType.Empty && cell != CellType.Void && cell != CellType.None;
        public static bool IsTile(this CellType cell) => cell is CellType.Type1 or CellType.Type2 or CellType.AnyCell;

        public static bool IsInteractable(this CellType cellType) => cellType.IsTile();
        public static bool IsInteractable(this CellState cellState) => cellState == CellState.Idle;

        public static bool CanBeSwiped(this CellType cellType) => !cellType.IsVoid();
        public static bool CanBeSwiped(this CellState cellState) => cellState == CellState.None || cellState == CellState.Idle;

        public static bool IsDestroyable(this CellState cellState) => cellState == CellState.Idle;

        public static bool CanFalling(this CellType cellType) => cellType.IsTile();
        public static bool CanFalling(this CellState cellState) => cellState == CellState.Idle || cellState == CellState.Falling;

        public static bool IsFalling(this CellState cellState) => cellState == CellState.Falling;
        // public static bool IsReserved(this CellState cellState) => cellState == CellState.Reserved;

        public static MergesCell ChangeCell(this MergesCell cell, CellType type) => new(type);
        public static MergesCell ChangeCell(this MergesCell cell, CellState state) => new(cell.CellType, state);
        public static MergesCell ChangeCell(this MergesCell cell, CellType type, CellState state) => new(type, state);
    }
}