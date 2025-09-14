using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Gameplay
{
    public abstract class BaseMover : IMover
    {
        protected readonly Dictionary<CellObject, MovingTileState> _activeMoves = new();

        public bool CanStart(CellObject cell) => !_activeMoves.TryGetValue(cell, out var state) || !state.IsRunning;

        public void UpdateTarget(CellObject cell, Vector3 newTarget)
        {
            if (_activeMoves.TryGetValue(cell, out var state) && state.IsRunning)
            {
                state.End = newTarget;
            }
            else
            {
                throw new InvalidOperationException($"Tile [{cell.name}] is not moving. Use RunAsync() instead.");
            }
        }

        public abstract UniTask RunAsync(MovingTileState state, CancellationToken cancellationToken);

        public MovingTileState PrepareState(CellObject cell)
        {
            if (!_activeMoves.TryGetValue(cell, out var state))
            {
                state = new MovingTileState { Cell = cell };
                _activeMoves[cell] = state;
            }
            return state;
        }

        public void Clear() => _activeMoves?.Clear();

        protected void Complete(CellObject cell) => _activeMoves.Remove(cell);
    }
}