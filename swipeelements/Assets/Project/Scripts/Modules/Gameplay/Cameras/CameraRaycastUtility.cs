using UnityEngine;

namespace Project.Gameplay
{
    public static class CameraRaycastUtility
    {
        public static T RaycastFromScreen<T>(Camera camera, Vector3 screenPosition) where T : Component
        {
            var worldPoint = camera.ScreenToWorldPoint(screenPosition);
            var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            return hit.collider != null ? hit.collider.GetComponent<T>() : null;
        }
    }
}