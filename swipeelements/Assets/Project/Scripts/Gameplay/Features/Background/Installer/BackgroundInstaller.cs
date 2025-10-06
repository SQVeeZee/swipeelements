using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class BackgroundInstaller : MonoInstaller
    {
        [SerializeField]
        private BackgroundConfig _backgroundConfig;
        [SerializeField]
        private BalloonAnimationConfig _balloonAnimationConfig;

        [SerializeField]
        private BalloonsContainer _balloonsContainer;

        public override void InstallBindings()
        {
            BindAnimation();
            BindBalloons();
        }

        private void BindBalloons()
        {
            Container.Bind<BackgroundConfig>().FromInstance(_backgroundConfig).AsSingle();
            Container.Bind<BackgroundBalloonController>().AsSingle();
            Container.Bind<BalloonsContainer>().FromInstance(_balloonsContainer).AsSingle();
            Container.Bind<BackgroundTimerHandler>().AsSingle();
        }

        private void BindAnimation()
        {
            Container.Bind<BalloonAnimationConfig>().FromInstance(_balloonAnimationConfig).AsSingle();
            Container.Bind<BackgroundAnimationHandler>().AsSingle();
        }

    }
}