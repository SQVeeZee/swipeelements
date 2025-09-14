using System;
using UnityEngine;

namespace Project.Gameplay
{
    [Serializable]
    public struct MoveSettings
    {
        [SerializeField]
        private float _duration;
        [SerializeField]
        private AnimationCurve _curve;
        [SerializeField]
        private float _acceleration;
        [SerializeField]
        private float _maxSpeed;

        public float Acceleration => _acceleration;
        public float MaxSpeed => _maxSpeed;
        public float Duration => _duration;
        public AnimationCurve Curve => _curve;
    }
}