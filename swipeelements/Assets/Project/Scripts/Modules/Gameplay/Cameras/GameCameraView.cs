using UnityEngine;

namespace Project.Gameplay
{
    public class GameCameraView : BaseCameraView, ICameraFitter
    {
        [SerializeField]
        private CameraFitConfig _config;

        void ICameraFitter.FitCamera(Bounds bounds) => Fit(bounds);

        private void Fit(Bounds bounds)
        {
            var width = bounds.size.x;
            var height = bounds.size.y;
            var aspect = (float)Screen.width / Screen.height;

            var targetWidth = width + _config.PaddingLeft + _config.PaddingRight;
            var targetHeight = height + _config.PaddingTop + _config.PaddingBottom;

            var sizeByHeight = targetHeight / 2f;
            var sizeByWidth = targetWidth / (2f * aspect);

            Camera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
        }
    }
}