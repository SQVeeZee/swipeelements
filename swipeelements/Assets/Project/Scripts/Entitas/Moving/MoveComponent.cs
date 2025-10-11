using Entitas;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Entitas
{
    [Game]
    public sealed class MoveComponent : IComponent
    {
        public MoveData move;
        public Vector3 start;
        public Vector3 end;
        public float elapsed;
        public float duration;
        public AnimationCurve curve;
    }
}