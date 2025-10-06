using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Project.Gameplay
{
    public class DestroyCellsSystem : ISystemClear
    {
        private readonly CellsContainer _cellsContainer;

        public HashSet<(int X, int Y)> DestroyedCells { get; } = new();

        [Inject]
        private DestroyCellsSystem(CellsContainer cellsContainer) => _cellsContainer = cellsContainer;

        void ISystemClear.Clear() => DestroyedCells.Clear();
        void ISystemClear.Terminate() => DestroyedCells.Clear();

        public async UniTask DestroyCellAsync((int X, int Y) coord, CancellationToken cancellationToken)
        {
            DestroyedCells.Add(coord);
            await _cellsContainer.DestroyAsync(coord, cancellationToken);
            DestroyedCells.Remove(coord);
        }

    }
}