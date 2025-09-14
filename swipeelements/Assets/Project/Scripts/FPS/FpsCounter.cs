using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.FPS
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private float _deltaTime;

        private void Awake()
        {
            var currentResolutionRefreshRate = Screen.currentResolution.refreshRateRatio;
            var maxRefreshRate = Math.Max((int)Math.Ceiling(currentResolutionRefreshRate.value), 60);
            Application.targetFrameRate = maxRefreshRate;
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            var fps = Mathf.CeilToInt(1.0f / _deltaTime);
            _text.text = $"{fps} FPS";
        }
    }
}