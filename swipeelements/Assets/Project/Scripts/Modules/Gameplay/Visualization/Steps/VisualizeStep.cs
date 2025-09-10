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

        protected void OnCellUpdate(CellInfo cell) => Board.OnCellUpdate(cell);
        protected void OnCellRemove(CellInfo cell) => Board.OnCellRemove(cell);
        protected void OnCellRemove((int X, int Y) coord, MergesCell cell) => Board.OnCellRemove(new CellInfo(coord, cell));

        protected void OnFeatUpdate(CellInfo cell) => Board.OnFeatUpdate(cell);
        protected void OnFeatRemove(CellInfo cell) => Board.OnFeatRemove(cell);
        protected void OnFeatRemove((int X, int Y) coord, MergesCell cell) => Board.OnFeatRemove(new CellInfo(coord, cell));
    }
}