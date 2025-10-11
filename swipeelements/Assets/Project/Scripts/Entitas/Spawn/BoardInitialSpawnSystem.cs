using System.Collections.Generic;
using Entitas;
using JetBrains.Annotations;
using Project.Gameplay;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [UsedImplicitly]
    public sealed class BoardInitialSpawnSystem : ReactiveSystem<GameEntity>
    {
        private readonly CellsContainer _cells;
        private readonly GameContext _gameContexts;

        public BoardInitialSpawnSystem(Contexts contexts, CellsContainer cells) : base(contexts.game)
        {
            _cells = cells;
            _gameContexts = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.Spawn.Added());

        protected override bool Filter(GameEntity gameEntity) => gameEntity.hasSpawn;

        protected override void Execute(List<GameEntity> list)
        {
            foreach (var gameEntity in list)
            {
                var spawnComponent = gameEntity.spawn;
                var mergesCell = new MergesCell(spawnComponent.cellType);
                var view = (TileCellObject)_cells.Spawn(mergesCell, spawnComponent);
                var tileEntity = _gameContexts.GetEntityWithTileCoord(spawnComponent.coord);
                view.Link(tileEntity);
            }
        }
    }
}