using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Gameplay
{
    public interface IAnimationImplementation
    {
        UniTask AnimateAsync(RectTransform rectTransform, BalloonAnimationConfig config, CancellationToken cancellationToken);
    }
}