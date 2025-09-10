using Project.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _nextLevelButton;

        private ILevelResultHandler _levelResultHandler;

        [Inject]
        private void Construct(
            ILevelResultHandler levelResultHandler)
            => _levelResultHandler = levelResultHandler;

        private void Awake() => Initialize();
        private void OnDestroy() => Dispose();

        private void Initialize() => Subscribe();
        private void Dispose() => UnSubscribe();

        private void Subscribe()
        {
            _restartButton.onClick.AddListener(RestartClickHandler);
            _nextLevelButton.onClick.AddListener(NextLevelClickHandler);
        }

        private void UnSubscribe()
        {
            _restartButton.onClick.RemoveListener(RestartClickHandler);
            _nextLevelButton.onClick.RemoveListener(NextLevelClickHandler);
        }

        private void RestartClickHandler() => _levelResultHandler.Restart();
        private void NextLevelClickHandler() => _levelResultHandler.Skip();
    }
}
