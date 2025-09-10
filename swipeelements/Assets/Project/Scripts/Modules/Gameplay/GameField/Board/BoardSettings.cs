using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public class BoardSettings : MonoBehaviour
    {
        [SerializeField]
        private BoardObject _boardObject;
        [SerializeField]
        private float _cellSize = 1f;

        public float CellSize => _cellSize;

        public Transform CellsContainer => _boardObject.CellsContainer;

        public void InitializeSettings(ILevelData levelData) => _boardObject.SetupBoard(levelData, _cellSize);
        public Vector3 GetCellPosition((int X, int Y) coord) => _boardObject.GetCellPosition(coord);
    }
}