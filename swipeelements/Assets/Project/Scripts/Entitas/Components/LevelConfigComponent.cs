using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Level, Unique]
    public sealed class LevelConfigComponent : IComponent
    {
        public LevelData LevelData;
    }
}