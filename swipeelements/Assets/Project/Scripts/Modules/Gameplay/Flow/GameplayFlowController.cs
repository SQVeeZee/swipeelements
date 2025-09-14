using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Level;
using Project.Core;
using Project.Profile;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class GameplayFlowController : IInitializableModuleAsync, IDisposable
    {
        private readonly LevelController _levelController;
        private readonly LevelInitializer _levelInitializer;
        private readonly SessionProfile _sessionProfile;
        private readonly VisualizationProgress _visualizationProgress;
        private readonly ICancellationTokenControl _levelCancellationTokenControl;
        private readonly ICancellationToken _appCancellationToken;
        private readonly List<ISystemClear> _systemClears;

        public GameplayFlowController(
            LevelController levelController,
            LevelInitializer levelInitializer,
            SessionProfile sessionProfile,
            VisualizationProgress visualizationProgress,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationTokenControl levelCancellationTokenControl,
            [Inject(Id = AppCancellationToken.Id)] ICancellationToken appCancellationToken,
            List<ISystemClear> systemClears)
        {
            _levelController = levelController;
            _levelInitializer = levelInitializer;
            _sessionProfile = sessionProfile;
            _visualizationProgress = visualizationProgress;
            _levelCancellationTokenControl = levelCancellationTokenControl;
            _appCancellationToken = appCancellationToken;
            _systemClears = systemClears;
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
            _levelInitializer.Terminate();
            _systemClears.ForEach(system => system.Terminate());
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
                    DisposeLevel();
                    WaitForVisualizationAndStart(_appCancellationToken.Token).Forget();
                    break;
                case LevelResult.Skip:
                case LevelResult.Restart:
                    CleanProfile();
                    DisposeLevel();
                    WaitForVisualizationAndStart(_appCancellationToken.Token).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        private void CleanProfile() => _sessionProfile.Clear();

        private void DisposeLevel()
        {
            _levelCancellationTokenControl.Cancel();
            _levelInitializer.DisposeLevel();
            _systemClears.ForEach(system => system.Clear());
        }

        private async UniTask WaitForVisualizationAndStart(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => !_visualizationProgress.IsVisualizing, cancellationToken: cancellationToken);
            StartNewSession();
        }
    }
}