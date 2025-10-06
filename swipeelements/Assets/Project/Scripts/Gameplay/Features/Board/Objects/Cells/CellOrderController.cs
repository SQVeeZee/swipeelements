using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class CellOrderController
    {
        private ILevelData _levelData;
        private Dictionary<(int X, int Y), int> _sortOrders;

        public void Initialize(ILevelData levelData)
        {
            _levelData = levelData;
            _sortOrders = new Dictionary<(int X, int Y), int>();

            PrecomputeSortOrders();
        }

        public void ApplyCellSortOrder(CellObject cellObject, (int X, int Y) coord)
        {
            if (!_sortOrders.TryGetValue(coord, out var baseOrder))
            {
                throw new ArgumentException($"Sort order for coord {coord} not found");
            }

            var sortOrder = baseOrder + GetSortOrderByType(cellObject);
            cellObject.SetSortingOrder(sortOrder);
        }

        private void PrecomputeSortOrders()
        {
            for (var y = 0; y < _levelData.Rows; y++)
            {
                for (var x = 0; x < _levelData.Columns; x++)
                {
                    var coord = (X: x, Y: y);
                    var sortOrder = GetSortOrderByCoord(coord);
                    _sortOrders[coord] = sortOrder;
                }
            }
        }

        private int GetSortOrderByCoord((int X, int Y) coord) =>
            coord.Y * _levelData.Columns + coord.X;

        private static int GetSortOrderByType(CellObject cellObject) =>
            cellObject switch
            {
                TileCellObject => 0,
                EmptyCellObject => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(cellObject))
            };
    }

}