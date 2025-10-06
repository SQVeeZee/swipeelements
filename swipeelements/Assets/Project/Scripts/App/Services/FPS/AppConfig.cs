using UnityEngine;

namespace Project.FPS
{
    [CreateAssetMenu(fileName = "app_config", menuName = "Configs/app_config")]
    public class AppConfig : ScriptableObject
    {
        [SerializeField]
        private int _defaultFPS = 60;

        public int DefaultFPS => _defaultFPS;
    }
}