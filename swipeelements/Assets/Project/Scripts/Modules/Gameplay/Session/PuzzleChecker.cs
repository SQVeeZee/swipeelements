using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class PuzzleChecker
    {
        private readonly MergesGame _mergesGame;
        private readonly BoardLocker _boardLocker;

        [Inject]
        private PuzzleChecker(
            MergesGame mergesGame,
            BoardLocker boardLocker)
        {
            _mergesGame = mergesGame;
            _boardLocker = boardLocker;
        }

        public void CheckPuzzleState()
        {
            var lockedCoords = _boardLocker.GetAllLockedCoords();
            _mergesGame.ResolveBoard(lockedCoords);
        }
    }
}