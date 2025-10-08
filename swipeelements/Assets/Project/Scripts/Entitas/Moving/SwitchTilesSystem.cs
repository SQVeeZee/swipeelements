using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Entitas;
using JetBrains.Annotations;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [UsedImplicitly]
    public sealed class SwitchTilesSystem  : ReactiveSystem<GameEntity>
    {
        private readonly ICellsMovingSystem _cellsMovingSystem;
        private readonly IGroup<GameEntity> _moved;

        public SwitchTilesSystem(
            Contexts contexts,
            ICellsMovingSystem cellsMovingSystem) : base(contexts.game)
        {
            _cellsMovingSystem = cellsMovingSystem;
            _moved = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.SwitchRequest));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.SwitchRequest.Added());

        protected override bool Filter(GameEntity e) => e.hasSwitchRequest;

        protected override void Execute(List<GameEntity> ents)
        {
            foreach (var gameEntity in _moved)
            {
                _cellsMovingSystem.SwitchTilesAsync(gameEntity.switchRequest.moveData, new CancellationToken()).Forget();
            }
        }
    }
}