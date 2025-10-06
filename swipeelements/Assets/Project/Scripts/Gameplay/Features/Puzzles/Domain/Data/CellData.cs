using System;

namespace Project.Gameplay.Puzzles
{
    [Serializable]
    public struct CellData : ICellData
    {
        public CellType CellType { get; set; }
    }
}