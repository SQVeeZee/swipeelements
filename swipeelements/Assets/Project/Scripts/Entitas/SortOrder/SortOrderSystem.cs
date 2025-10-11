using System;
using System.Collections.Generic;
using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas.SortOrder
{
    public sealed class SortOrderSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly Contexts _context;

        private Dictionary<Coord, int> _sortOrders;

        public SortOrderSystem(Contexts contexts) : base(contexts.game)
            => _context = contexts;

        void IInitializeSystem.Initialize()
        {
            var levelData = _context.level.levelConfig.LevelData;
            _sortOrders = new Dictionary<Coord, int>();

            PrecomputeSortOrders(levelData);
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> contexts)
            => contexts.CreateCollector( GameMatcher.TileCoord.Added());

        protected override bool Filter(GameEntity entity) => entity.hasTileCoord;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                ApplyCellSortOrder(gameEntity, gameEntity.tileCoord.value);
            }
        }

        private void ApplyCellSortOrder(GameEntity gameEntity, Coord coord)
        {
            if (!_sortOrders.TryGetValue(coord, out var baseOrder))
            {
                throw new ArgumentException($"Sort order for coord {coord} not found");
            }
            var sortOrder = baseOrder + GetSortOrderByType(gameEntity);
            gameEntity.ReplaceTileSortOrder(sortOrder);
        }

        private void PrecomputeSortOrders(LevelData levelData)
        {
            for (var y = 0; y < levelData.Rows; y++)
            {
                for (var x = 0; x < levelData.Columns; x++)
                {
                    var coord = new Coord(x, y);
                    var sortOrder = GetSortOrderByCoord(levelData, coord);
                    _sortOrders[coord] = sortOrder;
                }
            }
        }

        private static int GetSortOrderByCoord(LevelData levelData, Coord coord) =>
            coord.Y * levelData.Columns + coord.X;

        private static int GetSortOrderByType(GameEntity e) => e switch
        {
            { isTileTag: true } => 0,
            { isCellTag: true } => 0,
            _ => 0
        };
    }
}