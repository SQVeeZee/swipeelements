using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class WinGameVisualizationStep : VisualizeStep<WinGameStep>
    {
        private readonly ILevelResultHandler _levelResultHandler;

        public WinGameVisualizationStep(
            StepsVisualizer visualizer,
            WinGameStep step,
            ILevelResultHandler levelResultHandler) : base(visualizer, step)
            => _levelResultHandler = levelResultHandler;

        public override UniTask ApplyAsync(CancellationToken cancellationToken)
        {
            CompleteLevel();
            return default;
        }

        private void CompleteLevel() => _levelResultHandler.Complete();
    }
}