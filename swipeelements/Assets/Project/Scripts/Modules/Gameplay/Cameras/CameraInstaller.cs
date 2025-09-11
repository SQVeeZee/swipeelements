using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField]
        private GameCameraView _gameCamera;
        [SerializeField]
        private UICameraView _uiCamera;

        public override void InstallBindings() => BindCameras();

        private void BindCameras()
        {
            BindGameCamera();
            BinUICamera();
        }

        private void BindGameCamera()
        {
            Container.Bind<ICameraView>()
                .WithId(CameraIds.GameCamera)
                .FromInstance(_gameCamera);

            Container.Bind<ICameraFitter>()
                .FromInstance(_gameCamera);
        }

        private void BinUICamera() =>
            Container.Bind<ICameraView>()
                .WithId(CameraIds.UICamera)
                .FromInstance(_uiCamera)
                .AsSingle();
    }
}