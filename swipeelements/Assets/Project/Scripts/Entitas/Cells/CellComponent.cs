using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public sealed class CellComponent : IComponent
    {
        public CellType cellType;
        public CellState cellState;
    }
}