using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class MoveTilesSystem : ReactiveSystem<GameEntity>
    {
        private readonly ICellsMovingSystem _cellsMovingSystem;

        public MoveTilesSystem(
            Contexts contexts,
            ICellsMovingSystem cellsMovingSystem) : base(contexts.game)
            => _cellsMovingSystem = cellsMovingSystem;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.MoveRequest.Added());

        protected override bool Filter(GameEntity e) => e.hasMoveRequest;

        protected override void Execute(List<GameEntity> ents)
        {
            foreach (var gameEntity in ents)
            {
                var moveData = gameEntity.moveRequest.moveData;
                _cellsMovingSystem.MoveTileAsync(moveData, CancellationToken.None).Forget();
            }
        }
    }
}