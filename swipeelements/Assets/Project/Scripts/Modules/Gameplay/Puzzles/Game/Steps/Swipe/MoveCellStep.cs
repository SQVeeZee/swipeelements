namespace Project.Gameplay.Puzzles
{
    public class MoveCellStep : MergesStep
    {
        public bool IsApply { get; set; } = false;
        public override bool MakeSense => IsApply;

        public MoveData MoveData { get; private set; }

        private MoveCellStep(MergesState initial) : base(initial)
        {

        }

        public static MoveCellStep CalculateStep(MergesState state, (int X, int Y) from, (int X, int Y) to)
        {
            var step = new MoveCellStep(state);
            step.ApplyMoveStep(from,to);
            return step;
        }

        private void ApplyMoveStep((int X, int Y) from, (int X, int Y) to)
        {
            Final[to] = Initial[from].ChangeCell(CellState.Moving);
            Final[from] = Initial[from].ChangeCell(CellType.Empty, CellState.Moving);

            MoveData = new MoveData(from, to);
            IsApply = true;
        }
    }
}