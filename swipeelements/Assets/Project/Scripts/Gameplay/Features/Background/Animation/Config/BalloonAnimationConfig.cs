using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu( menuName = "Configs/balloon_animation_config", fileName = "balloon_animation_config", order = 0)]
    public class BalloonAnimationConfig : ScriptableObject
    {
        [SerializeField]
        private float _minSpeed = 0.5f;
        [SerializeField]
        private float _maxSpeed = 2f;

        [SerializeField]
        private float _minHeight = -2f;
        [SerializeField]
        private float _maxHeight = 2f;

        [SerializeField]
        private AnimationCurve[] _yCurves;
        [SerializeField]
        private float _curveMultiplierMin = 100f;
        [SerializeField]
        private float _curveMultiplierMax = 100f;

        public float MinSpeed => _minSpeed;
        public float MaxSpeed => _maxSpeed;
        public float MinHeight => _minHeight;
        public float MaxHeight => _maxHeight;
        public AnimationCurve[] YCurves => _yCurves;
        public float CurveMultiplierMin => _curveMultiplierMin;
        public float CurveMultiplierMax => _curveMultiplierMax;
    }
}