using System.Collections.Generic;
using Entitas;

namespace Project.Entitas
{
    public sealed class DestroyCellsSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _gameContext;

        public DestroyCellsSystem(Contexts contexts) : base(contexts.game)
        {
            _gameContext = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> contexts)
            => contexts.CreateCollector(GameMatcher.FallingFinished.Added());

        protected override bool Filter(GameEntity entity) => entity.hasFallingFinished;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                // TODO: обработка сущностей
            }
        }
    }
}