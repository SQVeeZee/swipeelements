namespace Project.Gameplay.Puzzles
{
    public struct StepData
    {
        public MergesStep Step { get; }
        public MergesAction MergesAction { get; }

        public StepData(MergesStep step, MergesAction mergesAction)
        {
            Step = step;
            MergesAction = mergesAction;
        }
    }
}