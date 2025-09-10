namespace Project.Gameplay.Puzzles
{
    public class NonSenseStep : MergesStep
    {
        public override bool MakeSense => false;

        public NonSenseStep(MergesState initial) : base(initial)
        {
        }
    }
}