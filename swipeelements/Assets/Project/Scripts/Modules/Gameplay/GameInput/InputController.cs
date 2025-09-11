using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class InputController : ITickable
    {
        public event Action<Vector2> OnMouseButtonDown;
        public event Action<Vector2> OnMouseButtonUp;
        public event Action<SwipeData> OnSwiping;

        private bool _isSwiping;
        private Vector2 _startPosition;

        private const float MinSwipeDistance = 50f;

        void ITickable.Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseButtonDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseButtonUp();
            }
        }

        private void MouseButtonDown()
        {
            _startPosition = Input.mousePosition;
            OnMouseButtonDown?.Invoke(_startPosition);
            _isSwiping = true;
        }

        private void MouseButtonUp()
        {
            var endPosition = (Vector2)Input.mousePosition;
            OnMouseButtonUp?.Invoke(endPosition);

            if (_isSwiping)
            {
                var swipeData = new SwipeData(_startPosition, endPosition);
                if (swipeData.Distance >= MinSwipeDistance)
                {
                    OnSwiping?.Invoke(swipeData);
                }
            }

            _isSwiping = false;
            _startPosition = Vector2.zero;
        }
    }
}