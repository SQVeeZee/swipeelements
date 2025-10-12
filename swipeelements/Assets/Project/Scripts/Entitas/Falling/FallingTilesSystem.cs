using Entitas;
using Project.Gameplay;
using UnityEngine;

namespace Project.Entitas
{
    public sealed class FallingTilesSystem : IInitializeSystem, IExecuteSystem
    {
        private readonly IGroup<GameEntity> _falling;
        private readonly GameContext _gameContext;

        private MoveSettings _settings;

        public FallingTilesSystem(Contexts contexts)
        {
            _gameContext = contexts.game;
            _falling = contexts.game.GetGroup(GameMatcher.Falling);
        }

        void IInitializeSystem.Initialize() => _settings = _gameContext.fallingConfig.moveConfig;

        void IExecuteSystem.Execute()
        {
            var entities = _falling.GetEntities();
            for (var i = 0; i < entities.Length; i++)
            {
                var gameEntity = entities[i];
                MoveTile(gameEntity, gameEntity.falling);
            }
        }

        private void MoveTile(GameEntity gameEntity, FallingComponent fallingComponent)
        {
            var deltaTime = Time.deltaTime;
            if (deltaTime <= 0f)
            {
                return;
            }

            var pos = gameEntity.tilePosition.value;
            var toVec = fallingComponent.end - pos;
            var dist = toVec.magnitude;

            var speed = fallingComponent.speed;
            speed += _settings.Acceleration * deltaTime;
            if (speed > _settings.MaxSpeed)
            {
                speed = _settings.MaxSpeed;
            }

            var stepLen = speed * deltaTime;

            var reached = dist <= 1e-4f || stepLen >= dist;
            if (!reached)
            {
                var dir = toVec / dist;
                var targetPosition = pos + dir * stepLen;
                gameEntity.ReplaceTilePosition(targetPosition);
                gameEntity.ReplaceFalling(fallingComponent.moveData, fallingComponent.start, fallingComponent.end, speed, fallingComponent.elapsed + deltaTime);

            }
            else
            {
                gameEntity.ReplaceTilePosition(fallingComponent.end);
                gameEntity.RemoveFallingComponent(fallingComponent.moveData);
            }
        }
    }
}
