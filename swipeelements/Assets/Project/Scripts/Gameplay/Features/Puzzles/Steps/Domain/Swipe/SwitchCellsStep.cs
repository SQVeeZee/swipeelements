using Project.Entitas;

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

        public static SwitchCellsStep CalculateStep(MergesState state, Coord from, Coord to)
        {
            var step = new SwitchCellsStep(state);
            step.ApplySwitchStep(from, to);
            return step;
        }

        private void ApplySwitchStep(Coord from, Coord to)
        {
            Final[from] = Initial[to].ChangeCell(CellState.Moving);
            Final[to] = Initial[from].ChangeCell(CellState.Moving);
            MoveData = new MoveData(from, to);
            IsApply = true;
        }
    }
}