using Profile;
using Project.Core;
using Project.Core.Utility;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private Runner _runner;
        [SerializeField]
        private CellsMovingConfig _cellsMovingConfig;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameplayFlowController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelInitializer>().AsSingle();
            Container.BindInterfacesAndSelfTo<SessionController>().AsSingle();

            BindInput();
            BindMoving();
            BindTimer();
            BindProfiles();
            BindCancellationTokens();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayInputHandler>().AsSingle();
        }

        private void BindMoving()
        {
            Container.Bind<CellsMovingSystem>().AsSingle();
            Container.Bind<CellsMovingController>().AsSingle();
            Container.Bind<CellsMovingConfig>().FromInstance(_cellsMovingConfig).AsSingle();
        }

        private void BindTimer()
        {
            Container.BindInterfacesAndSelfTo<GameplayTimer>().AsSingle();
            Container.Bind<IdleTimer>().AsSingle();
        }

        private void BindProfiles()
        {
            Container.BindService<ProfileService>();
            Container.BindProfile<GeneralProfile>();
            Container.BindProfile<SessionProfile>();
        }

        private void BindCancellationTokens()
        {
            Container.BindSelfRunCancellationToken<LevelCancellationToken>(LevelCancellationToken.Id);
            var appCancellationToken = new AppCancellationToken(_runner.destroyCancellationToken);
            Container.BindCancellationToken(appCancellationToken, AppCancellationToken.Id);
        }
    }
}
