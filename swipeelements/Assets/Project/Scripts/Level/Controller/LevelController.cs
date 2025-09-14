using System;
using Project.Gameplay;
using Project.Gameplay.Puzzles;
using Project.Level.Utility;
using Project.Profile;
using Zenject;

namespace Project.Level
{
    public class LevelController : ILevelResultHandler
    {
        private readonly LevelsConfig _levelsConfig;
        private readonly LevelProgress _levelProgress;
        private readonly SessionProfile _sessionProfile;

        public event Action<LevelResult> OnLevelFinished;

        [Inject]
        private LevelController(
            LevelsConfig levelsConfig,
            LevelProgress levelProgress,
            SessionProfile sessionProfile)
        {
            _levelsConfig = levelsConfig;
            _levelProgress = levelProgress;
            _sessionProfile = sessionProfile;
        }

        void ILevelResultHandler.Restart() => RestartLevel();
        void ILevelResultHandler.Skip() => GoToNextLevel();
        void ILevelResultHandler.Complete() => CompleteLevel();

        public LevelData GetCurrentLevel()
        {
            var index = GetLoopedIndex();
            var levelAsset = _levelsConfig.GetLevelFile(index);
            var levelData = LevelSerializerUtility.DeserializeLevelAsset(levelAsset);

            return levelData;
        }

        private int GetLoopedIndex()
        {
            if (_levelsConfig.LevelCount == 0)
            {
                return 0;
            }

            return _levelProgress.LevelIndex % _levelsConfig.LevelCount;
        }

        private void CompleteLevel()
        {
            _levelProgress.CompleteLevel();
            OnLevelFinished?.Invoke(LevelResult.Success);
        }

        private void GoToNextLevel()
        {
            _levelProgress.CompleteLevel();
            OnLevelFinished?.Invoke(LevelResult.Skip);
        }

        private void RestartLevel()
        {
            _sessionProfile.Clear();
            OnLevelFinished?.Invoke(LevelResult.Restart);
        }
    }
}
