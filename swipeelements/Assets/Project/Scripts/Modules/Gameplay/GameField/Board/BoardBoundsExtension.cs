using UnityEngine;

namespace Project.Gameplay.Puzzles
{
    public static class BoardBoundsExtension
    {
        public static Bounds GetBoardBounds(int columns, int rows, float cellSize)
        {
            var width = columns * cellSize;
            var height = rows * cellSize;

            var center = new Vector3(0f, height / 2f, 0f);
            var size = new Vector3(width, height, 0f);

            return new Bounds(center, size);
        }
    }
}
