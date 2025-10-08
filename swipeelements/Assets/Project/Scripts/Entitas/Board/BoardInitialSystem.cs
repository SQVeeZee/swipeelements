using Entitas;
using Project.Entitas.Cells;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class BoardInitialSystem : IInitializeSystem
    {
        private readonly GameContext _gameContext;
        private readonly LevelContext _levelContext;

        public BoardInitialSystem(Contexts contexts)
        {
            _gameContext = contexts.game;
            _levelContext = contexts.level;
        }

        void IInitializeSystem.Initialize()
        {
            var levelData = _levelContext.levelConfig.LevelData;
            var cells = levelData.InitialValues.ToDictionary();
            for (var y = 0; y < levelData.Rows; y++)
            {
                for (var x = 0; x < levelData.Columns; x++)
                {
                    var coord = new Coord(x, y);
                    var cell = cells[coord];
                    var cellType = cell.CellType;
                    if (cellType.IsTile())
                    {
                        _gameContext.CreateTile(cellType, coord);
                    }
                    else
                    {
                        _gameContext.CreateCell(cellType, coord);
                    }
                }
            }
        }
    }
}