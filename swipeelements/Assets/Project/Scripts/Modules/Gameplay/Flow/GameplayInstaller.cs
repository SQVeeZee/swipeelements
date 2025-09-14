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

        public override void InstallBindings()
        {
            BindControllers();
            BindInput();
            BindProfiles();
            BindCancellationTokens();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<GameplayFlowController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayTimer>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayInputHandler>().AsSingle();
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
