using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public abstract class VisualizeStep<T>
        where T : MergesStep
    {
        protected readonly T Step;

        protected readonly MergesBoard Board;
        protected CellsContainer CellsContainer { get; private set; }

        protected VisualizeStep(MergesBoard board, T step)
        {
            Step = step;
            Board = board;

            CellsContainer = Board.CellsContainer;
        }

        public abstract UniTask ApplyAsync(CancellationToken cancellationToken);
    }
}