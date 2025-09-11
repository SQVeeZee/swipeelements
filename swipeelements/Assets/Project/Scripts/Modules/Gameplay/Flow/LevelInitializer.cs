using System.Threading;
using JetBrains.Annotations;
using Level;
using Profile;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class LevelInitializer
    {
        private readonly GamePoolManager _poolManager;
        private readonly MergesBoard _mergesBoard;
        private readonly MergesGame _mergesGame;
        private readonly CellOrderController _orderController;
        private readonly TileAnimatorController _animatorController;
        private readonly SessionProfile _sessionProfile;
        private readonly BoardSettings _boardSettings;
        private readonly CellsContainer _cellsContainer;
        private readonly SessionController _sessionController;
        private readonly ICameraFitter _cameraFitter;

        public LevelInitializer(
            GamePoolManager poolManager,
            MergesBoard board,
            MergesGame mergesGame,
            CellOrderController orderController,
            TileAnimatorController animatorController,
            SessionProfile sessionProfile,
            BoardSettings boardSettings,
            CellsContainer cellsContainer,
            SessionController sessionController,
            ICameraFitter cameraFitter)
        {
            _poolManager = poolManager;
            _mergesBoard = board;
            _mergesGame = mergesGame;
            _orderController = orderController;
            _animatorController = animatorController;
            _sessionProfile = sessionProfile;
            _boardSettings = boardSettings;
            _cellsContainer = cellsContainer;
            _sessionController = sessionController;
            _cameraFitter = cameraFitter;
        }

        public void Initialize()
        {
            _poolManager.BindBoardPools();
            _animatorController.Initialize();
        }

        public void InitializeLevel(LevelData data)
        {
            _mergesBoard.Initialize();
            _boardSettings.InitializeSettings(data);
            _orderController.Initialize(data);
            FitCamera(data);

            var state = _sessionProfile.MergesState ?? new MergesState(data);
            _mergesGame.Initialize(state, data);
            _sessionController.Initialize();
        }

        public void DisposeLevel()
        {
            _sessionController.Dispose();
            _mergesBoard.Dispose();
            _cellsContainer.Clear();
        }

        private void FitCamera(LevelData data)
        {
            var bounds = BoardBoundsExtension.GetBoardBounds(data.Columns, data.Rows, _boardSettings.CellSize);
            _cameraFitter.FitCamera(bounds);
        }
    }
}
