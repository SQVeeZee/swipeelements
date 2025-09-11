using UnityEngine;

namespace Project.Gameplay
{
    public static class CameraViewUtility
    {
        public static T RaycastFromScreen<T>(this ICameraView camera, Vector3 screenPosition) where T : Component
        {
            var worldPoint = camera.ScreenToWorldPoint(screenPosition);
            var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            return hit.collider != null ? hit.collider.GetComponent<T>() : null;
        }

        public static Vector3 WorldToScreenPoint(this ICameraView cameraView, Vector3 position)
            => cameraView.Camera.WorldToScreenPoint(position);

        public static Vector3 ScreenToWorldPoint(this ICameraView cameraView, Vector3 position)
            => cameraView.Camera.ScreenToWorldPoint(position);

        public static float GetDepth(this ICameraView cameraView, Vector3 position)
            => (cameraView.Camera.transform.position - position).z;
    }
}