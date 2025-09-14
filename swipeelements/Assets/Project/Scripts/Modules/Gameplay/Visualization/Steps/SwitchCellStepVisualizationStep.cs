using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class SwitchCellStepVisualizationStep : VisualizeStep<SwitchCellsStep>
    {
        private readonly CellsMovingSystem _cellsMovingSystem;

        public SwitchCellStepVisualizationStep(
            StepsVisualizer visualizer,
            SwitchCellsStep step,
            CellsMovingSystem cellsMovingSystem)
            : base(visualizer, step)
            => _cellsMovingSystem = cellsMovingSystem;

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
            => await _cellsMovingSystem.SwitchTilesAsync(_cellsStep.MoveData, cancellationToken);
    }
}