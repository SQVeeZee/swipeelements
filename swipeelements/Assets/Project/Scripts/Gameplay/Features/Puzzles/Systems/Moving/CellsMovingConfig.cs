using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/cells_moving_config", fileName = "cells_moving_config")]
    public class CellsMovingConfig : ScriptableObject
    {
        [SerializeField]
        private MoveSettings _falling;
        [SerializeField]
        private MoveSettings _switching;
        [SerializeField]
        private MoveSettings _moving;

        public MoveSettings GetSettings(CellMoveType type) => type switch
        {
            CellMoveType.Falling => _falling,
            CellMoveType.Switching => _switching,
            CellMoveType.Moving => _moving,
            _ => _moving
        };
    }
}