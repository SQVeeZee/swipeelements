using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class MergesBoard : IDisposable
    {
        private readonly MergesGame _mergesGame;
        private readonly StepsVisualizer _stepsVisualizer;
        private readonly VisualizationProgress _visualizationProgress;
        private readonly BoardLocker _boardLocker;
        private readonly ICancellationToken _levelCancellationToken;

        [Inject]
        private MergesBoard(
            MergesGame mergesGame,
            StepsVisualizer stepsVisualizer,
            VisualizationProgress visualizationProgress,
            BoardLocker boardLocker,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationToken levelCancellationToken)
        {
            _mergesGame = mergesGame;
            _stepsVisualizer = stepsVisualizer;
            _visualizationProgress = visualizationProgress;
            _boardLocker = boardLocker;
            _levelCancellationToken = levelCancellationToken;
        }

        public void Initialize() => _mergesGame.OnGameChanged += OnGameChanged;
        public void Dispose() => _mergesGame.OnGameChanged -= OnGameChanged;

        private void OnGameChanged(MergesAction action, MergesState prevState, MergesStep step)
        {
            if (step != null)
            {
                ApplyStep(step, _levelCancellationToken.Token).Forget();
            }
        }

        private async UniTaskVoid ApplyStep(MergesStep step, CancellationToken cancellationToken)
        {
            switch (step)
            {
                case ILockedStep lockedStep:
                    await ResolveLockedStep(step, step.GlobalId, lockedStep.LockedCoords, cancellationToken);
                    break;
                case CombineStep combineStep:
                    await ResolveCombineStepAsync(combineStep, cancellationToken);
                    break;
                case WinGameStep winGameStep:
                    await ResolveWinGameStep(winGameStep, cancellationToken);
                    break;
                default:
                    await ResolveMergesStep(step, cancellationToken);
                    break;
            }
        }

        private async UniTask ResolveLockedStep(MergesStep step, string stepId, HashSet<(int X, int Y)> coords, CancellationToken cancellationToken)
        {
            _boardLocker.AddLockedCells(stepId, coords);
            try
            {
                await _stepsVisualizer.VisualizeAsync(step, cancellationToken);
            }
            finally
            {
                _boardLocker.Remove(stepId);
            }
        }

        private async UniTask ResolveCombineStepAsync(CombineStep combineStep, CancellationToken cancellationToken)
        {
            foreach (var inner in combineStep.Steps)
            {
                if (inner is ILockedStep innerLocked)
                {
                    _boardLocker.AddLockedCells(inner.GlobalId, innerLocked.LockedCoords);
                }
            }

            try
            {
                await _stepsVisualizer.VisualizeAsync(combineStep, cancellationToken);
            }
            finally
            {
                foreach (var inner in combineStep.Steps)
                {
                    if (inner is ILockedStep)
                    {
                        _boardLocker.Remove(inner.GlobalId);
                    }
                }
            }
        }

        private async UniTask ResolveWinGameStep(WinGameStep winGameStep, CancellationToken cancellationToken)
        {
            if (_visualizationProgress.IsVisualizing)
            {
                await UniTask.WaitWhile(() => _visualizationProgress.IsVisualizing, cancellationToken: cancellationToken);
            }
            await _stepsVisualizer.VisualizeAsync(winGameStep, cancellationToken);
        }

        private async UniTask ResolveMergesStep(MergesStep step, CancellationToken cancellationToken)
        {
            await _stepsVisualizer.VisualizeAsync(step, cancellationToken);
        }
    }
}
