using System.Collections.Generic;
using System.Linq;

namespace Project.Gameplay.Puzzles
{
    public class BoardDestroyStep : MergesStep, ILockedStep
    {
        public override bool MakeSense => DestroyedCells.Count > 0;
        public HashSet<(int X, int Y)> DestroyedCells { get; } = new();
        public HashSet<(int X, int Y)> LockedCoords { get; } = new();

        private BoardDestroyStep(MergesState initial) : base(initial) { }

        public static BoardDestroyStep CalculateStep(MergesState state, HashSet<(int X, int Y)> ignored)
        {
            var step = new BoardDestroyStep(state);
            var toDestroy = step.FindMatches(ignored);

            foreach (var coord in toDestroy)
            {
                step.Final[coord] = MergesCell.Empty;
                step.DestroyedCells.Add(coord);
                step.LockedCoords.Add(coord);
            }
            return step;
        }

        private HashSet<(int X, int Y)> FindMatches(HashSet<(int X, int Y)> ignored)
        {
            var visited = new HashSet<(int, int)>();
            var toDestroy = new HashSet<(int, int)>();

            foreach (var coord in Final.GetTileCoords())
            {
                if (visited.Contains(coord) || ignored.Contains(coord))
                    continue;

                var region = FloodFill(coord, ignored);

                foreach (var c in region)
                    visited.Add(c);

                if (!ContainsLine(region))
                    continue;

                foreach (var c in region)
                    toDestroy.Add(c);
            }

            return toDestroy;
        }

        private List<(int, int)> FloodFill((int X, int Y) start, HashSet<(int X, int Y)> ignored)
        {
            var result = new List<(int, int)>();
            var stack = new Stack<(int, int)>();
            var targetType = Final[start].CellType;

            stack.Push(start);

            while (stack.Count > 0)
            {
                var (x, y) = stack.Pop();
                if (result.Contains((x, y)) || ignored.Contains((x, y)))
                    continue;

                if (!Final[(x, y)].IsTile || Final[(x, y)].CellType != targetType)
                    continue;

                result.Add((x, y));

                foreach (var (nx, ny) in Neighbors(x, y))
                {
                    if (nx >= 0 && nx < Final.Columns &&
                        ny >= 0 && ny < Final.Rows &&
                        !result.Contains((nx, ny)) &&
                        !ignored.Contains((nx, ny)))
                    {
                        stack.Push((nx, ny));
                    }
                }
            }

            return result;
        }


        private IEnumerable<(int, int)> Neighbors(int x, int y)
        {
            yield return (x + 1, y);
            yield return (x - 1, y);
            yield return (x, y + 1);
            yield return (x, y - 1);
        }

        private bool ContainsLine(List<(int, int)> region)
        {
            var groupedByRow = region.GroupBy(c => c.Item2);
            foreach (var row in groupedByRow)
            {
                int count = 0, lastX = int.MinValue;
                foreach (var (x, _) in row.OrderBy(c => c.Item1))
                {
                    if (x == lastX + 1)
                    {
                        count++;
                    }
                    else
                    {
                        count = 1;
                    }
                    lastX = x;

                    if (count >= 3)
                    {
                        return true;
                    }
                }
            }

            var groupedByCol = region.GroupBy(c => c.Item1);
            foreach (var col in groupedByCol)
            {
                int count = 0, lastY = int.MinValue;
                foreach (var (_, y) in col.OrderBy(c => c.Item2))
                {
                    if (y == lastY + 1)
                    {
                        count++;
                    }
                    else
                    {
                        count = 1;
                    }
                    lastY = y;

                    if (count >= 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
