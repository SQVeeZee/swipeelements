using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class FallTilesSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _g;

        public FallTilesSystem(Contexts c) : base(c.game)
        {
            _g = c.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.FallRequest.Added());

        protected override bool Filter(GameEntity e) => e.hasFallRequest;

        protected override void Execute(List<GameEntity> ents)
        {
            foreach (var e in ents)
            {
                // var move = e.fallRequest.moveData;
                //
                // // лок кошельков: можно как в MoveTilesSystem
                // var fromE = _g.GetEntityWithCoord(move.From);
                // fromE.isInteractive = false;
                //
                // _moving.FallTileAsync(new FallingData(move, BuildPath(move)), CancellationToken.None)
                //     .ContinueWith(_ => {
                //         // по завершении: вернуть интерактив и кинуть MoveFinishedEvent
                //         var arrived = _g.GetEntityWithCoord(move.To);
                //         if (arrived != null) arrived.isInteractive = true;
                //         _g.CreateEntity().AddMoveFinishedEvent(move);
                //     }).Forget();
            }
        }

        private HashSet<Coord> BuildPath(MoveData move)
        {
            var set = new HashSet<Coord>();
            for (var y = move.To.Y; y < move.From.Y; y++)
                set.Add(new Coord(move.From.X, y));
            return set;
        }
    }
}