using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class CellsContainer : ItemContainer<CellObject>
    {
        private CellsFactory _factory;
        private BoardSettings _boardSettings;
        private CellOrderController _cellOrderController;

        [Inject]
        public void Construct(
            CellsFactory factory,
            BoardSettings boardSettings,
            CellOrderController cellOrderController)
        {
            _cellOrderController = cellOrderController;
            _boardSettings = boardSettings;
            _factory = factory;
        }

        protected override async UniTask DestroyRemoveAsync((int X, int Y) coord, CellObject item, CancellationToken cancellationToken)
        {
            try
            {
                await item.DestroyCellAsync(cancellationToken);
            }
            catch (OperationCanceledException e) { }
            finally
            {
                ReturnItem(item);
            }
        }

        public override CellObject Spawn(MergesCell cell, (int X, int Y) coord)
        {
            var cellObject = base.Spawn(cell, coord);
            return cellObject;
        }

        protected override CellObject CreateItem(MergesCell cell, (int X, int Y) coord)
        {
            var rootContainer = _boardSettings.CellsContainer;
            var position = _boardSettings.GetCellPosition(coord);
            var cellObject = _factory.Create(cell, position, rootContainer);
            _cellOrderController.ApplyCellSortOrder(cellObject, coord);
            return cellObject;
        }

        public void Swap((int X, int Y) coord1, (int X, int Y) coord2)
        {
            if (!CoordToCell.TryGetValue(coord1, out var cell1))
            {
                throw new Exception($"Can't swap cells. Reason: [{coord1.X}:{coord1.Y}] doesn't exist");
            }

            if (!CoordToCell.TryGetValue(coord2, out var cell2))
            {
                throw new Exception($"Can't swap cells. Reason: [{coord2.X}:{coord2.Y}] doesn't exist");
            }

            CellToCoord[cell2] = coord1;
            CellToCoord[cell1] = coord2;

            CoordToCell[coord1] = cell2;
            CoordToCell[coord2] = cell1;
        }

        public Vector3 GetCellPosition((int X, int Y) coord) => _boardSettings.GetCellPosition(coord);
        protected override void ReturnItem(CellObject item) => _factory.Return(item);
    }
}