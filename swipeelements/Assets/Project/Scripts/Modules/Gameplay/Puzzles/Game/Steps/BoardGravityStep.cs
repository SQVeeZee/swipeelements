using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public class BoardGravityStep : MergesStep
    {
        public override bool MakeSense => FallingMoves.Count > 0;

        public List<MoveData> FallingMoves { get; } = new();

        private BoardGravityStep(MergesState initial) : base(initial) { }

        public static BoardGravityStep CalculateStep(MergesState state)
        {
            var step = new BoardGravityStep(state);
            step.ApplyGravity();
            return step;
        }

        private void ApplyGravity()
        {
            for (var x = 0; x < Final.Columns; x++)
            {
                for (var y = 1; y < Final.Rows; y++)
                {
                    var coord = (x, y);
                    var cell = Final[coord];

                    if (!cell.IsTile)
                    {
                        continue;
                    }

                    var targetY = y;
                    while (targetY > 0 && Final[(x, targetY).Bottom()].IsEmpty)
                    {
                        targetY--;
                    }

                    if (targetY == y)
                    {
                        continue;
                    }
                    var toCoord = (x, targetY);

                    Final[toCoord] = cell;
                    Final[coord] = MergesCell.Empty;

                    FallingMoves.Add(new MoveData(coord, toCoord));
                }
            }
        }
    }
}