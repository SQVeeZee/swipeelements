namespace Project.Gameplay.Puzzles
{
    public class WinGameStep : MergesStep
    {
        public override bool MakeSense => true;

        public WinGameStep( MergesState initial) : base(initial) { }

        public static WinGameStep CalculateStep(MergesState state) => new(state);
    }
}