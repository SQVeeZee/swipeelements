using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using Project.Level;
using Zenject;

namespace Project.Entitas
{
    public sealed class EntitasModule
    {
        private readonly LevelController _levelController;
        private readonly BoardViewAdapter _boardViewAdapter;

        public Contexts Contexts { get; private set; }
        public global::Entitas.Systems Systems { get; private set; }

        [Inject]
        private EntitasModule(
            LevelController levelController,
            BoardViewAdapter boardViewAdapter)
        {
            _levelController = levelController;
            _boardViewAdapter = boardViewAdapter;
        }

        public UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            var levelData = _levelController.GetCurrentLevel();
            Create(levelData, _boardViewAdapter);
            return UniTask.CompletedTask;
        }

        protected void Dispose()
        {
            Systems.TearDown();
            Systems = null;
            Contexts = null;
        }

        protected void Tick()
        {
            Systems.Execute();
            Systems.Cleanup();
        }

        private void Create(LevelData levelData, IBoardView boardView)
        {
            Contexts = new Contexts();
            Contexts.level.SetLevelConfig(levelData);

            Systems = new Feature("Root")
                .Add(new PuzzleFeature(Contexts, boardView));
            Systems.Initialize();
        }
    }
}