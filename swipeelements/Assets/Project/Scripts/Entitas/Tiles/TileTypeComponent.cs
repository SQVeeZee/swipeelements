using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public sealed class TileTypeComponent : IComponent
    {
        public CellType type;
    }
}