using Project.Core;
using Project.Core.Utility;
using Project.LifeCycle;
using Project.Profile;
using UnityEngine;
using Zenject;

namespace Project.FPS
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField]
        private AppConfig _appConfig;
        [SerializeField]
        private Bootstrapper _bootstrapper;
        [SerializeField]
        private ApplicationEvents _applicationEvents;
        [SerializeField]
        private LoadingSplashScreen _loadingSplashScreen;

        public override void InstallBindings()
        {
            BindApplicationEvents();
            BindBootstrapper();
            BindServices();
            // BindLoading();

            Container.Bind<AppConfig>().FromScriptableObject(_appConfig).AsSingle();
            Container.Bind<ProjectRunner>().AsSingle();
        }

        private void BindBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<Bootstrapper>().FromInstance(_bootstrapper).AsSingle();
            var appCancellationToken = new AppCancellationToken(_bootstrapper.destroyCancellationToken);
            Container.BindCancellationToken(appCancellationToken, AppCancellationToken.Id);
        }

        private void BindServices()
        {
            Container.BindService<ProfileService>();
            Container.BindService<FramerateService>();
            Container.BindService<SceneService>();
        }

        private void BindApplicationEvents()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ApplicationQuitSignal>().OptionalSubscriber();
            Container.DeclareSignal<ApplicationPauseSignal>().OptionalSubscriber();
            Container.DeclareSignal<ApplicationFocusSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<ApplicationEvents>().FromInstance(_applicationEvents).AsSingle();
        }

        private void BindLoading()
        {
            Container.BindPanel(_loadingSplashScreen);
            Container.BindInterfacesAndSelfTo<SplashScreenProcessor>().AsSingle();
        }
    }
}