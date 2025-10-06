using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using Zenject;
using UnityEngine;

namespace Project.FPS
{
    public class FramerateService : Service
    {
        private readonly AppConfig _appConfig;

        [Inject]
        private FramerateService(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        protected override UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            SetApplicationFramerate();
            return default;
        }

        private void SetApplicationFramerate()
        {
            var rr = Screen.currentResolution.refreshRateRatio;
            var maxRefreshRate = Math.Max((int)Math.Ceiling(rr.value), _appConfig.DefaultFPS);
            Application.targetFrameRate = maxRefreshRate;
        }
    }
}