using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game]
    public sealed class TileCoordComponent : IComponent
    {
        [PrimaryEntityIndex]
        public Coord value;
    }
}