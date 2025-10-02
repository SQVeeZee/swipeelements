using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Context("Game")] // до первого прогона; потом можно [Game]
public sealed class PositionComponent : IComponent { public Vector2 value; }