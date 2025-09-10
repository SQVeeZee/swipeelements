using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Puzzles;
using Zenject;

namespace Project.Gameplay
{
    public class CellsMovingSystem
    {
        private readonly CellsContainer _cellsContainer;
        private readonly CellsMovingController _movingController;
        private readonly CellOrderController _orderController;

        [Inject]
        private CellsMovingSystem(
            CellsContainer cellsContainer,
            CellsMovingController cellsMovingController,
            CellOrderController cellOrderController)
        {
            _cellsContainer = cellsContainer;
            _movingController = cellsMovingController;
            _orderController = cellOrderController;
        }

        public async UniTask SwitchTilesAsync(MoveData switchData, CancellationToken cancellationToken)
        {
            await UniTask.WhenAll(
                MoveTileAsync(_cellsContainer[switchData.From], switchData.To, cancellationToken),
                MoveTileAsync(_cellsContainer[switchData.To], switchData.From, cancellationToken)
            );
            _cellsContainer.Swap(switchData.From, switchData.To);
        }

        public async UniTask MoveTileAsync(MoveData moveDataData, CancellationToken cancellationToken)
        {
            await MoveTileAsync(_cellsContainer[moveDataData.From], moveDataData.To, cancellationToken);
            _cellsContainer.Move(moveDataData.From, moveDataData.To);
        }

        private async UniTask MoveTileAsync(CellObject cellObject, (int X, int Y) to, CancellationToken cancellationToken)
        {
            _orderController.ApplyCellSortOrder(cellObject, to);
            await _movingController.MoveTileAsync(cellObject, _cellsContainer.GetCellPosition(to), cancellationToken);
        }
    }
}