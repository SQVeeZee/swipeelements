using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Project.Gameplay
{
    public class BackgroundAnimationHandler
    {
        private readonly BalloonAnimationConfig _animationConfig;

        [Inject]
        private BackgroundAnimationHandler(BalloonAnimationConfig animationConfig) => _animationConfig = animationConfig;

        public async UniTask DoAnimationAsync(IAnimationImplementation animationImplementation, IMovableAnimated animatedElement, CancellationToken cancellationToken)
            => await animationImplementation.AnimateAsync(animatedElement.MovingObject, _animationConfig, cancellationToken);

        public void Dispose()
        {
        }
    }
}