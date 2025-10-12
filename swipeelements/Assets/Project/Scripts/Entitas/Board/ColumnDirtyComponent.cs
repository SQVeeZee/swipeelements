using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game]
    public sealed class ColumnDirtyComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int value;
    }
}