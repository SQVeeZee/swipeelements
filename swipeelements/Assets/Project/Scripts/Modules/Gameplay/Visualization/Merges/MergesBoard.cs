using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ModestTree;
using Project.Core;
using Project.Gameplay.Puzzles;
using UniRx;
using UnityEngine;
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

        public readonly ReactiveProperty<UniTaskCompletionSource<bool>> Visualizing = new();

        public event Action<MergesStep> OnVisualizationStepFinished;
        public event Action OnVisualizationFinished;

        public event Action<CellInfo> OnCellRemoved;
        public event Action<CellInfo> OnCellUpdated;
        public event Action<CellInfo> OnFeatRemoved;
        public event Action<CellInfo> OnFeatUpdated;

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

        public void OnCellRemove(CellInfo cell) => OnCellRemoved?.Invoke(cell);
        public void OnFeatRemove(CellInfo cell) => OnFeatRemoved?.Invoke(cell);
        public void OnCellUpdate(CellInfo cell) => OnCellUpdated?.Invoke(cell);
        public void OnFeatUpdate(CellInfo cell) => OnFeatUpdated?.Invoke(cell);

        public void Initialize()
        {
            _mergesGame.OnGameChanged += OnGameChanged;
        }

        public void Dispose()
        {
            Visualizing.Value?.TrySetCanceled();
            Visualizing.Value = null;
        }

        public void Terminate()
        {
            _mergesGame.OnGameChanged -= OnGameChanged;
            Dispose();
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
            VisualizeSteps(_levelCancellationToken.Token).Forget();
        }

        private async UniTaskVoid VisualizeSteps(CancellationToken cancellationToken)
        {
            if (Visualizing.Value != null)
            {
                return;
            }
            Visualizing.Value = new UniTaskCompletionSource<bool>();

            while (!_steps.IsEmpty())
            {
                var frame = Time.frameCount;
                var step = _steps.Dequeue();

                await VisualizeStepAsync(step, cancellationToken);
                if (frame != Time.frameCount)
                {
                    continue;
                }
                await UniTask.Yield(cancellationToken);
            }

            if (Visualizing.Value != null)
            {
                Visualizing.Value.TrySetResult(true);
            }
            OnVisualizationFinished?.Invoke();
            Visualizing.Value = null;
        }

        private async UniTask VisualizeStepAsync(MergesStep step, CancellationToken cancellationToken)
        {
            await GetVisualizerForStepAsync(step, cancellationToken);
            OnVisualizationStepFinished?.Invoke(step);
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
