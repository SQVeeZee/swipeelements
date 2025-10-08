using System.Collections.Generic;
using System.Linq;
using Project.Entitas;

namespace Project.Gameplay.Puzzles
{
    public class DestroyCellsStep : MergesStep
    {
        public override bool MakeSense => DestroyedCells.Count > 0;
        public HashSet<Coord> DestroyedCells { get; } = new();

        private DestroyCellsStep(MergesState initial) : base(initial) { }

        public static DestroyCellsStep CalculateStep(MergesState state)
        {
            var step = new DestroyCellsStep(state);
            step.ApplyStep();
            return step;
        }

        private void ApplyStep()
        {
            var toDestroy = FindMatches();
            foreach (var coord in toDestroy)
            {
                Final[coord] = Final[coord].ChangeCell(CellType.Empty, CellState.Destroyed);
                DestroyedCells.Add(coord);
            }
        }

        private HashSet<Coord> FindMatches()
        {
            var visited = new HashSet<Coord>();
            var toDestroy = new HashSet<Coord>();

            foreach (var coord in Final.GetTileCoords())
            {
                if (visited.Contains(coord) || !Final[coord].IsDestroyable)
                {
                    continue;
                }

                var region = FloodFill(coord);

                foreach (var c in region)
                {
                    visited.Add(c);
                }

                if (!ContainsLine(region))
                {
                    continue;
                }

                foreach (var c in region)
                {
                    toDestroy.Add(c);
                }
            }

            return toDestroy;
        }

        private List<Coord> FloodFill(Coord start)
        {
            var result = new List<Coord>();
            var stack = new Stack<Coord>();
            var targetType = Final[start].CellType;

            stack.Push(start);

            while (stack.Count > 0)
            {
                var coord = stack.Pop();
                if (result.Contains(coord) || !Final[coord].IsDestroyable)
                {
                    continue;
                }

                if (!Final[coord].IsTile || Final[coord].CellType != targetType)
                {
                    continue;
                }

                result.Add(coord);

                foreach (var (nx, ny) in Neighbors(coord))
                {
                    if (nx >= 0 && nx < Final.Columns &&
                        ny >= 0 && ny < Final.Rows &&
                        !result.Contains(new Coord(nx, ny)) &&
                        Final[new Coord(nx, ny)].IsDestroyable)
                    {
                        stack.Push(new Coord(nx, ny));
                    }
                }
            }

            return result;
        }


        private IEnumerable<Coord> Neighbors(Coord coord)
        {
            yield return new Coord(coord.X + 1, coord.Y);
            yield return new Coord(coord.X - 1, coord.Y);
            yield return new Coord(coord.X, coord.Y + 1);
            yield return new Coord(coord.X, coord.Y - 1);
        }

        private bool ContainsLine(List<Coord> region)
        {
            var groupedByRow = region.GroupBy(c => c.Y);
            foreach (var row in groupedByRow)
            {
                int count = 0, lastX = int.MinValue;
                foreach (var (x, _) in row.OrderBy(c => c.X))
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

            var groupedByCol = region.GroupBy(c => c.X);
            foreach (var col in groupedByCol)
            {
                int count = 0, lastY = int.MinValue;
                foreach (var (_, y) in col.OrderBy(c => c.Y))
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
