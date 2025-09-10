using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Gameplay
{
    public class BalloonSinusAnimation : IAnimationImplementation
    {
        private float _screenEdgeX;

        public void Setup(float screenEdgeX) => _screenEdgeX = screenEdgeX;

        public async UniTask AnimateAsync(RectTransform rectTransform, BalloonAnimationConfig config, CancellationToken cancellationToken)
        {
            var startY = Random.Range(config.MinHeight, config.MaxHeight);

            var fromLeft = Random.value > 0.5f;
            var startX = fromLeft ? -_screenEdgeX - 100f : _screenEdgeX + 100f;
            var endX = -startX;

            rectTransform.anchoredPosition = new Vector2(startX, startY);

            var speed = Random.Range(config.MinSpeed, config.MaxSpeed);

            var curves = config.YCurves;
            var curve = curves.Length > 0
                ? curves[Random.Range(0, curves.Length)]
                : AnimationCurve.Linear(0, 0, 1, 0);

            var time = 0f;
            var journey = Mathf.Abs(endX - startX);
            var duration = journey / speed;
            var multiplier = Random.Range(config.CurveMultiplierMin, config.CurveMultiplierMax);

            while (time < duration && !cancellationToken.IsCancellationRequested)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / duration);

                var x = Mathf.Lerp(startX, endX, t);
                var y = startY + curve.Evaluate(t) * multiplier;

                rectTransform.anchoredPosition = new Vector2(x, y);

                await UniTask.Yield(cancellationToken);
            }
        }
    }
}