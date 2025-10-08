using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Gameplay.Puzzles
{
    public interface ICellsMovingSystem
    {
        UniTask SwitchTilesAsync(MoveData switchData, CancellationToken cancellationToken);
        UniTask MoveTileAsync(MoveData moveData, CancellationToken cancellationToken);
        UniTask FallTileAsync(FallingData data, CancellationToken cancellationToken);
    }
}