using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Project.Core
{
    public class SplashScreenProcessor : ILoadingProcessor
    {
        private readonly IScreenLoader<LoadingSplashScreen> _splashScreenLoader;
        private LoadingSplashScreen _screen;

        [Inject]
        private SplashScreenProcessor(IScreenLoader<LoadingSplashScreen> splashScreenLoader)
        {
            _splashScreenLoader = splashScreenLoader;
        }

        async UniTask ILoadingProcessor.ShowAsync(CancellationToken cancellationToken)
        {
            _screen = await _splashScreenLoader.LoadScreen(cancellationToken);
        }

        UniTask ILoadingProcessor.HideAsync(CancellationToken cancellationToken)
        {
            _screen.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        void ILoadingProcessor.SetProgress(float value)
        {
            return;
        }
    }
}