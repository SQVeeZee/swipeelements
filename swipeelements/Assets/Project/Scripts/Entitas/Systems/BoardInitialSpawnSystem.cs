using Entitas;

namespace Project.Entitas
{
    public sealed class BoardInitialSpawnSystem : IInitializeSystem
    {
        private readonly Contexts _contexts;
        private readonly IBoardView _view;

        public BoardInitialSpawnSystem(Contexts contexts, IBoardView view)
        {
            _contexts = contexts;
            _view = view;
        }

        void IInitializeSystem.Initialize()
        {
            var cells = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Tile));

            foreach (var gameEntity in cells)
            {
                var cell = gameEntity.cell;
                var cellView = _view.SpawnTile(cell.CellType, cell.Coord);
            }
        }
    }
}