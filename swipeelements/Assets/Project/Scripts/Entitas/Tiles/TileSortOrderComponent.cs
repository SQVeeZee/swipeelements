using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game, Event(EventTarget.Self)]
    public sealed class TileSortOrderComponent : IComponent
    {
        public int value;
    }
}