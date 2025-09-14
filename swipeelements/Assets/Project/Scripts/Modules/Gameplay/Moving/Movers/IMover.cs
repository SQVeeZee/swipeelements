using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Gameplay
{
    public interface IMover
    {
        bool CanStart(CellObject cell);
        void UpdateTarget(CellObject cell, Vector3 newTarget);
        UniTask RunAsync(MovingTileState state, CancellationToken cancellationToken);
        MovingTileState PrepareState(CellObject cell);
        void Clear();
    }
}