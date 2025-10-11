using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Entitas;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class GameplayInputHandler : ISceneModule
    {
        private readonly InputController _inputController;
        private readonly CellsContainer _cellsContainer;
        private readonly ICameraView _gameCamera;

        private readonly InputContext _inputContext;

        private TileCellObject _startTile;

        [Inject]
        private GameplayInputHandler(
            InputController inputController,
            CellsContainer cellsContainer,
            [Inject(Id = CameraIds.GameCamera)] ICameraView gameCamera)
        {
            _inputController = inputController;
            _cellsContainer = cellsContainer;
            _gameCamera = gameCamera;

            _inputContext = Contexts.sharedInstance.input;
        }

        UniTask ISceneModule.InitializeAsync(CancellationToken cancellationToken)
        {
            _inputController.OnMouseButtonDown += MouseButtonDownHandler;
            _inputController.OnSwiping += SwipingHandler;
            return default;
        }

        void ISceneModule.Tick() { }

        void ISceneModule.Dispose()
        {
            _inputController.OnMouseButtonDown -= MouseButtonDownHandler;
            _inputController.OnSwiping -= SwipingHandler;
        }

        private void MouseButtonDownHandler(Vector2 position) => OnMouseButtonDown(position);
        private void SwipingHandler(SwipeData swipeData) => OnSwiping(swipeData);

        private void OnMouseButtonDown(Vector2 screenPos)
        {
            if (!TryRaycast(screenPos, out var startTile) || startTile == null)
                return;

            _startTile = startTile;
        }

        private void OnSwiping(SwipeData swipeData)
        {
            if (_startTile == null)
            {
                _startTile = null;
                return;
            }

            _inputContext.CreateEntity().AddSwipeEvent(_startTile, swipeData.Direction);
            _startTile = null;
        }

        private bool TryRaycast(Vector3 screenPosition, out TileCellObject tileCellObject)
        {
            tileCellObject = _gameCamera.RaycastFromScreen<TileCellObject>(screenPosition);
            return tileCellObject != null;
        }
    }
}
