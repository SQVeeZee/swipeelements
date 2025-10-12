using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game, Cleanup(CleanupMode.RemoveComponent)]
    public class DestroyTileRequestComponent : IComponent
    {
        public Coord coord;
    }
}