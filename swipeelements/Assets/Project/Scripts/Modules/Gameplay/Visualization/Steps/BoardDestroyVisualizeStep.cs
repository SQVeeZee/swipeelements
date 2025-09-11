using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public class BoardDestroyVisualizeStep : VisualizeStep<BoardDestroyStep>
    {
        public BoardDestroyVisualizeStep(
            StepsVisualizer visualizer,
            BoardDestroyStep step) : base(visualizer, step)
        {
        }

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
        {
            await DestroyCellsAsync(Step.DestroyedCells, cancellationToken);
        }

        private async UniTask DestroyCellsAsync(HashSet<(int X, int Y)> coords, CancellationToken cancellationToken)
        {
            var tasks = new List<UniTask>();
            foreach (var coord in coords)
            {
                tasks.Add(CellsContainer.DestroyAsync(coord, cancellationToken));
            }
            Debug.Log(cancellationToken.IsCancellationRequested);
            await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellationToken);
        }
    }
}