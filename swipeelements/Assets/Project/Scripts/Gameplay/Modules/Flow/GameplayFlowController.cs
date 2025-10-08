using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Level;
using Project.Core;
using Project.Entitas;
using Project.Profile;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class GameplayFlowController : SceneModuleBase
    {
        private readonly LevelController _levelController;
        private readonly LevelInitializer _levelInitializer;
        private readonly SessionProfile _sessionProfile;
        private readonly VisualizationProgress _visualizationProgress;
        private readonly ICancellationTokenControl _levelCancellationTokenControl;
        private readonly ICancellationToken _moduleCancellationToken;
        private readonly List<ISystemClear> _systemClears;
        private readonly EntitasModule _entitasModule;

        public GameplayFlowController(
            LevelController levelController,
            LevelInitializer levelInitializer,
            SessionProfile sessionProfile,
            VisualizationProgress visualizationProgress,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationTokenControl levelCancellationTokenControl,
            [Inject(Id = ModuleCancellationToken.Id)] ICancellationToken moduleCancellationToken,
            List<ISystemClear> systemClears,
            EntitasModule entitasModule)
        {
            _levelController = levelController;
            _levelInitializer = levelInitializer;
            _sessionProfile = sessionProfile;
            _visualizationProgress = visualizationProgress;
            _levelCancellationTokenControl = levelCancellationTokenControl;
            _moduleCancellationToken = moduleCancellationToken;
            _systemClears = systemClears;
            _entitasModule = entitasModule;
        }

        protected override UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            _levelController.OnLevelFinished += HandleLevelResult;
            _levelInitializer.Initialize();
            StartNewSession();
            _entitasModule.Initialize();
            return UniTask.CompletedTask;
        }

        protected override void Tick()
        {
            _entitasModule.Tick();
        }

        protected override void Dispose()
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
                    WaitForVisualizationAndStart(_moduleCancellationToken.Token).Forget();
                    break;
                case LevelResult.Skip:
                case LevelResult.Restart:
                    CleanProfile();
                    DisposeLevel();
                    WaitForVisualizationAndStart(_moduleCancellationToken.Token).Forget();
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