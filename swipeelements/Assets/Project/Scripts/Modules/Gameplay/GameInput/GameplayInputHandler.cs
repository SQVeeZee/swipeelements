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
        private readonly BoardLocker _boardLocker;
        private readonly ICameraView _gameCamera;

        private TileCellObject _startTile;

        [Inject]
        private GameplayInputHandler(
            InputController inputController,
            CellsContainer cellsContainer,
            MergesGame mergesGame,
            BoardLocker boardLocker,
            [Inject(Id = CameraIds.GameCamera)] ICameraView gameCamera)
        {
            _inputController = inputController;
            _cellsContainer = cellsContainer;
            _mergesGame = mergesGame;
            _boardLocker = boardLocker;
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
            if (!_cellsContainer.TryGetValue(startTile, out var coord) || _boardLocker.Contains(coord))
            {
                return;
            }

            _startTile = startTile;
        }

        private void OnSwiping(SwipeData swipeData)
        {
            if (_startTile == null)
            {
                return;
            }
            var from = _cellsContainer[_startTile];
            var to = swipeData.Direction switch
            {
                SwipeDirection.Up => from.Top(),
                SwipeDirection.Down => from.Bottom(),
                SwipeDirection.Left => from.Left(),
                SwipeDirection.Right => from.Right(),
                _ => throw new ArgumentOutOfRangeException(nameof(swipeData.Direction), swipeData.Direction, null)
            };
            if (!_boardLocker.ContainsAny(from, to))
            {
                var lockedCoords = _boardLocker.GetAllLockedCoords();
                _mergesGame.ApplySwipe(from, to, lockedCoords);
            }
            _startTile = null;
        }

        private bool TryRaycast(Vector3 screenPosition, out TileCellObject tileCellObject)
        {
            tileCellObject = CameraRaycastUtility.RaycastFromScreen<TileCellObject>(_gameCamera.Camera, screenPosition);
            return tileCellObject != null;
        }
    }
}