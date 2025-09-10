using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ModestTree;
using Project.Core;
using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class MergesBoard
    {
        private readonly MergesGame _mergesGame;
        private readonly CellsContainer _cellsContainer;
        private readonly CellsMovingSystem _cellsMovingSystem;
        private readonly ILevelResultHandler _levelResultHandler;
        private readonly ICancellationToken _levelCancellationToken;
        private readonly Queue<MergesStep> _steps = new();

        public bool IsVisualizing { get; private set; } = false;

        public CellsContainer CellsContainer => _cellsContainer;

        [Inject]
        private MergesBoard(
            MergesGame mergesGame,
            CellsContainer cellsContainer,
            CellsMovingSystem cellsMovingSystem,
            ILevelResultHandler levelResultHandler,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationToken levelCancellationToken)
        {
            _mergesGame = mergesGame;
            _cellsContainer = cellsContainer;
            _cellsMovingSystem = cellsMovingSystem;
            _levelResultHandler = levelResultHandler;
            _levelCancellationToken = levelCancellationToken;
        }

        public void Initialize()
        {
            _mergesGame.OnGameChanged += OnGameChanged;
        }

        public void Dispose()
        {
            _steps.Clear();
            _mergesGame.OnGameChanged -= OnGameChanged;
        }

        private void OnGameChanged(MergesAction action, MergesState prevState, MergesStep step)
        {
            ApplyStep(action, prevState, step);
        }

        private void ApplyStep(MergesAction action, MergesState prevState, MergesStep step)
        {
            if (step == null)
            {
                return;
            }

            _steps.Enqueue(step);

            if (IsVisualizing)
            {
                return;
            }
            VisualizeSteps(_levelCancellationToken.Token).Forget();
        }

        private async UniTaskVoid VisualizeSteps(CancellationToken cancellationToken)
        {
            IsVisualizing = true;

            try
            {
                while (!_steps.IsEmpty())
                {
                    var step = _steps.Dequeue();

                    await GetVisualizerForStepAsync(step, cancellationToken);
                    await UniTask.Yield(cancellationToken);
                }
            }
            finally
            {
                IsVisualizing = false;
            }
        }

        private UniTask GetVisualizerForStepAsync(MergesStep iteration, CancellationToken cancellationToken) =>
            iteration switch
            {
                InitializeGridStep step => InitializeGridAsync(step, cancellationToken),
                SwitchCellsStep step => SwitchCellStepAsync(step, cancellationToken),
                MoveCellStep step => MoveCellStepAsync(step, cancellationToken),
                BoardDestroyStep step => BoardDestroyStepAsync(step, cancellationToken),
                BoardGravityStep step => BoardGravityStepAsync(step, cancellationToken),
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
