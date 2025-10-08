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

        private readonly Contexts _contexts;

        private Systems _systems;

        [Inject]
        private EntitasModule(
            LevelController levelController,
            DiContainer diContainer)
        {
            _levelController = levelController;
            _diContainer = diContainer;
        }

        public void Initialize()
        {
            var levelData = _levelController.GetCurrentLevel();
            CreateSystems(Contexts.sharedInstance, levelData);
        }

        private void CreateSystems(Contexts contexts, LevelData levelData)
        {
            _systems = new GameSystems(contexts, _diContainer);
            contexts.level.SetLevelConfig(levelData);
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