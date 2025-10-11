using System.Collections.Generic;
using Entitas;
using JetBrains.Annotations;
using Project.Gameplay;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [UsedImplicitly]
    public sealed class ApplyInputSystem : ReactiveSystem<InputEntity>
    {
        private readonly GameContext _gameContext;

        public ApplyInputSystem(Contexts contexts)
            : base(contexts.input) => _gameContext = contexts.game;

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
            => context.CreateCollector(InputMatcher.SwipeEvent.Added());

        protected override bool Filter(InputEntity inputEntity) => inputEntity.hasSwipeEvent;

        protected override void Execute(List<InputEntity> list)
        {
            foreach (var inputEntity in list)
            {
                var tileEntity = inputEntity.swipeEvent.tile.Entity;
                var tileCoord = tileEntity.tileCoord.value;
                var to = GetSwipeDirection(tileCoord, inputEntity.swipeEvent.dir);
                tileEntity.AddMoveRequest(new MoveData(tileCoord, to));
            }
        }

        private static Coord GetSwipeDirection(Coord from, SwipeDirection direction) =>
            direction switch
            {
                SwipeDirection.Up => from.Top(),
                SwipeDirection.Down => from.Bottom(),
                SwipeDirection.Left => from.Left(),
                SwipeDirection.Right => from.Right(),
                _ => from
            };
    }
}