using Project.Core;
using Project.Core.Utility;
using Project.Profile;
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
            BindControllers();
            BindInput();
            BindMoving();
            BindTimer();
            BindProfiles();
            BindCancellationTokens();
            BindDestroy();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<GameplayFlowController>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayInputHandler>().AsSingle();
        }

        private void BindMoving()
        {
            Container.BindInterfacesAndSelfTo<CellsMovingSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CellsMovingController>().AsSingle();
            Container.Bind<CellsMovingConfig>().FromInstance(_cellsMovingConfig).AsSingle();

            Container.Bind<LinearMover>().AsSingle();
            Container.Bind<FallingMover>().AsSingle();
        }

        private void BindDestroy()
        {
            Container.BindInterfacesAndSelfTo<DestroyCellsSystem>().AsSingle();
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
