using System;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class GameplayInputHandler : IInitializable, IDisposable
    {
        private readonly InputController _inputController;
        private readonly CellsContainer _cellsContainer;
        private readonly MergesGame _mergesGame;
        private readonly ICameraView _gameCamera;

        private TileCellObject _startTile;

        [Inject]
        private GameplayInputHandler(
            InputController inputController,
            CellsContainer cellsContainer,
            MergesGame mergesGame,
            [Inject(Id = CameraIds.GameCamera)] ICameraView gameCamera)
        {
            _inputController = inputController;
            _cellsContainer = cellsContainer;
            _mergesGame = mergesGame;
            _gameCamera = gameCamera;
        }

        void IInitializable.Initialize()
        {
            _inputController.OnMouseButtonDown += MouseButtonDownHandler;
            _inputController.OnSwiping += SwipingHandler;
        }

        void IDisposable.Dispose()
        {
            _inputController.OnMouseButtonUp -= MouseButtonDownHandler;
            _inputController.OnSwiping -= SwipingHandler;
        }

        private void MouseButtonDownHandler(Vector2 position) => OnMouseButtonDown(position);
        private void SwipingHandler(SwipeData swipeData) => OnSwiping(swipeData);

        private void OnMouseButtonDown(Vector2 vector2)
        {
            if (!TryRaycast(vector2, out var startTile) || startTile == null)
            {
                return;
            }
            if (!_cellsContainer.TryGetValue(startTile, out _) )
            {
                return;
            }

            _startTile = startTile;
        }

        private void OnSwiping(SwipeData swipeData)
        {
            if (_startTile == null || !_cellsContainer.TryGetValue(_startTile, out var from))
            {
                _startTile = null;
                return;
            }
            var to = swipeData.Direction switch
            {
                SwipeDirection.Up => from.Top(),
                SwipeDirection.Down => from.Bottom(),
                SwipeDirection.Left => from.Left(),
                SwipeDirection.Right => from.Right(),
                _ => throw new ArgumentOutOfRangeException(nameof(swipeData.Direction), swipeData.Direction, null)
            };
            _mergesGame.ApplySwipe(from, to);
            _startTile = null;
        }

        private bool TryRaycast(Vector3 screenPosition, out TileCellObject tileCellObject)
        {
            tileCellObject = _gameCamera.RaycastFromScreen<TileCellObject>(screenPosition);
            return tileCellObject != null;
        }
    }
}