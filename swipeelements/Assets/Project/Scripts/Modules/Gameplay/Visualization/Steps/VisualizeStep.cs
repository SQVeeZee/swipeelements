using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public abstract class VisualizeStep<T>
        where T : MergesStep
    {
        protected readonly T _cellsStep;

        protected readonly StepsVisualizer _visualizer;
        protected CellsContainer CellsContainer { get; private set; }

        protected VisualizeStep(StepsVisualizer visualizer, T cellsStep)
        {
            _cellsStep = cellsStep;
            _visualizer = visualizer;

            CellsContainer = _visualizer.CellsContainer;
        }

        public abstract UniTask ApplyAsync(CancellationToken cancellationToken);
    }
}