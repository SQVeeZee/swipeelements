using Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public sealed class BoardInitialSystem : IInitializeSystem
    {
        private readonly GameContext _gameContext;
        private readonly LevelContext _levelContext;

        public BoardInitialSystem(Contexts c)
        {
            _gameContext = c.game;
            _levelContext = c.level;
        }

        void IInitializeSystem.Initialize()
        {
            var levelData = _levelContext.levelConfig.LevelData;
            var cells = levelData.InitialValues.ToDictionary();
            for (var y = 0; y < levelData.Rows; y++)
            {
                for (var x = 0; x < levelData.Columns; x++)
                {
                    var coord = (x, y);
                    var cell = cells[coord];
                    var cellType = cell.CellType;
                    var gameEntity = _gameContext.CreateEntity();
                    gameEntity.AddCell(cellType, coord);
                    if (cellType.IsTile())
                    {
                        var cellState = cellType.IsTile() ? CellState.Idle : CellState.None;
                        gameEntity.AddTile(cellState);
                    }
                }
            }
        }
    }
}