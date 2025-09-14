using UnityEngine;

namespace Project.Gameplay
{
    public class MovingTileState
    {
        public CellObject Cell;
        public Vector3 Start;
        public Vector3 End;

        public float Duration;
        public float Elapsed;
        public AnimationCurve Curve;

        public bool IsRunning;

        public float Acceleration;
        public float MaxSpeed;
    }
}