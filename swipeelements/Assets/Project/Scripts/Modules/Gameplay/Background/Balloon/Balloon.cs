using System;
using UnityEngine;

namespace Project.Gameplay
{
    public class Balloon : MonoBehaviour, IMovableAnimated
    {
        [SerializeField]
        private RectTransform _rectTransform;

        private Action<Balloon> _returnToPool;
        public RectTransform MovingObject => _rectTransform;

        public void Initialize(Action<Balloon> returnToPool) => _returnToPool = returnToPool;
        public void Dispose() => _returnToPool?.Invoke(this);
    }
}