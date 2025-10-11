using Entitas;
using JetBrains.Annotations;
using Project.Gameplay;
using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    [UsedImplicitly]
    public sealed class BoardInitialSystem : IInitializeSystem
    {
        private readonly GameGridCalculation _gameGridCalculation;
        private readonly GameContext _gameContext;
        private readonly LevelContext _levelContext;

        public BoardInitialSystem(Contexts contexts, GameGridCalculation gameGridCalculation)
        {
            _gameGridCalculation = gameGridCalculation;
            _gameContext = contexts.game;
            _levelContext = contexts.level;
        }

        void IInitializeSystem.Initialize()
        {
            var levelData = _levelContext.levelConfig.LevelData;
            var gridPositions = _gameGridCalculation.CalculateGridPositions(levelData.Columns, levelData.Rows);
            var cells = levelData.InitialValues.ToDictionary();
            foreach (var gridPosition in gridPositions)
            {
                var coord = gridPosition.coord;
                var position = gridPosition.position;
                var cell = cells[coord];
                var cellType = cell.CellType;

                _gameContext.CreateCell(CellType.Empty, coord, position);
                if (cellType.IsTile())
                {
                    _gameContext.CreateTile(cellType, coord, position);
                }
            }
        }
    }
}