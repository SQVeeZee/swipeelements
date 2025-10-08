using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game, Cleanup(CleanupMode.DestroyEntity)]
    public sealed class SwitchRequestComponent : IComponent
    {
        public MoveData moveData;
    }

    [Game, Cleanup(CleanupMode.DestroyEntity)]
    public sealed class MoveRequestComponent : IComponent
    {
        public MoveData moveData;
    }

    [Game, Cleanup(CleanupMode.DestroyEntity)]
    public sealed class MoveValidateComponent : IComponent
    {
        public MoveData moveData;
    }
}