using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField]
        private GameCameraView _gameCameraView;

        public override void InstallBindings() => BindCameras();

        private void BindCameras()
        {
            Container.Bind<ICameraView>()
                .WithId(CameraIds.GameCamera)
                .FromInstance(_gameCameraView)
                .AsSingle();

            Container.Bind<ICameraFitter>()
                .FromInstance(_gameCameraView)
                .AsSingle();
        }
    }
}