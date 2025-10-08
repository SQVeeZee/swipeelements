using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game]
    public sealed class CoordComponent : IComponent
    {
        [PrimaryEntityIndex]
        public Coord value;
    }
}