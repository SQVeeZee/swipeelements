using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/tile_idle_timer_config", fileName = "tile_idle_timer_config", order = 0)]
    public class TileIdleTimerConfig : ScriptableObject
    {
        [SerializeField]
        private float _animationsDelay = 3f;
        [SerializeField]
        private int _fieldPercentage = 30;

        public float AnimationsDelay => _animationsDelay;
        public int FieldPercentage => _fieldPercentage;
    }
}