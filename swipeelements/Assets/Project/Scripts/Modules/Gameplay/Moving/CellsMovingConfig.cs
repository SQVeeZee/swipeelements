using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/cells_moving_config", fileName = "cells_moving_config")]
    public class CellsMovingConfig : ScriptableObject
    {
        [SerializeField]
        private float _tileMoveDuration = 0.25f;
        [SerializeField]
        private AnimationCurve _moveCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float TileMoveDuration => _tileMoveDuration;
        public AnimationCurve MoveCurve => _moveCurve;
    }
}