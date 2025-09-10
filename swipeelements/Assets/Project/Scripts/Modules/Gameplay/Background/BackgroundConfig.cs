using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/background_config", fileName = "background_config", order = 0)]
    public class BackgroundConfig : ScriptableObject
    {
        [SerializeField]
        private float _spawnInterval;
        [SerializeField]
        private int _maxBalloonsCount;

        public float SpawnInterval => _spawnInterval;
        public int MaxBalloonsCount => _maxBalloonsCount;
    }
}