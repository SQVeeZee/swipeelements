using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game, Cleanup(CleanupMode.RemoveComponent)]
    public sealed class FallingRequestComponent : IComponent
    {
        public MoveData moveData;
    }
}