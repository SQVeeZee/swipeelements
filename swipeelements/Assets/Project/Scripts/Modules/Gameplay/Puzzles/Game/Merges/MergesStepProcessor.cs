using JetBrains.Annotations;

namespace Project.Gameplay.Puzzles
{
    [UsedImplicitly]
    public class MergesStepProcessor
    {
        public StepData Initialize(MergesState state, ILevelData levelData)
        {
            MergesStep.ResetCounters();
            var step = InitializeGridStep.CalculateStep(state, levelData);
            return new StepData(step, MergesAction.Recordable);
        }

        public StepData ApplySwipe(MergesState state, (int X, int Y) from, (int X, int Y) to)
        {
            var moveData = new MoveData(from, to);
            var step = TileActionStep.Create(state, moveData);
            return new StepData(step, MergesAction.Recordable);
        }

        public StepData NormalizeBoard(MergesState state)
        {
            var step = FallingCellsStep.CalculateStep(state);
            return new StepData(step, MergesAction.Recordable);
        }

        public StepData DestroyCells(MergesState state)
        {
            var step = DestroyCellsStep.CalculateStep(state);
            return new StepData(step, MergesAction.Recordable);
        }

        public StepData TryWinGame(MergesState state)
        {
            var step = WinGameStep.CalculateStep(state);
            return new StepData(step, MergesAction.Braking);
        }
    }
}