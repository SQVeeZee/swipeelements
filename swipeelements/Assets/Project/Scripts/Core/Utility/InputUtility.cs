using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Core.Utility
{
    public static class InputUtility
    {
        private static DisposableInput _current;
        private static int _counter;

        private class DisposableInput : IDisposable
        {
            private readonly EventSystem _eventSystem;

            public DisposableInput(EventSystem eventSystem)
            {
                _eventSystem = eventSystem;
                _eventSystem.enabled = false;
            }

            public void Dispose()
            {
                if (_current != null)
                {
                    _counter--;
                    if (_counter == 0)
                    {
                        _eventSystem.enabled = true;
                        _current = null;
                    }
                }
                else
                {
                    Debug.LogError($"You are trying to dispose instance of {nameof(DisposableInput)} more then one time.");
                }
            }
        }

        public static IDisposable DisableEventSystem()
        {
            if (_counter == 0)
            {
                var eventSystem = EventSystem.current;
                if (eventSystem == null)
                {
                    throw new Exception("Can't find current EventSystem");
                }
                _current = new DisposableInput(eventSystem);
            }
            _counter++;
            return _current;
        }

        public static void ForceDebugDispose() => _current?.Dispose();
    }
}