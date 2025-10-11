using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Project.Entitas
{
    [Game, Event(EventTarget.Self)]
    public sealed class TilePositionComponent : IComponent
    {
        [PrimaryEntityIndex]
        public Vector3 value;
    }
}