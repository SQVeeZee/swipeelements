using UnityEngine;

namespace Project.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public abstract class BaseCameraView : MonoBehaviour, ICameraView
    {
        [SerializeField]
        private Camera _camera;

        public Camera Camera => _camera;

        private void OnValidate()
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>();
            }
        }
    }
}