using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class CellsMovingController
    {
        private readonly CellsMovingConfig _config;
        private readonly IMover _linearMover;
        private readonly IMover _fallingMover;

        [Inject]
        private CellsMovingController(
            CellsMovingConfig config,
            LinearMover linearMover,
            FallingMover fallingMover)
        {
            _config = config;
            _linearMover = linearMover;
            _fallingMover = fallingMover;
        }

        public async UniTask MoveTileAsync(CellObject cell, Vector3 target, CellMoveType moveType, CancellationToken cancellationToken)
        {
            var mover = moveType == CellMoveType.Falling ? _fallingMover : _linearMover;
            if (!mover.CanStart(cell))
            {
                throw new System.InvalidOperationException($"Tile [{cell.name}] is already moving. Use UpdateTileMove() instead.");
            }

            var settings = _config.GetSettings(moveType);
            var state = mover.PrepareState(cell);

            state.Start = cell.transform.position;
            state.End = target;

            state.Duration = settings.Duration;
            state.Curve = settings.Curve;

            state.Acceleration = settings.Acceleration;
            state.MaxSpeed = settings.MaxSpeed;

            state.Elapsed = 0f;
            state.IsRunning = true;
            state.MoveType = moveType;

            await mover.RunAsync(state, cancellationToken);
        }

        public void UpdateTileMove(CellObject cell, Vector3 newTarget, CellMoveType moveType)
        {
            var mover = moveType == CellMoveType.Falling ? _fallingMover : _linearMover;
            mover.UpdateTarget(cell, newTarget);
        }

        public void Clear()
        {
            _linearMover.Clear();
            _fallingMover.Clear();
        }
    }
}