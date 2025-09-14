using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Gameplay;
using Project.Gameplay.Puzzles;
using Zenject;

public class CellsMovingSystem : ISystemClear
{
    private readonly CellsContainer _cellsContainer;
    private readonly CellsMovingController _movingController;
    private readonly CellOrderController _orderController;

    public MovingCellsContainer MovingContainer { get; } = new();

    [Inject]
    private CellsMovingSystem(
        CellsContainer cellsContainer,
        CellsMovingController movingController,
        CellOrderController orderController)
    {
        _cellsContainer = cellsContainer;
        _movingController = movingController;
        _orderController = orderController;
    }

    void ISystemClear.Clear()
    {
        MovingContainer.Clear();
        _movingController.Clear();
    }

    void ISystemClear.Terminate()
    {
        MovingContainer.Clear();
        _movingController.Clear();
    }

    public async UniTask SwitchTilesAsync(MoveData switchData, CancellationToken cancellationToken)
    {
        MovingContainer.AddMoving(switchData);
        await UniTask.WhenAll(
            AnimateMoveAsync(switchData, CellMoveType.Switching, cancellationToken),
            AnimateMoveAsync(new MoveData(switchData.To, switchData.From), CellMoveType.Switching, cancellationToken));
        MovingContainer.RemoveMoving(switchData);

        _cellsContainer.Swap(switchData.From, switchData.To);
    }

    public async UniTask MoveTileAsync(MoveData moveData, CancellationToken cancellationToken)
    {
        MovingContainer.AddMoving(moveData);
        await AnimateAndApplyMoveAsync(moveData, CellMoveType.Moving, cancellationToken);
        MovingContainer.RemoveMoving(moveData);
    }

    public async UniTask FallTileAsync(FallingData data, CancellationToken cancellationToken)
    {
        MovingContainer.AddFalling(data);
        await AnimateAndApplyMoveAsync(data.MoveData, CellMoveType.Falling, cancellationToken);
        MovingContainer.RemoveFalling(data);
    }

    public void UpdateFallingCell(FallingData data)
    {
        var moveData = data.MoveData;
        var cell = _cellsContainer[moveData.From];
        var position = _cellsContainer.GetCellPosition(moveData.To);
        _movingController.UpdateTileMove(cell, position, CellMoveType.Falling);
    }

    private async UniTask AnimateAndApplyMoveAsync(MoveData moveData, CellMoveType moveType, CancellationToken cancellationToken)
    {
        await AnimateMoveAsync(moveData, moveType, cancellationToken);
        _cellsContainer.Move(moveData.From, moveData.To);
    }

    private async UniTask AnimateMoveAsync(MoveData moveData, CellMoveType moveType, CancellationToken cancellationToken)
    {
        var cell = _cellsContainer[moveData.From];
        var position = _cellsContainer.GetCellPosition(moveData.To);
        await _movingController.MoveTileAsync(cell, position, moveType, cancellationToken);
        _orderController.ApplyCellSortOrder(cell, moveData.To);
    }
}
