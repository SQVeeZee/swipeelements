using System;
using Zenject;

namespace Project.Gameplay
{
    public class IdleTimer
    {
        private readonly TileIdleTimerConfig _config;
        private readonly GameplayTimer _gameplayTimer;

        public event Action<int> OnTimerTick;
        private IDisposable _subscription;

        [Inject]
        private IdleTimer(
            TileIdleTimerConfig tileIdleTimerConfig,
            GameplayTimer gameplayTimer)
        {
            _config = tileIdleTimerConfig;
            _gameplayTimer = gameplayTimer;
        }

        public void Initialize() => Subscribe();
        private void Subscribe() => _subscription = _gameplayTimer.Subscribe(_config.AnimationsDelay, OnTimerTickHandler);

        private void OnTimerTickHandler() =>OnTimerTick?.Invoke(_config.FieldPercentage);
        public void Terminate() => _subscription?.Dispose();
    }
}