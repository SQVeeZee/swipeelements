using Entitas;
using Project.Gameplay.Puzzles;
using UnityEngine.Pool;

namespace Project.Entitas
{
    public sealed class PlanGravitySystem : IExecuteSystem
    {
        private readonly GameContext _gameContext;
        private readonly LevelContext _levelContext;
        private readonly IGroup<GameEntity> _dirty;

        public PlanGravitySystem(Contexts c)
        {
            _gameContext = c.game;
            _levelContext = c.level;
            _dirty = _gameContext.GetGroup(GameMatcher.ColumnDirty);
        }

        void IExecuteSystem.Execute()
        {
            if (_dirty.count == 0)
            {
                return;
            }

            var levelData = _levelContext.levelConfig.LevelData;
            var width  = levelData.Columns;
            var height = levelData.Rows;

            var xs = ListPool<int>.Get();
            foreach (var d in _dirty)
            {
                xs.Add(d.columnDirty.column);
            }

            foreach (var x in xs)
            {
                if (x < 0 || x >= width)
                {
                    continue;
                }

                var lowestFree = 0;

                for (var y = 1; y < height; y++)
                {
                    var from = new Coord(x, y);
                    var e = _gameContext.GetEntityWithTileCoord(from);
                    if (e.hasCellType && e.cellType.value.IsTile())
                    {
                        continue;
                    }

                    var targetY = FindLowestFreeRow(x, y, lowestFree);
                    if (targetY == y)
                    {
                        lowestFree = y + 1;
                        continue;
                    }

                    var to = new Coord(x, targetY);

                    e.ReplaceFallRequest(new MoveData(from, to));

                    lowestFree = targetY + 1;
                }
            }

            ListPool<int>.Release(xs);
        }

        private int FindLowestFreeRow(int x, int fromY, int lowestFree)
        {
            for (var check = fromY - 1; check >= lowestFree; check--)
            {
                if (_gameContext.GetEntityWithTileCoord(new Coord(x, check)) != null)
                {
                    return check + 1;
                }
            }
            return lowestFree;
        }
    }
}
