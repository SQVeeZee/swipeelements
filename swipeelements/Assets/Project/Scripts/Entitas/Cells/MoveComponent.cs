using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public sealed class MoveComponent : IComponent
    {
        public MoveData moveData;
    }
}