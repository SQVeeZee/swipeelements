using Entitas;
using UnityEngine;

namespace Project.Entitas
{
    [Game]
    public sealed class CellPositionComponent : IComponent
    {
        public Vector3 value;
    }
}