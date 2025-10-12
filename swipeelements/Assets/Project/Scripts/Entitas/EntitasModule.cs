using Entitas;
using Project.Gameplay;
using Project.Gameplay.Puzzles;
using Project.Level;
using Zenject;

namespace Project.Entitas
{
    public sealed class EntitasModule
    {
        private readonly LevelController _levelController;
        private readonly DiContainer _diContainer;
        private readonly CellsMovingConfig _cellsMovingConfig;

        private readonly Contexts _contexts;

        private Systems _systems;

        [Inject]
        private EntitasModule(
            LevelController levelController,
            DiContainer diContainer,
            CellsMovingConfig cellsMovingConfig)
        {
            _levelController = levelController;
            _diContainer = diContainer;
            _cellsMovingConfig = cellsMovingConfig;
        }

        public void Initialize()
        {
            var levelData = _levelController.GetCurrentLevel();
            CreateSystems(Contexts.sharedInstance, levelData,
                _cellsMovingConfig.GetSettings(CellMoveType.Moving),
                _cellsMovingConfig.GetSettings(CellMoveType.Falling));
        }

        private void CreateSystems(Contexts contexts, LevelData levelData, MoveSettings moveSettings, MoveSettings fallingSettings)
        {
            _systems = new GameSystems(contexts, _diContainer);
            contexts.level.SetLevelConfig(levelData);
            contexts.game.SetMoveConfig(moveSettings);
            contexts.game.SetFallingConfig(fallingSettings);
            _systems.Initialize();
        }

        private void Dispose()
        {
            _systems.TearDown();
            _systems = null;
        }

        public void Tick()
        {
            _systems.Execute();
            _systems.Cleanup();
        }
    }
}