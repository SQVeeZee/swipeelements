using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay;

namespace Project.Entitas
{
    [Input, Cleanup(CleanupMode.DestroyEntity)]
    public sealed class SwipeEventComponent : IComponent
    {
        public ITileView tile;
        public SwipeDirection dir;
    }
}