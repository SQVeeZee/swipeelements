using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class SwitchCellStepVisualizationStep : VisualizeStep<SwitchCellsStep>
    {
        private readonly CellsMovingSystem _cellsMovingSystem;

        public SwitchCellStepVisualizationStep(
            MergesBoard board,
            SwitchCellsStep step,
            CellsMovingSystem cellsMovingSystem)
            : base(board, step)
            => _cellsMovingSystem = cellsMovingSystem;

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
            => await _cellsMovingSystem.SwitchTilesAsync(Step.MoveData, cancellationToken);
    }
}