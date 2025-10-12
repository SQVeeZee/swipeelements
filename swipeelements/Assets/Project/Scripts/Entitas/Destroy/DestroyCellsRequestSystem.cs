using System.Collections.Generic;
using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class DestroyCellsRequestSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _gameContext;

        public DestroyCellsRequestSystem(Contexts contexts) : base(contexts.game) => _gameContext = contexts.game;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.FallingFinished.Added());

        protected override bool Filter(GameEntity e) => e.hasFallingFinished;

        protected override void Execute(List<GameEntity> events)
        {
            var typesToCheck = CellUtilities.GetTilesTypes();

            foreach (var type in typesToCheck)
            {
                var tiles = _gameContext.GetEntitiesWithTileType(type);
                TypeLineMatcher.BuildIndex(tiles, out var coords, out var byCoord);

                if (coords.Count == 0)
                {
                    continue;
                }

                var matches = TypeLineMatcher.FindLineMatches(coords);
                if (matches.Count == 0)
                {
                    continue;
                }

                foreach (var coord in matches)
                {
                    if (!byCoord.TryGetValue(coord, out var gameEntity) || gameEntity == null)
                    {
                        continue;
                    }

                    gameEntity.AddDestroyTileRequest(coord);
                }
            }
        }
    }
}
