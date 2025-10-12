using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public sealed class TileTypeComponent : IComponent
    {
        [EntityIndex]
        public CellType value;
    }
}