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
        private readonly ILevelResultHandler _levelResultHandler;

        public CellsContainer CellsContainer { get; }

        private int _activeVisualizations;
        public bool IsVisualizing => _activeVisualizations > 0;
        public event Action OnVisualizationFinished;

        [Inject]
        private StepsVisualizer(
            CellsContainer cellsContainer,
            CellsMovingSystem cellsMovingSystem,
            ILevelResultHandler levelResultHandler)
        {
            CellsContainer = cellsContainer;
            _cellsMovingSystem = cellsMovingSystem;
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
                if (!IsVisualizing)
                {
                    OnVisualizationFinished?.Invoke();
                }
            }
        }

        private UniTask GetVisualizerForStepAsync(MergesStep iteration, CancellationToken cancellationToken) =>
            iteration switch
            {
                CombineStep step => CombineStepAsync(step, cancellationToken),
                InitializeGridStep step => InitializeGridAsync(step, cancellationToken),
                SwitchCellsStep step => SwitchCellStepAsync(step, cancellationToken),
                MoveCellStep step => MoveCellStepAsync(step, cancellationToken),
                BoardDestroyStep step => BoardDestroyStepAsync(step, cancellationToken),
                BoardGravityStep step => BoardGravityStepAsync(step, cancellationToken),
                WinGameStep step => WinGameStepAsync(step, cancellationToken),
                _ => throw new Exception($"Can't find visualizer for {iteration.GetType().Name}")
            };

        private async UniTask CombineStepAsync(CombineStep combineStep, CancellationToken cancellationToken)
        {
            foreach (var mergesStep in combineStep.Steps)
            {
                await GetVisualizerForStepAsync(mergesStep, cancellationToken);
            }
        }

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

        private async UniTask BoardDestroyStepAsync(BoardDestroyStep step, CancellationToken cancellationToken)
        {
            var vs = new BoardDestroyVisualizeStep(this, step);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask BoardGravityStepAsync(BoardGravityStep step, CancellationToken cancellationToken)
        {
            var vs = new BoardGravityVisualizeStep(this, step, _cellsMovingSystem);
            await vs.ApplyAsync(cancellationToken);
        }

        private async UniTask WinGameStepAsync(WinGameStep step, CancellationToken cancellationToken)
        {
            var vs = new WinGameVisualizationStep(this, step, _levelResultHandler);
            await vs.ApplyAsync(cancellationToken);
        }
    }
}