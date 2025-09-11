using System;
using Profile;
using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class SessionController
    {
        private readonly MergesGame _mergesGame;
        private readonly SessionProfile _sessionProfile;
        private readonly StepsVisualizer _stepsVisualizer;
        private readonly PuzzleChecker _puzzleChecker;

        [Inject]
        private SessionController(
            MergesGame mergesGame,
            SessionProfile sessionProfile,
            StepsVisualizer stepsVisualizer,
            PuzzleChecker puzzleChecker)
        {
            _mergesGame = mergesGame;
            _sessionProfile = sessionProfile;
            _stepsVisualizer = stepsVisualizer;
            _puzzleChecker = puzzleChecker;
        }

        public void Initialize()
        {
            _mergesGame.OnGameChanged += GameChangedHandler;
            _stepsVisualizer.OnVisualizationFinished += VisualizationFinishedHandler;
        }

        public void Dispose()
        {
            _mergesGame.OnGameChanged -= GameChangedHandler;
            _stepsVisualizer.OnVisualizationFinished -= VisualizationFinishedHandler;
        }

        private void GameChangedHandler(MergesAction action, MergesState state, MergesStep step)
            => RecordAction(action, step.Final);

        private void VisualizationFinishedHandler() => _puzzleChecker.CheckPuzzleState();

        private void RecordAction(MergesAction action, MergesState state)
        {
            switch (action)
            {
                case MergesAction.None:
                    return;
                case MergesAction.Recordable:
                    _sessionProfile.MergesState = state;
                    _sessionProfile.Save();
                    break;
                case MergesAction.Braking:
                    _sessionProfile.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}