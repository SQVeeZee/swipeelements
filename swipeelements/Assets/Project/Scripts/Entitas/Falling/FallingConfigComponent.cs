using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay;

namespace Project.Entitas
{
    [Game, Unique]
    public sealed class FallingConfigComponent : IComponent
    {
        public MoveSettings moveConfig;
    }
}