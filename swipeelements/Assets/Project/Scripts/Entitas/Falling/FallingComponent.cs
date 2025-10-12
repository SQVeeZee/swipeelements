using Entitas;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Entitas
{
    [Game]
    public sealed class FallingComponent : IComponent
    {
        public MoveData moveData;
        public Vector3 start;
        public Vector3 end;
        public float speed;
        public float elapsed;
    }
}