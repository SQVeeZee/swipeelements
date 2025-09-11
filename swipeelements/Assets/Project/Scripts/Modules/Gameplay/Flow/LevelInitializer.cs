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
        private readonly GameGridCalculation _gameGridCalculation;
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
            GameGridCalculation gameGridCalculation,
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
            _gameGridCalculation = gameGridCalculation;
            _cameraFitter = cameraFitter;
        }

        public void Initialize()
        {
            _poolManager.BindBoardPools();
            _animatorController.Initialize();
        }

        public void InitializeLevel(LevelData levelData)
        {
            _mergesBoard.Initialize();
            FitCamera(levelData);
            CalculateGridPositions(levelData);
            _orderController.Initialize(levelData);

            var state = _sessionProfile.MergesState ?? new MergesState(levelData);
            _mergesGame.Initialize(state, levelData);
            _sessionController.Initialize();
        }

        private void CalculateGridPositions(LevelData levelData)
        {
            var gridPositions = _gameGridCalculation.CalculateGridPositions(levelData.Columns, levelData.Rows);
            _boardSettings.Initialize(gridPositions);
        }

        private void FitCamera(LevelData levelData)
        {
            var bounds = _gameGridCalculation.GetBoardBounds(levelData.Columns, levelData.Rows);
            _cameraFitter.FitCamera(bounds);
        }

        public void DisposeLevel()
        {
            _sessionController.Dispose();
            _mergesBoard.Dispose();
            _cellsContainer.Clear();
        }
    }
}
