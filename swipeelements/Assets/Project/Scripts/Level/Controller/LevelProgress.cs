using Project.Profile;
using Zenject;

namespace Level
{
    public class LevelProgress
    {
        private readonly GeneralProfile _generalProfile;

        [Inject]
        private LevelProgress(GeneralProfile generalProfile) => _generalProfile = generalProfile;

        public int LevelIndex => _generalProfile.CurrentLevelIndex;

        public void CompleteLevel()
        {
            _generalProfile.CurrentLevelIndex++;
            _generalProfile.Save();
        }
    }
}