using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Project.Entitas
{
    [Game, Event(EventTarget.Self), Cleanup(CleanupMode.DestroyEntity)]
    public class DestroyTileComponent : IComponent { }
}