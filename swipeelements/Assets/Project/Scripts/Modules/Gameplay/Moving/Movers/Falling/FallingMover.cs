using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class FallingMover : BaseMover
    {
        private readonly CellsContainer _cellsContainer;
        private readonly GameGridConfig _gridConfig;

        private const float Epsilon = 0.0005f;

        public FallingMover(CellsContainer cellsContainer, GameGridConfig gridConfig)
        {
            _cellsContainer = cellsContainer;
            _gridConfig = gridConfig;
        }

        public override async UniTask RunAsync(MovingTileState state, CancellationToken cancellationToken)
        {
            var verticalVelocity = 0f;
            var elapsedTime = 0f;

            var baseAcceleration = state.Acceleration > 0f ? state.Acceleration : 30f;
            var maxSpeed = state.MaxSpeed > 0f ? state.MaxSpeed : 25f;

            while (!cancellationToken.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;

                var accelerationFactor = 1f;
                if (state.Curve != null && state.Duration > 0f)
                {
                    accelerationFactor = state.Curve.Evaluate(Mathf.Clamp01(elapsedTime / state.Duration));
                }

                verticalVelocity -= baseAcceleration * accelerationFactor * Time.deltaTime;
                verticalVelocity = Mathf.Max(verticalVelocity, -maxSpeed);

                var currentPosition = state.Cell.transform.position;
                var targetYPosition = state.End.y;

                var nextYPosition = currentPosition.y + verticalVelocity * Time.deltaTime;

                var hasBelow = TryGetTileBelow(state.Cell, out var belowCell);
                if (hasBelow)
                {
                    var belowTileY = belowCell.transform.position.y;
                    var minYPositionAllowed = belowTileY + _gridConfig.CellSize + Epsilon;

                    if (nextYPosition < minYPositionAllowed)
                    {
                        nextYPosition = minYPositionAllowed;
                        if (verticalVelocity < 0f)
                        {
                            verticalVelocity = 0f;
                        }
                    }
                }

                if (nextYPosition < targetYPosition)
                {
                    nextYPosition = targetYPosition;
                    if (verticalVelocity < 0f)
                    {
                        verticalVelocity = 0f;
                    }
                }

                currentPosition.y = nextYPosition;
                state.Cell.transform.position = currentPosition;

                var atEnd = Mathf.Abs(currentPosition.y - targetYPosition) <= Epsilon;

                var blockedFromBelow =
                    hasBelow && (targetYPosition < (belowCell.transform.position.y + _gridConfig.CellSize - Epsilon));

                if (atEnd && !blockedFromBelow)
                {
                    break;
                }

                await UniTask.Yield(cancellationToken);
            }

            var finalPosition = state.Cell.transform.position;
            finalPosition.y = Mathf.Max(finalPosition.y, state.End.y);
            state.Cell.transform.position = finalPosition;

            state.IsRunning = false;
            Complete(state.Cell);
        }

        private bool TryGetTileBelow(CellObject cell, out CellObject below)
        {
            if (!_cellsContainer.TryGetValue(cell, out var coord))
            {
                below = null;
                return false;
            }

            var belowCoord = (coord.X, coord.Y - 1);
            return _cellsContainer.TryGetValue(belowCoord, out below);
        }
    }
}