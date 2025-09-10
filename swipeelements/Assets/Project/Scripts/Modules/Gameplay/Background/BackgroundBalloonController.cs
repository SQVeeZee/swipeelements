using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Canvas;
using Project.Core;
using Zenject;

namespace Project.Gameplay
{
    public class BackgroundBalloonController
    {
        private readonly BackgroundTimerHandler _backgroundTimerHandler;
        private readonly BalloonsContainer _balloonsContainer;
        private readonly BackgroundAnimationHandler _animationHandler;
        private readonly BackgroundConfig _backgroundConfig;
        private readonly ICanvasItem _canvasItem;
        private readonly ICancellationToken _appCancellationToken;
        private readonly BalloonSinusAnimation _animationImplementation = new ();

        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        private BackgroundBalloonController(
            BackgroundTimerHandler backgroundTimerHandler,
            BalloonsContainer balloonsContainer,
            BackgroundAnimationHandler animationHandler,
            BackgroundConfig backgroundConfig,
            [Inject(Id = CanvasIds.Background)] ICanvasItem canvasItem,
            [Inject(Id = AppCancellationToken.Id)] ICancellationToken appCancellationToken)
        {
            _backgroundTimerHandler = backgroundTimerHandler;
            _balloonsContainer = balloonsContainer;
            _animationHandler = animationHandler;
            _backgroundConfig = backgroundConfig;
            _canvasItem = canvasItem;
            _appCancellationToken = appCancellationToken;
        }

        public void Initialize()
        {
            _backgroundTimerHandler.OnBackgroundTick += BackgroundTickHandler;
            _balloonsContainer.Initialize();
            _backgroundTimerHandler.Initialize();
            InitializeAnimationHandler();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_appCancellationToken.Token);
        }

        private void InitializeAnimationHandler()
        {
            var rect = _canvasItem.RectTransform.rect;
            var xWidth = rect.width / 2f;
            _animationImplementation.Setup(xWidth);
        }

        public void Dispose()
        {
            _backgroundTimerHandler.OnBackgroundTick -= BackgroundTickHandler;
            _backgroundTimerHandler.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private void BackgroundTickHandler()
        {
            var balloonsDiff = _backgroundConfig.MaxBalloonsCount - _balloonsContainer.BalloonsInProgress;
            if (balloonsDiff <= 0)
            {
                return;
            }
            SpawnBalloons(balloonsDiff);
        }

        private void SpawnBalloons(int addBalloonsMax)
        {
            var balloonsCount = UnityEngine.Random.Range(0, addBalloonsMax + 1);
            for (var i = 0; i < balloonsCount; i++)
            {
                if (!_balloonsContainer.TryCreateBalloon(out var balloon))
                {
                    return;
                }
                SpawnBalloon(balloon, _cancellationTokenSource.Token).Forget();
            }
        }

        private async UniTaskVoid SpawnBalloon(Balloon balloon, CancellationToken cancellationToken)
        {
            await _animationHandler.DoAnimationAsync(_animationImplementation, balloon, cancellationToken);
            balloon.Dispose();
        }
    }
}