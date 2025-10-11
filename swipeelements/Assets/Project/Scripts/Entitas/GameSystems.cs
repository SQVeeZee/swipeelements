using Project.Entitas.SortOrder;
using Zenject;

namespace Project.Entitas
{
    public sealed class GameSystems : Feature
    {
        public GameSystems(
            Contexts contexts,
            DiContainer diContainer) : base("Puzzle")
        {
            //input
            Add(new ApplyInputSystem(contexts));

            //board
            Add(diContainer.Instantiate<BoardInitialSystem>(new object[] { contexts }));
            Add(diContainer.Instantiate<BoardInitialSpawnSystem>(new object[] { contexts }));

            //puzzle
            Add(new MoveRequestValidationSystem(contexts));
            Add(diContainer.Instantiate<MoveTilesSystem>(new object[] { contexts }));
            Add(new BoardUpdateSystem(contexts));
            Add(new SortOrderSystem(contexts));

            // Add(new PlanGravitySystem(contexts));

            // Events (Generated)
            // Add(new InputEventSystems(contexts));
            Add(new GameEventSystems(contexts));
            // Add(new GameStateEventSystems(contexts));

            // Cleanup (Generated)
            Add(new InputCleanupSystems(contexts));
            Add(new GameCleanupSystems(contexts));
        }
    }
}