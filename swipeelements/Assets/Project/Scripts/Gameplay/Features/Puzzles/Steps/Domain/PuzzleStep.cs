namespace Project.Gameplay.Puzzles
{
    public abstract class PuzzleStep
    {
        public MergesState Initial { get; private set; }
        public MergesState Final { get; internal set; }

        public abstract bool MakeSense { get; }

        protected PuzzleStep(MergesState initial)
        {
            Initial = initial;
            Final = initial.Clone();
        }
    }
}