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
        private readonly MergesBoard _mergesBoard;
        private readonly ICancellationTokenControl _cancellationTokenControl;

        public GameplayFlowController(
            LevelController levelController,
            LevelInitializer levelInitializer,
            MergesBoard mergesBoard,
            [Inject(Id = LevelCancellationToken.Id)] ICancellationTokenControl cancellationTokenControl)
        {
            _levelController = levelController;
            _levelInitializer = levelInitializer;
            _mergesBoard = mergesBoard;
            _cancellationTokenControl = cancellationTokenControl;
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
            var token = _cancellationTokenControl.CreateToken();
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
                    WaitForVisualizeEnded().Forget();
                    break;
            }
        }

        private async UniTask WaitForVisualizeEnded()
        {
            _cancellationTokenControl.Cancel();
            _levelInitializer.DisposeLevel();
            await UniTask.WaitUntil(() => !_mergesBoard.IsVisualizing);
            StartNewSession();
        }
    }
}