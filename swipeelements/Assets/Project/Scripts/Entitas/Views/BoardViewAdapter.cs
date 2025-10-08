using System.Threading;
using Project.Gameplay;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Project.Entitas
{
    public sealed class BoardViewAdapter : IBoardView
    {
        private readonly CellsContainer _cellsContainer;
        private readonly BoardSettings _boardSettings;
        private readonly CellsMovingSystem _cellsMovingSystem;

        [Inject]
        private BoardViewAdapter(
            CellsContainer cellsContainer,
            BoardSettings boardSettings,
            CellsMovingSystem cellsMovingSystem)
        {
            _cellsContainer = cellsContainer;
            _boardSettings = boardSettings;
            _cellsMovingSystem = cellsMovingSystem;
        }

        public ICellView SpawnTile(CellType type, Coord coord)
        {
            var cellObj = _cellsContainer.Spawn(new MergesCell(type), coord);
            cellObj.transform.position = _boardSettings.GetCellPosition(coord);
            return new TileViewAdapter(_cellsContainer, cellObj);
        }

        public void MoveTile(MoveData moveData)
        {
            _cellsMovingSystem.MoveTileAsync(moveData, new CancellationToken());
        }

        public void MoveTile(ICellView view, Coord coord)
        {
            var v = (TileViewAdapter)view;
            // только вью-телепорт (логика координат уже изменилась в ECS)
            v.Tile.transform.position = _boardSettings.GetCellPosition(coord);

            // если хочешь, чтобы контейнер держал актуальную карту:
            if (_cellsContainer.TryGetValue(v.Tile, out var _))
            {
                _cellsContainer.ReplaceInfo(_cellsContainer[v.Tile], coord); // обновим CellToCoord
            }
        }

        public void DespawnTile(ICellView view)
        {
            var v = (TileViewAdapter)view;
            if (_cellsContainer.TryGetValue(v.Tile, out var coord))
                _cellsContainer.Remove(coord);           // вернёт в пул и очистит словари
            else
                _cellsContainer.Clear(); // аварийный fallback, по желанию
        }

        private sealed class TileViewAdapter : ICellView
        {
            public readonly CellObject Tile;
            private readonly CellsContainer _cells;

            public TileViewAdapter(CellsContainer cells, CellObject tile)
            {
                _cells = cells;
                Tile = tile;
            }

            public void SetPosition(Vector3 position)
            {
                Tile.transform.position = position;
            }
        }
    }
}
