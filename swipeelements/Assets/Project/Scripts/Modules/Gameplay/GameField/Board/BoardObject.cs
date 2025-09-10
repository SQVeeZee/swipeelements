using System;
using System.Collections.Generic;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public class BoardObject : MonoBehaviour
    {
        [SerializeField]
        private Transform _cellsContainer;

        private Dictionary<(int X, int Y), Vector3> _positions;

        public Transform CellsContainer => _cellsContainer;

        public void SetupBoard(ILevelData levelData, float cellSize)
            => _positions = GenerateGridPositions(levelData.Columns, levelData.Rows, cellSize);

        public Vector3 GetCellPosition((int X, int Y) coord)
        {
            if (_positions.TryGetValue(coord, out var position))
            {
                return position;
            }
            throw new Exception($"Can't find cell position for {coord}");
        }

        private static Dictionary<(int X, int Y), Vector3> GenerateGridPositions(
            int columns, int rows, float cellSize = 1f)
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
                    var posY = originY + y * cellSize;
                    dict[(x, y)] = new Vector3(posX, posY, 0f);
                }
            }

            return dict;
        }
    }
}