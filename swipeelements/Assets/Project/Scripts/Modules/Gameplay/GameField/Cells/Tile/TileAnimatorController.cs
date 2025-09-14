using System.Collections.Generic;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class TileAnimatorController
    {
        private readonly IdleTimer _idleTimer;
        private readonly CellsContainer _cellsContainer;
        private readonly System.Random _random = new();

        [Inject]
        private TileAnimatorController(
            IdleTimer idleTimer,
            CellsContainer cellsContainer)
        {
            _idleTimer = idleTimer;
            _cellsContainer = cellsContainer;
        }

        public void Initialize()
        {
            _idleTimer.OnTimerTick += IdleTimerTickHandler;
            _idleTimer.Initialize();
        }

        public void Dispose()
        {
            _idleTimer.OnTimerTick -= IdleTimerTickHandler;
            _idleTimer.Terminate();
        }

        private void IdleTimerTickHandler(int fieldPercentage)
        {
            var tiles = new List<TileCellObject>();
            foreach (var cell in _cellsContainer)
            {
                if (cell is TileCellObject { Info: { CellState: CellState.Idle } } tileCellObject)
                {
                    tiles.Add(tileCellObject);
                }
            }
            if (tiles.Count == 0)
            {
                return;
            }

            SelectTilesPlayIdleAnimations(tiles, fieldPercentage);
        }

        private void SelectTilesPlayIdleAnimations(List<TileCellObject> tiles, int fieldPercentage)
        {
            var count = Mathf.CeilToInt(tiles.Count * (fieldPercentage / 100f));
            var selected = new HashSet<int>();

            while (selected.Count < count && selected.Count < tiles.Count)
            {
                var index = _random.Next(tiles.Count);
                if (selected.Add(index))
                {
                   tiles[index].AnimatorProcessor.PlayIdle();
                }
            }
        }
    }
}