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

        public BoardInitialSpawnSystem(Contexts contexts, CellsContainer cells) : base(contexts.game)
            => _cells = cells;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.Spawn.Added());

        protected override bool Filter(GameEntity gameEntity)
            => gameEntity.hasSpawn && gameEntity.hasCoord;

        protected override void Execute(List<GameEntity> list)
        {
            foreach (var e in list)
            {
                var mergesCell = new MergesCell(e.spawn.cellType);
                var view = _cells.Spawn(mergesCell, e.coord.value);
                view.Link(e);
            }
        }
    }
}