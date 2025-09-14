using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class CellsStateConverter
    {
        private readonly CellsContainer _cellsContainer;
        private readonly CellsMovingSystem _cellsMovingSystem;
        private readonly DestroyCellsSystem _destroyCellsSystem;

        [Inject]
        private CellsStateConverter(
            CellsContainer cellsContainer,
            CellsMovingSystem cellsMovingSystem,
            DestroyCellsSystem destroyCellsSystem)
        {
            _cellsContainer = cellsContainer;
            _cellsMovingSystem = cellsMovingSystem;
            _destroyCellsSystem = destroyCellsSystem;
        }

        public MergesState ToState(MergesState mergesState)
        {
            var state = mergesState.Clone();
            for (var y = 0; y < mergesState.Rows; y++)
            {
                for (var x = 0; x < mergesState.Columns; x++)
                {
                    var coord = (x, y);
                    var cellState = GetCellState(coord);
                    if (_cellsContainer.TryGetValue(coord, out var cellObj))
                    {
                        state[coord] = state[coord].ChangeCell(cellObj.Info.CellType, cellState);
                    }
                    else
                    {
                        state[coord] = MergesCell.Empty.ChangeCell(cellState);
                    }
                }
            }

            return state;
        }

        private CellState GetCellState((int X, int Y) coord)
        {
            if (_destroyCellsSystem.DestroyedCells.Contains(coord))
            {
                return CellState.Destroyed;
            }
            if (_cellsMovingSystem.MovingContainer.IsMoving(coord))
            {
                return CellState.Moving;
            }
            if (_cellsMovingSystem.MovingContainer.IsFalling(coord))
            {
                return CellState.Falling;
            }
            if (!_cellsContainer.TryGetValue(coord, out var state))
            {
                return CellState.None;
            }
            return CellState.Idle;
        }
    }
}