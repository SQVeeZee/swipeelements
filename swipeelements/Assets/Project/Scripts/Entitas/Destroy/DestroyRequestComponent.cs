using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game]
    public class DestroyRequestComponent : IComponent
    {
        public Coord coord;
    }
}