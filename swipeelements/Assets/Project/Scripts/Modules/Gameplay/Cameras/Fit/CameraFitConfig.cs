using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/camera_fit_config", fileName = "camera_fit_config", order = 0)]
    public class CameraFitConfig : ScriptableObject
    {
        [SerializeField]
        private float _paddingLeft = 0.5f;
        [SerializeField]
        private float _paddingRight = 0.5f;
        [SerializeField]
        private float _paddingTop = 0.5f;
        [SerializeField] private float _paddingBottom = 0.5f;

        public float PaddingLeft => _paddingLeft;
        public float PaddingRight => _paddingRight;
        public float PaddingTop => _paddingTop;
        public float PaddingBottom => _paddingBottom;
    }
}