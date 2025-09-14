using System;
using Zenject;

namespace Project.Gameplay
{
    public class BackgroundTimerHandler
    {
        private readonly GameplayTimer _gameplayTimer;
        private readonly BackgroundConfig _backgroundConfig;
        private IDisposable _subscription;

        public event Action OnBackgroundTick;

        [Inject]
        private BackgroundTimerHandler(
            GameplayTimer gameplayTimer,
            BackgroundConfig backgroundConfig)
        {
            _gameplayTimer = gameplayTimer;
            _backgroundConfig = backgroundConfig;
        }

        public void Initialize() => Subscribe();
        public void Dispose() => _subscription?.Dispose();

        private void Subscribe() => _subscription = _gameplayTimer.Subscribe(_backgroundConfig.SpawnInterval, BackgroundTickHandler);

        private void BackgroundTickHandler() => OnBackgroundTick?.Invoke();
    }
}