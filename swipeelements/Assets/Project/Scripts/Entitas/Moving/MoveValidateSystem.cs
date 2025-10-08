using System.Collections.Generic;
using Entitas;

namespace Project.Entitas
{
    public sealed class MoveValidateSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _gameContext;

        public MoveValidateSystem(Contexts contexts) : base(contexts.game)
        {
            _gameContext = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.MoveValidate.Added());

        protected override bool Filter(GameEntity gameEntity) => gameEntity.hasMoveValidate;

        protected override void Execute(List<GameEntity> requests)
        {
            foreach (var request in requests)
            {
                var moveData = request.moveValidate.moveData;
                var fromEntity = _gameContext.GetEntityWithCoord(moveData.From);
                if (fromEntity == null)
                {
                    request.Destroy();
                    continue;
                }
                var toEntity = _gameContext.GetEntityWithCoord(moveData.To);
                if (fromEntity.isTile && toEntity != null)
                {
                    if (toEntity.isTile)
                    {
                        fromEntity.AddSwitchRequest(moveData);
                    }
                    else
                    {
                        fromEntity.AddMoveRequest(moveData);
                    }
                }
                request.Destroy();
            }
        }
    }
}