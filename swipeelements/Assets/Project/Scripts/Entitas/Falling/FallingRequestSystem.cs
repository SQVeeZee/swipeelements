using System.Collections.Generic;
using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class FallingRequestSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _gameContext;
        private readonly LevelContext _levelContext;

        public FallingRequestSystem(Contexts contexts) : base(contexts.game)
        {
            _gameContext = contexts.game;
            _levelContext = contexts.level;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> contexts)
            => contexts.CreateCollector(GameMatcher.ColumnDirty);

        protected override bool Filter(GameEntity entity) => entity.hasColumnDirty;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                var rows = _levelContext.levelConfig.LevelData.Rows;
                var col = entity.columnDirty.column;
                var targetRow = 0;

                for (var y = 0; y < rows; y++)
                {
                    var coord = new Coord(col, y);

                    if (!_gameContext.TryGetEntityWithTileCoord(coord, out var tile))
                    {
                        continue;
                    }

                    if (y != targetRow)
                    {
                        var to = new Coord(col, targetRow);
                        var moveData = new MoveData(coord, to);
                        if (tile.hasFalling)
                        {
                            tile.ReplaceFallingComponent(_gameContext, moveData);
                        }
                        else
                        {
                            tile.AddFallingComponent(_gameContext, moveData);
                        }
                    }

                    targetRow++;
                }
            }
        }
    }
}
