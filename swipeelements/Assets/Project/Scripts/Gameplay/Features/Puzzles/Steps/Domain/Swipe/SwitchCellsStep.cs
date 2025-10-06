namespace Project.Gameplay.Puzzles
{
    public class SwitchCellsStep : MergesStep
    {
        public bool IsApply { get; set; } = false;
        public override bool MakeSense => IsApply;
        public MoveData MoveData { get; private set; }

        private SwitchCellsStep(MergesState initial) : base(initial)
        {
        }

        public static SwitchCellsStep CalculateStep(MergesState state, (int X, int Y) from, (int X, int Y) to)
        {
            var step = new SwitchCellsStep(state);
            step.ApplySwitchStep(from, to);
            return step;
        }

        private void ApplySwitchStep((int X, int Y) from, (int X, int Y) to)
        {
            Final[from] = Initial[to].ChangeCell(CellState.Moving);
            Final[to] = Initial[from].ChangeCell(CellState.Moving);
            MoveData = new MoveData(from, to);
            IsApply = true;
        }
    }
}