using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/game_grid_config", fileName = "game_grid_config", order = 0)]
    public class GameGridConfig : ScriptableObject
    {
        [SerializeField]
        private float _cellSize;

        public float CellSize => _cellSize;
    }
}