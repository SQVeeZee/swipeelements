using System.Collections.Generic;
using Entitas;
using Project.Gameplay;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class MoveRequestValidationSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _gameContext;

        public MoveRequestValidationSystem(Contexts contexts) : base(contexts.game)
            => _gameContext = contexts.game;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.MoveRequest.Added());

        protected override bool Filter(GameEntity gameEntity) => gameEntity.hasTileType && gameEntity.hasMoveRequest;

        protected override void Execute(List<GameEntity> requests)
        {
            foreach (var requestedTile in requests)
            {
                if (requestedTile is not { isInteractive: true, hasTileType: true })
                {
                    continue;
                }
                var moveData = requestedTile.moveRequest.moveData;
                requestedTile.AddMoveComponent(_gameContext, moveData);

                var toEntity = _gameContext.GetEntityWithTileCoord(moveData.To);
                if (toEntity is { isInteractive: true, hasTileType: true })
                {
                    toEntity.AddMoveComponent(_gameContext, moveData.Switch());
                }
            }
        }
    }
}