using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game, Cleanup(CleanupMode.RemoveComponent)]
    public sealed class ColumnDirtyComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int column;
    }
}