using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game]
    public sealed class CellCoordComponent : IComponent
    {
        [PrimaryEntityIndex]
        public Coord value;
    }
}