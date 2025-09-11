using System.Collections.Generic;

namespace Project.Gameplay.Puzzles
{
    public class MoveCellStep : MergesStep, ILockedStep
    {
        public override bool MakeSense => true;

        public MoveData MoveData { get; private set; }
        public HashSet<(int X, int Y)> LockedCoords { get; } = new();

        private MoveCellStep(MergesState initial) : base(initial)
        {

        }

        public static MoveCellStep CalculateStep(MergesState state, (int X, int Y) from, (int X, int Y) to)
        {
            var step = new MoveCellStep(state)
            {
                MoveData = new MoveData(from, to),
            };
            step.Final[to] = step.Initial[from];
            step.Final[from] = step.Initial[from].ChangeCell(CellType.Empty);

            step.LockedCoords.Add(to);
            step.LockedCoords.Add(from);

            return step;
        }

    }
}