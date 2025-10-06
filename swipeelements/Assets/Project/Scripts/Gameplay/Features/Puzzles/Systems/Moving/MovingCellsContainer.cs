using System.Collections.Generic;
using Project.Core.Utility;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public class MovingCellsContainer
    {
        private readonly HashSet<(int X, int Y)> _movingCells = new();
        private readonly HashSet<(int X, int Y)> _fallingCells = new();

        public void Clear()
        {
            _movingCells.Clear();
            _fallingCells.Clear();
        }

        public void AddMoving(MoveData data)
        {
            _movingCells.Add(data.From);
            _movingCells.Add(data.To);
        }
        public void RemoveMoving(MoveData data)
        {
            _movingCells.Remove(data.From);
            _movingCells.Remove(data.To);
        }

        public bool IsMoving((int X, int Y) coord) => _movingCells.Contains(coord);

        public void AddFalling(FallingData data)
        {
            _fallingCells.Add(data.MoveData.From);
            data.Path.ForEach(coord => _fallingCells.Add(coord));
        }

        public void RemoveFalling(FallingData data)
        {
            _fallingCells.Remove(data.MoveData.From);
            data.Path.ForEach(coord => _fallingCells.Remove(coord));
        }

        public bool IsFalling((int X, int Y) coord)
            => _fallingCells.Contains(coord);
    }
}