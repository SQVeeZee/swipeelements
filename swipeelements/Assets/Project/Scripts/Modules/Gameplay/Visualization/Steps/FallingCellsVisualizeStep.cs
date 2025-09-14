using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class FallingCellsVisualizeStep : VisualizeStep<FallingCellsStep>
    {
        private readonly CellsMovingSystem _cellsMovingSystem;

        public FallingCellsVisualizeStep(
            StepsVisualizer visualizer,
            FallingCellsStep step,
            CellsMovingSystem cellsMovingSystem)
            : base(visualizer, step) =>
            _cellsMovingSystem = cellsMovingSystem;

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
        {
            UpdateMoveCells(_cellsStep.ContinuedFallingMoves);
            await MoveCellsAsync(_cellsStep.NewFallingMoves, cancellationToken);
        }

        private void UpdateMoveCells(List<FallingData> fallingData)
        {
            foreach (var data in fallingData)
            {
                _cellsMovingSystem.UpdateFallingCell(data);
            }
        }

        private async UniTask MoveCellsAsync(List<FallingData> fallingData, CancellationToken cancellationToken)
        {
            var movingTasks = new List<UniTask>();
            foreach (var data in fallingData)
            {
                movingTasks.Add(_cellsMovingSystem.FallTileAsync(data, cancellationToken));
            }
            await UniTask.WhenAll(movingTasks);
        }
    }
}