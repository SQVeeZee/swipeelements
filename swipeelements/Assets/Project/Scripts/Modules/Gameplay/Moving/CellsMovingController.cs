using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class CellsMovingController
    {
        private readonly CellsMovingConfig _config;

        [Inject]
        private CellsMovingController(
            CellsMovingConfig config)
        {
            _config = config;
        }

        public async UniTask MoveTileAsync(CellObject cell, Vector3 end, CancellationToken cancellationToken)
        {
            var duration = _config.TileMoveDuration;
            var curve = _config.MoveCurve;

            var start = cell.transform.position;
            var startTime = Time.time;

            while (true)
            {
                var t = (Time.time - startTime) / duration;
                if (t >= 1f)
                {
                    break;
                }
                var easedT = curve.Evaluate(t);
                cell.transform.position = Vector3.LerpUnclamped(start, end, easedT);

                await UniTask.Yield(cancellationToken);
            }
            cell.transform.position = end;
        }
    }
}