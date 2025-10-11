using System.Collections.Generic;
using Entitas;

namespace Project.Entitas
{
    public sealed class BoardUpdateSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _gameContext;

        public BoardUpdateSystem(Contexts contexts) : base(contexts.game)
            => _gameContext = contexts.game;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.MoveFinished.Added());

        protected override bool Filter(GameEntity e) => e.hasMoveFinished;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                var moveData = entity.moveFinished.value;
                var fromX = moveData.From.X;
                var toX = moveData.To.X;
                entity.ReplaceTileCoord(moveData.To);

                if (_gameContext.GetEntityWithColumnDirty(fromX) != null)
                {
                    continue;
                }

                _gameContext.CreateEntity().AddColumnDirty(fromX);
                if (toX != fromX)
                {
                    _gameContext.CreateEntity().AddColumnDirty(toX);
                }
            }
        }
    }
}