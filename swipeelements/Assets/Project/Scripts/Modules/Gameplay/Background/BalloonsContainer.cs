using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay
{
    public class BalloonsContainer : MonoBehaviour
    {
        [SerializeField]
        private Balloon[] _balloons;

        private readonly List<Balloon> _pooledBalloons = new List<Balloon>(3);
        public int BalloonsInProgress => _balloons.Length - _pooledBalloons.Count;

        public void Initialize()
        {
            foreach (var balloon in _balloons)
            {
                balloon.gameObject.SetActive(false);
                _pooledBalloons.Add(balloon);
            }
        }

        public bool TryCreateBalloon(out Balloon balloon)
        {
            if (!TryGetBalloon(out balloon))
            {
                return false;
            }

            balloon.gameObject.SetActive(true);
            return true;
        }

        private bool TryGetBalloon(out Balloon balloon)
        {
            if (_pooledBalloons.Count > 0)
            {
                balloon = _pooledBalloons[0];
                _pooledBalloons.Remove(balloon);
                balloon.Initialize(Return);
                return true;
            }

            balloon = null;
            return false;
        }

        private void Return(Balloon balloon)
        {
            _pooledBalloons.Add(balloon);
            balloon.gameObject.SetActive(false);
        }
    }
}