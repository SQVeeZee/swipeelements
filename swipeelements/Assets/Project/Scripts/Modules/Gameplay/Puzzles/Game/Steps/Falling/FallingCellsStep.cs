using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public class FallingCellsStep : MergesStep
    {
        public override bool MakeSense => NewFallingMoves.Count > 0 || ContinuedFallingMoves.Count > 0;

        public List<FallingData> NewFallingMoves { get; } = new();
        public List<FallingData> ContinuedFallingMoves { get; } = new();

        public bool AnyNewFalling => NewFallingMoves.Count > 0;

        private FallingCellsStep(MergesState initial) : base(initial) { }

        public static FallingCellsStep CalculateStep(MergesState state)
        {
            var step = new FallingCellsStep(state);
            step.ApplyGravity();
            return step;
        }

        private void ApplyGravity()
        {
            for (var column = 0; column < Final.Columns; column++)
            {
                var lowestFreeRow = 0;

                for (var row = 1; row < Final.Rows; row++)
                {
                    var fromCoord = (column, row);
                    var cell = Final[fromCoord];

                    if (cell.IsEmpty)
                    {
                        continue;
                    }

                    lowestFreeRow = FindLowestFreeRow(column, row, lowestFreeRow);

                    if (row == lowestFreeRow)
                    {
                        lowestFreeRow++;
                        continue;
                    }

                    var targetCoord = (column, lowestFreeRow);
                    var fallPath = BuildFallPath(column, targetCoord.lowestFreeRow, row);

                    var move = new MoveData(fromCoord, targetCoord);
                    var fallMove = new FallingData(move, fallPath);

                    if (cell.CellType.IsTile() && cell.CellState == CellState.Falling)
                    {
                        ContinuedFallingMoves.Add(fallMove);
                    }
                    else if (cell.CellType.IsTile() && cell.CellState == CellState.Idle)
                    {
                        NewFallingMoves.Add(fallMove);
                    }

                    Final[targetCoord] = cell.ChangeCell(cell.CellType, CellState.Falling);
                    Final[fromCoord] = cell.ChangeCell(CellType.Empty, CellState.Falling);

                    lowestFreeRow++;
                }
            }
        }

        private int FindLowestFreeRow(int column, int fromRow, int lowestFreeRow)
        {
            for (var checkRow = fromRow - 1; checkRow >= lowestFreeRow; checkRow--)
            {
                if (!Final[column, checkRow].CanFallInto)
                {
                    return checkRow + 1;
                }
            }
            return lowestFreeRow;
        }

        private HashSet<(int X, int Y)> BuildFallPath(int column, int startRow, int endRow)
        {
            var path = new HashSet<(int X, int Y)>();
            for (var row = startRow; row < endRow; row++)
            {
                path.Add((column, row));
                Final[(column, row)] = Final[(column, row)].ChangeCell(CellState.Falling);
            }
            return path;
        }
    }
}
