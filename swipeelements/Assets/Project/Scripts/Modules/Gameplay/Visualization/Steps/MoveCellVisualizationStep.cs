using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class MoveCellVisualizationStep : VisualizeStep<MoveCellStep>
    {
        private readonly CellsMovingSystem _cellsMovingSystem;

        public MoveCellVisualizationStep(
            StepsVisualizer visualizer,
            MoveCellStep step,
            CellsMovingSystem cellsMovingSystem)
            : base(visualizer, step)
            => _cellsMovingSystem = cellsMovingSystem;

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
            => await _cellsMovingSystem.MoveTileAsync(Step.MoveData, cancellationToken);
    }
}