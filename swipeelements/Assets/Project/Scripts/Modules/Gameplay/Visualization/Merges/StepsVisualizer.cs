using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class StepsVisualizer
    {
        private readonly CellsMovingSystem _cellsMovingSystem;
        private readonly DestroyCellsSystem _destroyCellsSystem;
        private readonly ILevelResultHandler _levelResultHandler;

        public CellsContainer CellsContainer { get; }

        private int _activeVisualizations;
        public bool IsVisualizing => _activeVisualizations > 0;

        [Inject]
        private StepsVisualizer(
            CellsContainer cellsContainer,
            CellsMovingSystem cellsMovingSystem,
            DestroyCellsSystem destroyCellsSystem,
            ILevelResultHandler levelResultHandler)
        {
            CellsContainer = cellsContainer;
            _cellsMovingSystem = cellsMovingSystem;
            _destroyCellsSystem = destroyCellsSystem;
            _levelResultHandler = levelResultHandler;
        }

        public async UniTask VisualizeAsync(MergesStep step, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _activeVisualizations);
            try
            {
                await GetVisualizerForStepAsync(step, cancellationToken);
            }
            finally
            {
                Interlocked.Decrement(ref _activeVisualizations);
            }
        }

        private UniTask GetVisualizerForStepAsync(MergesStep iteration, CancellationToken cancellationToken) =>
            iteration switch
            {
                InitializeGridStep step => InitializeGridAsync(step, cancellationToken),
                SwitchCellsStep step => SwitchCellStepAsync(step, cancellationToken),
                MoveCellStep step => MoveCellStepAsync(step, cancellationToken),
                DestroyCellsStep step => BoardDestroyStepAsync(step, cancellationToken),
                FallingCellsStep step => BoardGravityStepAsync(step, cancellationToken),
                WinGameStep step => WinGameStepAsync(step, cancellationToken),
                _ => throw new Exception($"Can't find visualizer for {iteration.GetType().Name}")
            };

        private async UniTask InitializeGridAsync(InitializeGridStep step, CancellationToken cancellationToken)
        {
            var vs = new InitializeGridVisualizeStep(this, step);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask SwitchCellStepAsync(SwitchCellsStep step, CancellationToken cancellationToken)
        {
            var vs = new SwitchCellStepVisualizationStep(this, step, _cellsMovingSystem);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask MoveCellStepAsync(MoveCellStep step, CancellationToken cancellationToken)
        {
            var vs = new MoveCellVisualizationStep(this, step, _cellsMovingSystem);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask BoardDestroyStepAsync(DestroyCellsStep cellsStep, CancellationToken cancellationToken)
        {
            var vs = new BoardDestroyVisualizeStep(this, cellsStep, _destroyCellsSystem);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask BoardGravityStepAsync(FallingCellsStep step, CancellationToken cancellationToken)
        {
            var vs = new FallingCellsVisualizeStep(this, step, _cellsMovingSystem);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask WinGameStepAsync(WinGameStep step, CancellationToken cancellationToken)
        {
            var vs = new WinGameVisualizationStep(this, step, _levelResultHandler);
            await vs.ApplyAsync(cancellationToken);
        }
    }
}