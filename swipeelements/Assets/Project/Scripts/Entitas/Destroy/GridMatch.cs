using System.Collections.Generic;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    internal static class GridMatch
    {
        public static bool[,] MarkCellsInLines(GameEntity[,] grid, int cols, int rows)
        {
            var mark = new bool[cols, rows];

            for (var y = 0; y < rows; y++)
            {
                var runStart = 0;
                GameEntity prev = null;

                for (var x = 0; x <= cols; x++)
                {
                    var cur = x < cols ? grid[x, y] : null;

                    if (x < cols && IsMatchable(cur) && SameType(prev, cur))
                    {

                    }
                    else
                    {
                        var len = x - runStart;
                        if (len >= 3 && IsMatchable(prev))
                        {
                            for (var k = runStart; k < x; k++)
                            {
                                mark[k, y] = true;
                            }
                        }

                        runStart = x;
                    }

                    prev = IsMatchable(cur) ? cur : null;
                }
            }

            for (var x = 0; x < cols; x++)
            {
                var runStart = 0;
                GameEntity prev = null;

                for (var y = 0; y <= rows; y++)
                {
                    var cur = y < rows ? grid[x, y] : null;

                    if (y < rows && IsMatchable(cur) && SameType(prev, cur))
                    {

                    }
                    else
                    {
                        var len = y - runStart;
                        if (len >= 3 && IsMatchable(prev))
                        {
                            for (var k = runStart; k < y; k++)
                            {
                                mark[x, k] = true;
                            }
                        }

                        runStart = y;
                    }

                    prev = IsMatchable(cur) ? cur : null;
                }
            }

            return mark;
        }

        public static List<Coord> CollectRegionsToDestroy(GameEntity[,] grid, bool[,] inLine, int cols, int rows)
        {
            var visited = new bool[cols, rows];
            var result = new List<Coord>(64);

            for (var x = 0; x < cols; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    if (visited[x, y] || !IsMatchable(grid[x, y]))
                    {
                        continue;
                    }

                    var type = grid[x, y].cellType.value;
                    var region = new List<Coord>(16);
                    var touches = false;

                    FloodFillRegion(grid, inLine, visited, cols, rows, x, y, type, region, ref touches);

                    if (!touches)
                    {
                        continue;
                    }

                    for (var i = 0; i < region.Count; i++)
                    {
                        result.Add(region[i]);
                    }
                }
            }

            return result;
        }

        private static void FloodFillRegion(
            GameEntity[,] grid,
            bool[,] inLine,
            bool[,] visited,
            int cols,
            int rows,
            int sx,
            int sy,
            CellType type,
            List<Coord> region,
            ref bool touches)
        {
            var stack = new Stack<Coord>();
            stack.Push(new Coord(sx, sy));

            while (stack.Count > 0)
            {
                var c = stack.Pop();
                var x = c.X;
                var y = c.Y;

                if ((uint)x >= (uint)cols || (uint)y >= (uint)rows || visited[x, y])
                {
                    continue;
                }

                visited[x, y] = true;

                var e = grid[x, y];
                if (!IsMatchable(e) || e.cellType.value != type)
                {
                    continue;
                }

                region.Add(c);
                if (inLine[x, y])
                {
                    touches = true;
                }

                stack.Push(new Coord(x + 1, y));
                stack.Push(new Coord(x - 1, y));
                stack.Push(new Coord(x, y + 1));
                stack.Push(new Coord(x, y - 1));
            }
        }

        private static bool IsMatchable(GameEntity e)
            => e != null && e.IsDestroyable();

        private static bool SameType(GameEntity a, GameEntity b)
            => a != null && b != null && a.cellType.value == b.cellType.value;
    }
}