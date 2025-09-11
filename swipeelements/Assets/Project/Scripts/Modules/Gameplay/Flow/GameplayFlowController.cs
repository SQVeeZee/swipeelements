using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Level;
using Project.Core;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class GameplayFlowController : IInitializableModuleAsync, IDisposable
    {
        private readonly LevelController _levelController;
        private readonly LevelInitializer _levelInitializer;
        private readonly VisualizationProgress _visualizationProgress;
        private readonly ICancellationTokenControl _levelCancellationTokenControl;
        private readonly ICancellationToken _appCancellationToken;

        public GameplayFlowController(
            LevelController levelController,
            LevelInitializer levelInitializer,
            VisualizationProgress visualizationProgress,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationTokenControl levelCancellationTokenControl,
            [Inject(Id = AppCancellationToken.Id)] ICancellationToken appCancellationToken)
        {
            _levelController = levelController;
            _levelInitializer = levelInitializer;
            _visualizationProgress = visualizationProgress;
            _levelCancellationTokenControl = levelCancellationTokenControl;
            _appCancellationToken = appCancellationToken;
        }

        UniTask IInitializableModuleAsync.InitializeAsync(CancellationToken cancellationToken)
        {
            _levelController.OnLevelFinished += HandleLevelResult;
            _levelInitializer.Initialize();
            StartNewSession();
            return default;
        }

        void IDisposable.Dispose()
        {
            _levelController.OnLevelFinished -= HandleLevelResult;
        }

        private void StartNewSession()
        {
            _levelCancellationTokenControl.CreateToken();
            var levelData = _levelController.GetCurrentLevel();
            _levelInitializer.InitializeLevel(levelData);
        }

        private void HandleLevelResult(LevelResult result)
        {
            switch (result)
            {
                case LevelResult.Success:
                case LevelResult.Skip:
                case LevelResult.Restart:
                    DisposeLevel();
                    WaitForVisualizationAndStart(_appCancellationToken.Token).Forget();
                    break;
            }
        }

        private void DisposeLevel()
        {
            _levelCancellationTokenControl.Cancel();
            _levelInitializer.DisposeLevel();
        }

        private async UniTask WaitForVisualizationAndStart(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => !_visualizationProgress.IsVisualizing, cancellationToken: cancellationToken);
            StartNewSession();
        }
    }
}