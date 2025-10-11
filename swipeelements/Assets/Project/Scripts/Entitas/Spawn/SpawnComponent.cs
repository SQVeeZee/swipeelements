using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Entitas
{
    [Game, Cleanup(CleanupMode.RemoveComponent)]
    public sealed class SpawnComponent : IComponent
    {
        public CellType cellType;
        public Coord coord;
        public Vector3 position;
    }
}