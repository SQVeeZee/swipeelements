using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class BoardGravityVisualizeStep : VisualizeStep<BoardGravityStep>
    {
        private readonly CellsMovingSystem _cellsMovingSystem;

        public BoardGravityVisualizeStep(
            StepsVisualizer visualizer,
            BoardGravityStep step,
            CellsMovingSystem cellsMovingSystem)
            : base(visualizer, step)
            => _cellsMovingSystem = cellsMovingSystem;

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
            => await MoveCellsAsync(Step.FallingMoves, cancellationToken);

        private async UniTask MoveCellsAsync(List<MoveData> moves, CancellationToken cancellationToken)
        {
            var movingTasks = new List<UniTask>();
            foreach (var move in moves)
            {
                movingTasks.Add(_cellsMovingSystem.MoveTileAsync(move, cancellationToken));
            }
            await UniTask.WhenAll(movingTasks);
        }
    }
}