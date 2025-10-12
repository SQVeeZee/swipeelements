using System.Collections.Generic;
using Entitas;

namespace Project.Entitas
{
    public sealed class DestroyCellsRequestSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _game;
        private readonly LevelContext _level;
        private readonly IGroup<GameEntity> _tiles;

        public DestroyCellsRequestSystem(Contexts contexts) : base(contexts.game)
        {
            _game = contexts.game;
            _level = contexts.level;
            _tiles = _game.GetGroup(GameMatcher.TileCoord);
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> c)
            => c.CreateCollector(GameMatcher.FallingFinished.Added());

        protected override bool Filter(GameEntity e) => e.hasFallingFinished;

        protected override void Execute(List<GameEntity> events)
        {
            var cols = _level.levelConfig.LevelData.Columns;
            var rows = _level.levelConfig.LevelData.Rows;

            var grid = BuildGridSnapshot(cols, rows);
            var inLine = GridMatch.MarkCellsInLines(grid, cols, rows);

            var toDestroy = GridMatch.CollectRegionsToDestroy(grid, inLine, cols, rows);

            if (toDestroy.Count > 0)
            {
                // MarkDirtyColumns(toDestroy);
                ApplyDestroyRequests(grid, toDestroy);
            }
        }

        private GameEntity[,] BuildGridSnapshot(int cols, int rows)
        {
            var grid = new GameEntity[cols, rows];
            var all = _tiles.GetEntities();

            for (var i = 0; i < all.Length; i++)
            {
                var e = all[i];
                var c = e.tileCoord.value;

                if ((uint)c.X < (uint)cols && (uint)c.Y < (uint)rows)
                {
                    grid[c.X, c.Y] = e;
                }
            }

            return grid;
        }

        private void ApplyDestroyRequests(GameEntity[,] grid, List<Coord> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var coord = cells[i];
                var gameEntity = grid[coord.X, coord.Y];
                gameEntity?.AddDestroyRequest(coord);
            }
        }

        private void MarkDirtyColumns(List<Coord> cells)
        {
            var marked = new HashSet<int>();
            for (var i = 0; i < cells.Count; i++)
            {
                var x = cells[i].X;
                if (marked.Add(x))
                {
                    _game.CreateEntity().AddColumnDirty(x);
                }
            }
        }
    }
}