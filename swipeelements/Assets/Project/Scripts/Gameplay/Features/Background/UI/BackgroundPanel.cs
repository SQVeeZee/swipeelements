using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class BackgroundPanel : MonoBehaviour, ISceneModule
    {
        private BackgroundBalloonController _balloonController;

        [Inject]
        private void Construct(BackgroundBalloonController balloonController)
            => _balloonController = balloonController;

        UniTask ISceneModule.InitializeAsync(CancellationToken cancellationToken)
        {
            _balloonController.Initialize();
            return UniTask.CompletedTask;
        }

        public void Tick(){}

        void ISceneModule.Dispose() => _balloonController.Dispose();
    }
}