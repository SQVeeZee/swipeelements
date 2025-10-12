using Entitas;
using Entitas.CodeGeneration.Attributes;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [Game]
    public class CellTypeComponent : IComponent
    {
        [EntityIndex]
        public CellType value;
    }
}