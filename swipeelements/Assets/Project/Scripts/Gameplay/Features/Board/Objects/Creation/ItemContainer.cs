using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core.Utility;
using Project.Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public abstract class ItemContainer<TCellObject> : IEnumerable<TCellObject>
        where TCellObject : CellObject
    {
        protected readonly Dictionary<Coord, TCellObject> CoordToCell = new();
        protected readonly Dictionary<TCellObject, Coord> CellToCoord = new();

        public Coord this[TCellObject item] => CellToCoord[item];
        public TCellObject this[Coord coord] => CoordToCell[coord];

        protected abstract TCellObject CreateItem(MergesCell cell, Coord coord);
        protected abstract void ReturnItem(TCellObject item);
        protected abstract UniTask DestroyRemoveAsync(Coord coord, TCellObject item, CancellationToken cancellationToken);

        public virtual TCellObject Spawn(MergesCell cell, Coord coord)
        {
            if (CoordToCell.TryGetValue(coord, out _))
            {
                Remove(coord);
            }

            var cellObject = CreateItem(cell, coord);
            ItemCreated(cell, cellObject, coord);
            return cellObject;
        }

        private void ItemCreated( MergesCell cell, TCellObject cellObject, Coord coord)
        {
            CoordToCell[coord] = cellObject;
            CellToCoord[cellObject] = coord;
            cellObject.Initialize(cell);
        }

        public virtual async UniTask DestroyAsync(Coord coord, CancellationToken cancellationToken)
        {
            var cell = RemoveItem(coord);
            await DestroyRemoveAsync(coord, cell, cancellationToken);
        }

        public virtual void Remove(Coord coord)
        {
            var cell = RemoveItem(coord);
            ReturnItem(cell);
        }

        private TCellObject RemoveItem(Coord coord)
        {
            if (!CoordToCell.TryGetValue(coord, out var cell))
            {
                throw new Exception($"Can't remove cells. Reason: [{coord.X}:{coord.Y}] doesn't exist");
            }

            CellToCoord.Remove(cell);
            CoordToCell.Remove(coord);
            return cell;
        }

        public virtual void Clear()
        {
            CellToCoord.ForEach(x => ReturnItem(x.Key));
            CellToCoord.Clear();
            CoordToCell.Clear();
        }

        public bool TryGetValue(Coord coord, out TCellObject cell) => CoordToCell.TryGetValue(coord, out cell);

        public bool TryGetValue(TCellObject cell, out Coord coord) => CellToCoord.TryGetValue(cell, out coord);

        public void ReplaceInfo(Coord from, Coord to)
        {
            var cell = CoordToCell[from];
            CellToCoord[cell] = to;
        }

        public TCellObject Move(Coord from, Coord to)
        {
            if (!CoordToCell.TryGetValue(from, out var cell1))
            {
                throw new Exception($"Can't move cells. Reason: [{from.X}:{from.Y}] doesn't exist");
            }

            if (CoordToCell.TryGetValue(to, out _))
            {
                throw new Exception($"Can't move cells:[{from.X}:{from.Y}]. Reason: destination cell [{to.X}:{to.Y}] are not empty");
            }

            CellToCoord.Remove(cell1);
            CoordToCell.Remove(from);

            CellToCoord[cell1] = to;
            CoordToCell[to] = cell1;
            return cell1;
        }

        public IEnumerator<TCellObject> GetEnumerator() => CellToCoord.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}