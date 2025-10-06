// Assets/Scripts/ECS/Systems/ViewSyncSystem.cs

using Entitas;

namespace Project.Entitas
{
    public sealed class ViewSyncSystem : IExecuteSystem {
        readonly GameContext _g;
        readonly IGroup<GameEntity> _moved; // будем слушать изменения позиции
        readonly IBoardView _view;

        public ViewSyncSystem(Contexts c, IBoardView view) {
            _g = c.game;
            _view = view;
            // _moved = _g.GetGroup(GameMatcher.AllOf(GameMatcher.TileTag, GameMatcher.Position));
        }

        public void Execute() {
            // Минимально: каждый кадр телепортируем во вью позицию.
            // Позже заменишь на реагирующую систему (Reactive) + анимации.
            foreach (var e in _moved) {
                // var wp = new Vector2(e.position.x, e.position.y);
            }
        }
    }
}