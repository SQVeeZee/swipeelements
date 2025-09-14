using System;
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
        private readonly ICancellationToken _levelCancellationToken;
        private readonly ICancellationToken _appCancellationToken;
        private CancellationTokenSource _cancellationToken;

        [Inject]
        private MergesBoard(
            MergesGame mergesGame,
            StepsVisualizer stepsVisualizer,
            VisualizationProgress visualizationProgress,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationToken levelCancellationToken,
            [Inject(Id = AppCancellationToken.Id)] ICancellationToken appCancellationToken)
        {
            _mergesGame = mergesGame;
            _stepsVisualizer = stepsVisualizer;
            _visualizationProgress = visualizationProgress;
            _levelCancellationToken = levelCancellationToken;
            _appCancellationToken = appCancellationToken;
        }

        public void Initialize()
        {
            _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(_appCancellationToken.Token, _levelCancellationToken.Token);
            _mergesGame.OnStepApply += OnGameChanged;
        }

        public void Dispose() => _mergesGame.OnStepApply -= OnGameChanged;

        private void OnGameChanged(MergesStep step)
        {
            if (step != null)
            {
                ApplyStep(step, _cancellationToken.Token).Forget();
            }
        }

        private async UniTaskVoid ApplyStep(MergesStep step, CancellationToken cancellationToken)
        {
            switch (step)
            {
                case WinGameStep winGameStep:
                    await ResolveWinGameStep(winGameStep, cancellationToken);
                    break;
                default:
                    await ResolveMergesStep(step, cancellationToken);
                    break;
            }
            _mergesGame.RunNextStepIfNeeded(step);
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
