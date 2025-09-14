using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public class BoardDestroyVisualizeStep : VisualizeStep<DestroyCellsStep>
    {
        private readonly DestroyCellsSystem _destroyCellsSystem;

        public BoardDestroyVisualizeStep(
            StepsVisualizer visualizer,
            DestroyCellsStep cellsStep,
            DestroyCellsSystem destroyCellsSystem)
            : base(visualizer, cellsStep) =>
            _destroyCellsSystem = destroyCellsSystem;

        public override async UniTask ApplyAsync(CancellationToken cancellationToken)
        {
            await DestroyCellsAsync(_cellsStep.DestroyedCells, cancellationToken);
        }

        private async UniTask DestroyCellsAsync(HashSet<(int X, int Y)> coords, CancellationToken cancellationToken)
        {
            var tasks = new List<UniTask>();
            foreach (var coord in coords)
            {
                tasks.Add(_destroyCellsSystem.DestroyCellAsync(coord, cancellationToken));
            }
            await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellationToken);
        }
    }
}