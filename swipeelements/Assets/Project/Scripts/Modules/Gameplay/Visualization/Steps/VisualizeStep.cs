using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public abstract class VisualizeStep<T>
        where T : MergesStep
    {
        protected readonly T Step;

        protected readonly StepsVisualizer Visualizer;
        protected CellsContainer CellsContainer { get; private set; }

        protected VisualizeStep(StepsVisualizer visualizer, T step)
        {
            Step = step;
            Visualizer = visualizer;

            CellsContainer = Visualizer.CellsContainer;
        }

        public abstract UniTask ApplyAsync(CancellationToken cancellationToken);
    }
}