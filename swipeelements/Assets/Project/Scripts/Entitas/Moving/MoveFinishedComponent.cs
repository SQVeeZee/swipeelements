using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game, Cleanup(CleanupMode.RemoveComponent)]
    public sealed class MoveFinishedComponent : IComponent
    {
        public MoveData value;
    }
}