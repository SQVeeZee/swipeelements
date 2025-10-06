using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Gameplay
{
    [UsedImplicitly]
    public class GameplayTimer : ITickable
    {
        private class Subscription
        {
            public float Interval;
            public float Elapsed;
            public Action Callback;
        }

        private readonly List<Subscription> _subscriptions = new();

        public void Tick()
        {
            var dt = Time.deltaTime;
            foreach (var sub in _subscriptions)
            {
                sub.Elapsed += dt;
                if (sub.Elapsed >= sub.Interval)
                {
                    sub.Elapsed = 0f;
                    sub.Callback?.Invoke();
                }
            }
        }

        public IDisposable Subscribe(float interval, Action callback)
        {
            var sub = new Subscription
            {
                Interval = interval,
                Elapsed = 0f,
                Callback = callback
            };
            _subscriptions.Add(sub);

            return Disposable.Create(() => _subscriptions.Remove(sub));
        }
    }
}