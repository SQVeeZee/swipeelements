using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Project.Gameplay;

namespace Project.Entitas
{
    [Input, Cleanup(CleanupMode.DestroyEntity)]
    public sealed class PointerDownComponent : IComponent
    {
        public Vector2 ScreenPos;
    }

    [Input, Cleanup(CleanupMode.DestroyEntity)]
    public sealed class SwipeEventComponent : IComponent
    {
        public Coord from;
        public SwipeDirection dir;
    }
}