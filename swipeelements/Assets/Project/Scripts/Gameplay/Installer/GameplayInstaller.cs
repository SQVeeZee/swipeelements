using Project.Core.Utility;
using Project.Entitas;
using Project.Profile;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private BackgroundPanel _backgroundPanel;
        [SerializeField]
        private GameSafeAreaPanel _safeArea;

        public override void InstallBindings()
        {
            BindControllers();
            BindInput();
            BindProfiles();
            BindCancellationTokens();
            BindPanels();

            Container.BindInterfacesAndSelfTo<EntitasModule>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayRunner>().AsSingle();
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
            Container.BindProfile<GeneralProfile>();
            Container.BindProfile<SessionProfile>();
        }

        private void BindCancellationTokens()
        {
            Container.BindSelfRunCancellationToken<LevelCancellationToken>(LevelCancellationToken.Id);
        }

        private void BindPanels()
        {
            Container.Bind<BackgroundPanel>().FromInstance(_backgroundPanel).AsSingle();
            Container.BindInterfacesAndSelfTo<GameSafeAreaPanel>().FromInstance(_safeArea);
        }
    }
}
