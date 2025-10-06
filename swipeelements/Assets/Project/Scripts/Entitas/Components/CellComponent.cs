using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public sealed class CellComponent : IComponent
    {
        public CellType CellType;
        public (int X, int Y) Coord;
    }

    [Game]
    public sealed class TileComponent : IComponent
    {
        public CellState CellState;
    }
}