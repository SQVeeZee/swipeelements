using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public sealed class SpawnComponent : IComponent
    {
        public CellType cellType;
    }
}