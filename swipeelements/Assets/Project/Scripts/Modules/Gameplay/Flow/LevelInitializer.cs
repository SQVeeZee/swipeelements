using JetBrains.Annotations;
using Level;
using Project.Gameplay.Puzzles;
using Project.Profile;

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
            _gameGridCalculation = gameGridCalculation;
            _cameraFitter = cameraFitter;
        }

        public void Initialize()
        {
            _poolManager.BindBoardPools();
            _animatorController.Initialize();
        }

        public void Terminate()
        {
            _animatorController.Dispose();
        }

        public void InitializeLevel(LevelData levelData)
        {
            _mergesBoard.Initialize();
            FitCamera(levelData);
            CalculateGridPositions(levelData);
            _orderController.Initialize(levelData);

            StartLevel(levelData);
        }

        private void StartLevel(LevelData levelData)
        {
            if (_sessionProfile.MergesState == null)
            {
                _mergesGame.Initialize(new MergesState(levelData), levelData);
            }
            else
            {
                _mergesGame.InitializeContinue(_sessionProfile.MergesState, levelData);
            }
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
            _mergesBoard.Dispose();
            _cellsContainer.Clear();
        }
    }
}
