namespace Project.Gameplay.Puzzles
{
    public class SwitchCellsStep : MergesStep
    {
        public override bool MakeSense => true;
        public MoveData MoveData { get; private set; }

        private SwitchCellsStep(MergesState initial) : base(initial)
        {
        }

        public static SwitchCellsStep CalculateStep(MergesState state, (int X, int Y) from, (int X, int Y) to)
        {
            var step = new SwitchCellsStep(state);

            step.Final[from] = step.Initial[to];
            step.Final[to] = step.Initial[from];

            step.MoveData = new MoveData(from, to);
            return step;
        }
    }
}