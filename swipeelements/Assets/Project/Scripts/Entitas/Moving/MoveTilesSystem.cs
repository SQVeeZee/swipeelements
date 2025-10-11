using Entitas;
using UnityEngine;

namespace Project.Entitas
{
    public sealed class MoveTilesSystem : IExecuteSystem
    {
        private const float PositionEpsilon = 1e-4f;
        private readonly IGroup<GameEntity> _moveEntities;

        public MoveTilesSystem(Contexts contexts) => _moveEntities = contexts.game.GetGroup(GameMatcher.Move);

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _moveEntities.GetEntities())
            {
                MoveTile(entity, entity.move);
            }
        }

        private static void MoveTile(GameEntity gameEntity, MoveComponent moveComponent)
        {
            moveComponent.elapsed += Time.deltaTime;

            var t = moveComponent.duration > Mathf.Epsilon
                ? Mathf.Clamp01(moveComponent.elapsed / moveComponent.duration)
                : 1f;

            var easedT = moveComponent.curve?.Evaluate(t) ?? t;

            var position = Vector3.LerpUnclamped(moveComponent.start, moveComponent.end, easedT);
            gameEntity.ReplaceTilePosition(position);

            if (t >= 1f - PositionEpsilon)
            {
                gameEntity.RemoveMoveComponent(gameEntity.move.move);
            }
        }
    }
}