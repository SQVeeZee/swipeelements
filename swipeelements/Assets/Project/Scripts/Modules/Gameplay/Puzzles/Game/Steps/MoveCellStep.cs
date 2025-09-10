namespace Project.Gameplay.Puzzles
{
    public class MoveCellStep : MergesStep
    {
        public override bool MakeSense => true;

        public MoveData MoveData { get; private set; }

        private MoveCellStep(MergesState initial) : base(initial)
        {

        }

        public static MoveCellStep CalculateStep(MergesState state, (int X, int Y) from, (int X, int Y) to)
        {
            var step = new MoveCellStep(state)
            {
                MoveData = new MoveData(from, to)
            };
            step.Final[to] = step.Initial[from];
            step.Final[from] = step.Initial[from].ChangeCell(CellType.Empty);

            return step;
        }
    }
}