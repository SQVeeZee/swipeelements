using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class BackgroundPanel : MonoBehaviour
    {
        private BackgroundBalloonController _balloonController;

        [Inject]
        private void Construct(BackgroundBalloonController balloonController)
            => _balloonController = balloonController;

        private void Start() => Initialize();
        private void OnDestroy() => Dispose();

        private void Initialize() => _balloonController.Initialize();
        private void Dispose() => _balloonController.Dispose();
    }
}