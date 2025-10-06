using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class LinearMover : BaseMover
    {
        private const float PositionEpsilon = 0.001f;

        public override async UniTask RunAsync(MovingTileState state, CancellationToken cancellationToken)
        {
            state.Elapsed = 0f;

            while (!cancellationToken.IsCancellationRequested)
            {
                state.Elapsed += Time.deltaTime;

                var t = state.Duration > Mathf.Epsilon
                    ? state.Elapsed / state.Duration
                    : 1f;

                if (t >= 1f - PositionEpsilon)
                {
                    break;
                }

                var easedT = state.Curve != null ? state.Curve.Evaluate(t) : t;
                state.Cell.transform.position = Vector3.LerpUnclamped(state.Start, state.End, easedT);

                await UniTask.Yield(cancellationToken);
            }

            state.Cell.transform.position = state.End;
            state.IsRunning = false;
            state.Elapsed = 0f;

            Complete(state.Cell);
        }
    }
}