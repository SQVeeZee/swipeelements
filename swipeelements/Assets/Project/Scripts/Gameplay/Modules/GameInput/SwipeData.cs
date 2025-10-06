using UnityEngine;

namespace Project.Gameplay
{
    public struct SwipeData
    {
        public Vector2 StartPosition { get; }
        public Vector2 EndPosition { get; }
        public float Distance { get; }
        public SwipeDirection Direction { get; }

        public SwipeData(Vector2 start, Vector2 end)
        {
            StartPosition = start;
            EndPosition = end;
            var delta = end - start;
            Distance = delta.magnitude;
            Direction = GetDirection(delta);
        }

        private static SwipeDirection GetDirection(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {
                return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
    }
}