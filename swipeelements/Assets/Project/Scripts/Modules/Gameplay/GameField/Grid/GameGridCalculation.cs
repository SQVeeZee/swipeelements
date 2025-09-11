using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    public class GameGridCalculation
    {
        private readonly GameGridConfig _gridConfig;
        private readonly IUIGameSafeArea _gameSafeArea;
        private readonly ICameraView _gameCamera;
        private readonly ICameraView _uiCamera;

        [Inject]
        private GameGridCalculation(
            GameGridConfig gridConfig,
            IUIGameSafeArea gameSafeArea,
            [Inject(Id = CameraIds.GameCamera)] ICameraView gameCamera,
            [Inject(Id = CameraIds.UICamera)] ICameraView uiCamera)
        {
            _gridConfig = gridConfig;
            _gameSafeArea = gameSafeArea;
            _gameCamera = gameCamera;
            _uiCamera = uiCamera;
        }

        public Dictionary<(int X, int Y), Vector3> CalculateGridPositions(int columns, int rows)
        {
            var cellSize = _gridConfig.CellSize;
            var anchorPosition = GetAnchorPosition();
            var yAnchorPosition = anchorPosition.y + cellSize * 0.5f;
            return GenerateGridPositions(columns, rows, cellSize, yAnchorPosition);
        }

        public Bounds GetBoardBounds(int columns, int rows)
        {
            var cellSize = _gridConfig.CellSize;
            var width = columns * cellSize;
            var height = rows * cellSize;

            var center = new Vector3(0f, height / 2f, 0f);
            var size = new Vector3(width, height, 0f);

            return new Bounds(center, size);
        }

        private Vector3 GetAnchorPosition()
        {
            var gridPosition = Vector3.zero;
            var worldFieldPosition = _gameSafeArea.GetWorldPositionForField();
            var screenPoint = _uiCamera.WorldToScreenPoint(worldFieldPosition);
            var depth = _gameCamera.GetDepth(gridPosition);
            return _gameCamera.ScreenToWorldPoint(new Vector3(
                screenPoint.x,
                screenPoint.y,
                depth
            ));
        }

        private static Dictionary<(int X, int Y), Vector3> GenerateGridPositions(int columns, int rows, float cellSize, float anchorY)
        {
            var dict = new Dictionary<(int X, int Y), Vector3>();

            var originX = 0f;
            var originY = 0f;
            var xOffset = -(columns - 1) * cellSize / 2f;

            for (var x = 0; x < columns; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    var posX = originX + xOffset + x * cellSize;
                    var posY = originY + y * cellSize + anchorY;
                    dict[(x, y)] = new Vector3(posX, posY, 0f);
                }
            }

            return dict;
        }
    }
}