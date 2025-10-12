using System;
using System.Collections.Generic;
using System.Reflection;

namespace Project.Gameplay.Puzzles
{
    public static class CellUtilities
    {
        private static readonly HashSet<CellType> _tileSet = BuildTileSet();
        private static readonly CellType[] _tileArray = BuildTileArray(_tileSet);

        public static ReadOnlySpan<CellType> GetTilesTypes() => _tileArray;

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

        public static MergesCell ChangeCell(this MergesCell cell, CellState state) => new(cell.CellType, state);
        public static MergesCell ChangeCell(this MergesCell _, CellType type, CellState state) => new(type, state);

        private static CellType[] BuildTileArray(HashSet<CellType> set)
        {
            var arr = new CellType[set.Count];
            var i = 0;
            foreach (var t in set)
            {
                arr[i++] = t;
            }
            return arr;
        }

        private static HashSet<CellType> BuildTileSet()
        {
            var set = new HashSet<CellType>();

            var values = (CellType[])Enum.GetValues(typeof(CellType));
            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];
                var fi = typeof(CellType).GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
                if (fi == null)
                {
                    continue;
                }

                var hasAttr = fi.GetCustomAttribute<TileTypeAttribute>() != null;
                if (hasAttr)
                {
                    set.Add(value);
                }
            }

            return set;
        }
    }
}